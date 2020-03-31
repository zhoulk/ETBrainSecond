/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/4
 * 模块描述： 驱动器实现
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using LT.Container;
using UnityEngine;

namespace LT.MonoDriver
{
    /// <summary>
    /// Mono驱动器
    /// </summary>
    internal sealed class MonoDriver : IMonoDriver
    {
        /// <summary>
        /// 更新
        /// </summary>
        private readonly SortSet<IUpdate, int> update = new SortSet<IUpdate, int>();

        /// <summary>
        /// 延后更新
        /// </summary>
        private readonly SortSet<ILateUpdate, int> lateUpdate = new SortSet<ILateUpdate, int>();

        /// <summary>
        /// 固定更新
        /// </summary>
        private readonly SortSet<IFixedUpdate, int> fixedUpdate = new SortSet<IFixedUpdate, int>();

        /// <summary>
        /// 帧结束
        /// </summary>
        private readonly SortSet<IEndOfFrame, int> endOfFrame = new SortSet<IEndOfFrame, int>();

        /// <summary>
        /// GUI绘制
        /// </summary>
        private readonly SortSet<IOnGUI, int> onGui = new SortSet<IOnGUI, int>();

        /// <summary>
        /// 当应用获取焦点
        /// </summary>
        private readonly SortSet<IOnApplicationFocus, int> applicationFocus = new SortSet<IOnApplicationFocus, int>();

        /// <summary>
        /// 当应用暂停
        /// </summary>
        private readonly SortSet<IOnApplicationPause, int> applicationPause = new SortSet<IOnApplicationPause, int>();

        /// <summary>
        /// 当应用退出
        /// </summary>
        private readonly SortSet<IOnApplicationQuit, int> applicationQuit = new SortSet<IOnApplicationQuit, int>();

        /// <summary>
        /// 释放时需要调用的
        /// </summary>
        private readonly SortSet<IOnDestroy, int> destroy = new SortSet<IOnDestroy, int>();

        /// <summary>
        /// 载入结果集
        /// </summary>
        private readonly HashSet<object> loadSet = new HashSet<object>();

        /// <summary>
        /// 应用程序
        /// </summary>
        private readonly IContainer container;

        /// <summary>
        /// Mono驱动程序
        /// </summary>
        private DriverBehaviour behaviour;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// 主线程调度队列
        /// </summary>
        private readonly Queue<Action> mainThreadDispatcherQueue = new Queue<Action>();

        /// <summary>
        /// 主线程ID
        /// </summary>
        private readonly int mainThreadId;

        /// <summary>
        /// 是否是主线程
        /// </summary>
        public bool IsMainThread
        {
            get
            {
                return mainThreadId == Thread.CurrentThread.ManagedThreadId;
            }
        }

        /// <summary>
        /// 构建一个Mono驱动器
        /// </summary>
        /// <param name="container">容器</param>、
        /// <param name="component">组件</param>
        public MonoDriver(IContainer container, Component component = null)
        {
            Guard.Requires<ArgumentNullException>(container != null);

            mainThreadId = Thread.CurrentThread.ManagedThreadId;
            this.container = container;
            container.OnResolving(DefaultOnResolving);
            container.OnRelease(DefaultOnRelease);
            if (component != null)
            {
                InitComponent(component);
            }
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        /// <param name="component">Unity组件</param>
        private void InitComponent(Component component)
        {
            behaviour = component.gameObject.AddComponent<DriverBehaviour>();
            behaviour.SetDriver(this);
        }

        /// <summary>
        /// 默认的解决事件
        /// </summary>
        /// <param name="binder">绑定数据</param>
        /// <param name="obj">对象</param>
        /// <returns>处理后的对象</returns>
        private void DefaultOnResolving(IBindData binder, object obj)
        {
            if (obj == null)
            {
                return;
            }

            if (binder.IsStatic)
            {
                Attach(obj);
            }
        }

        /// <summary>
        /// 默认的释放事件
        /// </summary>
        /// <param name="binder">绑定数据</param>
        /// <param name="obj">对象</param>
        private void DefaultOnRelease(IBindData binder, object obj)
        {
            if (obj == null)
            {
                return;
            }

            if (binder.IsStatic)
            {
                Detach(obj);
            }
        }

        /// <summary>
        /// 从驱动器中卸载对象
        /// 如果对象使用了增强接口，那么卸载对应增强接口
        /// 从驱动器中卸载对象会引发IDestroy增强接口
        /// </summary>
        /// <param name="obj">对象</param>
        /// <exception cref="ArgumentNullException">当卸载对象为<c>null</c>时引发</exception>
        public void Detach(object obj)
        {
            Guard.Requires<ArgumentNullException>(obj != null);

            if (!loadSet.Contains(obj))
            {
                return;
            }

            ConvertAndRemove(update, obj);
            ConvertAndRemove(lateUpdate, obj);
            ConvertAndRemove(fixedUpdate, obj);
            ConvertAndRemove(endOfFrame, obj);
            ConvertAndRemove(onGui, obj);
            ConvertAndRemove(applicationFocus, obj);
            ConvertAndRemove(applicationPause, obj);
            ConvertAndRemove(applicationQuit, obj);

            if (ConvertAndRemove(destroy, obj))
            {
                ((IOnDestroy)obj).OnDestroy();
            }

            loadSet.Remove(obj);
        }

        /// <summary>
        /// 如果对象实现了增强接口那么将对象装载进对应驱动器
        /// </summary>
        /// <param name="obj">对象</param>
        /// <exception cref="ArgumentNullException">当装载对象为<c>null</c>时引发</exception>
        public void Attach(object obj)
        {
            Guard.Requires<ArgumentNullException>(obj != null);

            if (loadSet.Contains(obj))
            {
                throw new RuntimeException("Object [" + obj + "] is already load.");
            }

            var isLoad = ConvertAndAdd(update, obj, "Update");
            isLoad = ConvertAndAdd(lateUpdate, obj, "LateUpdate") || isLoad;
            isLoad = ConvertAndAdd(fixedUpdate, obj, "FixedUpdate") || isLoad;
            isLoad = ConvertAndAdd(endOfFrame, obj, "EndOfFrame") || isLoad;
            isLoad = ConvertAndAdd(onGui, obj, "OnGUI") || isLoad;
            isLoad = ConvertAndAdd(destroy, obj, "OnDestroy") || isLoad;
            isLoad = ConvertAndAdd(applicationFocus, obj, "OnApplicationFocus") || isLoad;
            isLoad = ConvertAndAdd(applicationPause, obj, "OnApplicationPause") || isLoad;
            isLoad = ConvertAndAdd(applicationQuit, obj, "OnApplicationQuit") || isLoad;

            if (isLoad)
            {
                loadSet.Add(obj);
            }
        }

        /// <summary>
        /// 在主线程中调用
        /// </summary>
        /// <param name="action">代码块执行会处于主线程</param>
        public void MainThread(IEnumerator action)
        {
            Guard.Requires<ArgumentNullException>(action != null);
            if (IsMainThread)
            {
                StartCoroutine(action);
                return;
            }

            lock (syncRoot)
            {
                mainThreadDispatcherQueue.Enqueue(() =>
                {
                    StartCoroutine(action);
                });
            }
        }

        /// <summary>
        /// 在主线程中调用
        /// </summary>
        /// <param name="action">代码块执行会处于主线程</param>
        public void MainThread(Action action)
        {
            Guard.Requires<ArgumentNullException>(action != null);

            if (IsMainThread)
            {
                action.Invoke();
                return;
            }
            MainThread(ActionWrapper(action));
        }

        /// <summary>
        /// 包装器
        /// </summary>
        /// <param name="action">回调函数</param>
        /// <returns>迭代器</returns>
        private IEnumerator ActionWrapper(Action action)
        {
            action.Invoke();
            yield return null;
        }

        /// <summary>
        /// 启动协程
        /// </summary>
        /// <param name="routine">协程内容</param>
        /// <returns>协程</returns>
        /// <exception cref="ArgumentNullException">当<paramref name="routine"/>为<c>null</c>时引发</exception>
        public Coroutine StartCoroutine(IEnumerator routine)
        {
            Guard.Requires<ArgumentNullException>(routine != null);
            if (behaviour == null)
            {
                while (routine.MoveNext())
                {
                    var current = routine.Current as IEnumerator;
                    if (current != null)
                    {
                        StartCoroutine(current);
                    }
                }
                return null;
            }
            return behaviour.StartCoroutine(routine);
        }

        /// <summary>
        /// 停止协程
        /// </summary>
        /// <param name="routine">协程</param>
        /// <exception cref="ArgumentNullException">当<paramref name="routine"/>为<c>null</c>时引发</exception>
        public void StopCoroutine(IEnumerator routine)
        {
            if (behaviour == null)
            {
                return;
            }

            if (routine == null)
            {
                return;
            }

            behaviour.StopCoroutine(routine);
        }

        /// <summary>
        /// 停止协程
        /// </summary>
        /// <param name="routine">协程</param>
        public void StopCoroutine(UnityEngine.Coroutine routine)
        {
            if (behaviour == null)
            {
                return;
            }

            if (routine == null)
            {
                return;
            }

            behaviour.StopCoroutine(routine);
        }

        /// <summary>
        /// 每帧更新
        /// </summary>
        public void Update()
        {
            foreach (var current in update)
            {
                current.Update();
            }

            lock (syncRoot)
            {
                while (mainThreadDispatcherQueue.Count > 0)
                {
                    mainThreadDispatcherQueue.Dequeue().Invoke();
                }
            }
        }

        /// <summary>
        /// 每帧更新后
        /// </summary>
        public void LateUpdate()
        {
            foreach (var current in lateUpdate)
            {
                current.LateUpdate();
            }
        }

        /// <summary>
        /// 固定刷新
        /// </summary>
        public void FixedUpdate()
        {
            foreach (var current in fixedUpdate)
            {
                current.FixedUpdate();
            }
        }

        /// <summary>
        /// GUI绘制时
        /// </summary>
        public void OnGUI()
        {
            foreach (var current in onGui)
            {
                current.OnGUI();
            }
        }

        /// <summary>
        /// 帧结束
        /// </summary>
        public void EndOfFrame()
        {
            foreach (var current in endOfFrame)
            {
                current.EndOfFrame();
            }
        }

        /// <summary>
        /// 当应用获取/失去焦点
        /// </summary>
        /// <param name="focus">是否获得焦点</param>
        public void OnApplicationFocus(bool focus)
        {
            foreach (var current in applicationFocus)
            {
                current.OnApplicationFocus(focus);
            }
        }

        /// <summary>
        /// 当应用暂停
        /// </summary>
        /// <param name="pause">是否暂停</param>
        public void OnApplicationPause(bool pause)
        {
            foreach (var current in applicationPause)
            {
                current.OnApplicationPause(pause);
            }
        }

        /// <summary>
        /// 当应用退出
        /// </summary>
        public void OnApplicationQuit()
        {
            foreach (var current in applicationQuit)
            {
                current.OnApplicationQuit();
            }
        }

        /// <summary>
        /// 当释放时
        /// </summary>
        public void OnDestroy()
        {
            container.Flush();

            foreach (var current in destroy)
            {
                current.OnDestroy();
            }

            update.Clear();
            lateUpdate.Clear();
            fixedUpdate.Clear();
            onGui.Clear();
            destroy.Clear();
            loadSet.Clear();
            applicationFocus.Clear();
            applicationPause.Clear();
            applicationQuit.Clear();
        }

        /// <summary>
        /// 转换到指定目标并且删除
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="sortset">有序集</param>
        /// <param name="obj">对象</param>
        /// <returns>是否成功</returns>
        private bool ConvertAndRemove<T>(SortSet<T, int> sortset, object obj) where T : class
        {
            var ele = obj as T;
            return ele != null && sortset.Remove(ele);
        }

        /// <summary>
        /// 转换到指定目标并且添加
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="sortset">有序集</param>
        /// <param name="obj">对象</param>
        /// <param name="function">获取优先级的函数名</param>
        /// <returns>是否成功</returns>
        private bool ConvertAndAdd<T>(SortSet<T, int> sortset, object obj, string function) where T : class
        {
            T t = obj as T;

            bool flag = t != null;

            if (flag)
            {
                sortset.Add(t, 0);
            }

            return flag;
        }
    }
}