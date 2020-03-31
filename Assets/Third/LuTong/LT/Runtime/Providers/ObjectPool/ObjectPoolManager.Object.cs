/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/08/08
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;

namespace LT.ObjectPool
{
    internal sealed partial class ObjectPoolManager
    {
        /// <summary>
        /// 内部对象。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        private sealed class Object<T> where T : ObjectBase
        {
            private readonly T m_InstanceObject;
            private int m_SpawnCount;

            /// <summary>
            /// 初始化内部对象的新实例。
            /// </summary>
            /// <param name="obj">对象。</param>
            /// <param name="spawned">对象是否已被获取。</param>
            public Object(T obj, bool spawned)
            {
                Guard.Verify<ArgumentException>(obj == null, "Object is invalid.");

                m_InstanceObject = obj;
                m_SpawnCount = spawned ? 1 : 0;

                if (spawned)
                {
                    m_InstanceObject.OnSpawn();
                }
            }

            /// <summary>
            /// 获取对象名称。
            /// </summary>
            public string Name
            {
                get
                {
                    return m_InstanceObject.Name;
                }
            }

            /// <summary>
            /// 获取对象是否被加锁。
            /// </summary>
            public bool Locked
            {
                get
                {
                    return m_InstanceObject.Locked;
                }
                internal set
                {
                    m_InstanceObject.Locked = value;
                }
            }

            /// <summary>
            /// 获取自定义释放检查标记。
            /// </summary>
            public bool CustomCanReleaseFlag
            {
                get
                {
                    return m_InstanceObject.CustomCanReleaseFlag;
                }
            }

            /// <summary>
            /// 获取对象的优先级。
            /// </summary>
            public int Priority
            {
                get
                {
                    return m_InstanceObject.Priority;
                }
                internal set
                {
                    m_InstanceObject.Priority = value;
                }
            }

            /// <summary>
            /// 获取对象上次使用时间。
            /// </summary>
            public DateTime LastUseTime
            {
                get
                {
                    return m_InstanceObject.LastUseTime;
                }
            }

            /// <summary>
            /// 是否正在使用。
            /// </summary>
            public bool IsInUse
            {
                get
                {
                    return m_SpawnCount > 0;
                }
            }

            /// <summary>
            /// 获取对象的获取计数。
            /// </summary>
            public int SpawnCount
            {
                get
                {
                    return m_SpawnCount;
                }
            }

            /// <summary>
            /// 查看对象。
            /// </summary>
            /// <returns>对象。</returns>
            public T Peek()
            {
                return m_InstanceObject;
            }

            /// <summary>
            /// 获取对象。
            /// </summary>
            /// <returns>对象。</returns>
            public T Spawn()
            {
                m_SpawnCount++;
                m_InstanceObject.LastUseTime = DateTime.Now;
                m_InstanceObject.OnSpawn();
                return m_InstanceObject;
            }

            /// <summary>
            /// 回收对象。
            /// </summary>
            public void Unspawn()
            {
                m_InstanceObject.OnUnspawn();
                m_InstanceObject.LastUseTime = DateTime.Now;
                m_SpawnCount--;

                Guard.Verify<LogicException>(m_SpawnCount < 0, "Spawn count is less than 0.");
            }

            /// <summary>
            /// 释放对象。
            /// </summary>
            /// <param name="isShutdown">是否是关闭对象池时触发。</param>
            public void Release(bool isShutdown)
            {
                m_InstanceObject.OnRelease(isShutdown);
            }
        }
    }
}