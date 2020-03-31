/*
*    描述:
*          1.任务表
*
*    开发人: 邓平
*/
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LtFramework.Util.Task
{
    public enum AddTaskState
    {
        Add,
        Exist,
        Error,
    }

    public class TaskTable
    {
        //任务列表
        private readonly Dictionary<string, List<ITask>> _TaskDic = new Dictionary<string, List<ITask>>();

        //任务移除列表
        private readonly List<ITask> _RemoveTask = new List<ITask>();

        //同一任务 单个顺序执行表
        private readonly Dictionary<string, List<ITask>> _SangleTaskDic = new Dictionary<string, List<ITask>>();

        //同一任务 单个移除列表
        private readonly List<ITask> _SangleRemoveTask = new List<ITask>();


        /// <summary>
        /// 添加任务列表
        /// </summary>
        /// <param UIName="task"></param>
        public AddTaskState AddTask(ITask task)
        {
            string type = task.GetType().FullName;
            if (task.TaskType == TaskType.Order)
            {
                if (!_SangleTaskDic.ContainsKey(type))
                {
                    List<ITask> taskList = new List<ITask>();
                    _SangleTaskDic[type] = taskList;

                }

                _SangleTaskDic[type].Add(task);
                return AddTaskState.Add;
            }
            else
            {
                if (!_TaskDic.ContainsKey(type))
                {
                    List<ITask> taskList = new List<ITask>();
                    _TaskDic[type] = taskList;
                }

                //判断任务类型
                if (task.TaskType == TaskType.Add)
                {
                    //添加
                    _TaskDic[type].Add(task);
                    return AddTaskState.Add;
                }
                else if (task.TaskType == TaskType.Replace)
                {
                    //替换
                    List<ITask> tasks = _TaskDic[type];
                    tasks.ForEach(p => p.EndInterrupt());
                    tasks.Clear();
                    tasks.Add(task);
                    return AddTaskState.Add;
                }
                else if (task.TaskType == TaskType.Discard)
                {
                    List<ITask> tasks = _TaskDic[type];
                    if (tasks.Count > 0)
                    {
                        Debug.LogWarning("有该任务正在执行 ; " + task);
                        return AddTaskState.Exist;
                    }
                    else
                    {
                        _TaskDic[type].Add(task);
                        return AddTaskState.Add;
                    }

                    //丢弃
                }

                return AddTaskState.Error;
            }
        }

        /// <summary>
        /// 移除任务列表
        /// </summary>
        /// <param UIName="task">指定移除任务</param>
        public void RemoveTask(ITask task)
        {
            string type = task.GetType().FullName;
            if (task.TaskType == TaskType.Order)
            {
                if (_SangleTaskDic.ContainsKey(type))
                {
                    task.EndInterrupt();
                    //将任务添加到移除列表
                    _SangleRemoveTask.Add(task);
                }
                else
                {
                    Debug.LogError("任务字典不存在任务表 " + task);
                }
            }
            else
            {
                if (_TaskDic.ContainsKey(type))
                {
                    if (_TaskDic[type].Contains(task))
                    {
                        task.EndInterrupt();
                        //将任务添加到移除列表
                        _RemoveTask.Add(task);
                    }
                    else
                    {
                        Debug.LogError("任务表不存在任务 " + task);
                    }
                }
                else
                {
                    Debug.LogError("任务字典不存在任务表 " + task);
                }
            }

            Remove();
        }

        private void Remove()
        {
            if (_RemoveTask.Count > 0)
            {
                foreach (ITask task in _RemoveTask)
                {
                    RemoveTaskToDic(_TaskDic, task);
                }

                _RemoveTask.Clear();
            }

            if (_SangleRemoveTask.Count > 0)
            {
                foreach (ITask task in _SangleRemoveTask)
                {
                    RemoveTaskToDic(_SangleTaskDic, task);
                }

                _SangleRemoveTask.Clear();
            }
        }


        /// <summary>
        ///  移除最后添加的指定类型的任务
        /// </summary>
        /// <param UIName="type"></param>
        public void RemoveLastTask(Type type)
        {
            if (type.IsSubclassOf(typeof(ITask)))
            {
                if (_TaskDic.ContainsKey(type.FullName))
                {
                    List<ITask> taskLis = _TaskDic[type.FullName];
                    ITask task = taskLis[taskLis.Count - 1];
                    task.EndInterrupt();
                    //将任务添加到移除列表
                    _RemoveTask.Add(task);
                }
                else if (_SangleTaskDic.ContainsKey(type.FullName))
                {
                    List<ITask> taskLis = _SangleTaskDic[type.FullName];
                    int len = taskLis.Count;
                    ITask task = taskLis[taskLis.Count - 1];
                    //只要一个任务,中断该任务
                    if (len == 1)
                    {
                        task.EndInterrupt();
                    }

                    //将任务添加到移除列表
                    _SangleRemoveTask.Add(task);
                }
                else
                {
                    Debug.LogError("没该任务在执行列表中");
                }
            }
            else
            {

                Debug.LogError("传递的 type " + type + " 不是Itask的子类");
            }

            Remove();
        }

        /// <summary>
        /// 重字典中移除任务
        /// </summary>
        /// <param UIName="task"></param>
        private void RemoveTaskToDic(Dictionary<string, List<ITask>> dic, ITask task)
        {
            string type = task.GetType().FullName;
            if (dic.ContainsKey(type))
            {
                List<ITask> taskList = dic[type];
                if (taskList != null && taskList.Contains(task))
                {
                    taskList.Remove(task);
                    if (taskList.Count == 0)
                    {
                        //dic.Remove(type);
                    }

                    Debug.Log("移除任务 " + task);
                }
                else
                {
                    Debug.Log("该任务已经移除" + task);
                }

                //if (taskList.Count == 0)
                //{
                //    dic.Remove(type);
                //}
            }
            else
            {
                Debug.Log("该任务已经移除" + task);
            }

        }


        public void Update()
        {
            List<string> keys = _TaskDic.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                for (int j = 0; j < _TaskDic[keys[i]].Count; j++)
                {
                    _TaskDic[keys[i]][j].UpdateBase();
                }
            }

            keys = _SangleTaskDic.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                Debug.Log(keys[i]);
                _SangleTaskDic[keys[i]][0].UpdateBase();
            }

        }

        public void FixedUpdate()
        {

            foreach (List<ITask> list in _TaskDic.Values)
            {
                list.ForEach(p => p.FixedUpdateBase());
            }

            foreach (List<ITask> list in _SangleTaskDic.Values)
            {
                list[0].FixedUpdateBase();
            }
        }


        /// <summary>
        /// 情况指定任务列表
        /// </summary>
        /// <param UIName="task"></param>
        public void ClearTask(ITask task)
        {
            string type = task.GetType().FullName;
            if (_TaskDic.ContainsKey(type))
            {
                List<ITask> tmp = _TaskDic[type];
                tmp.ForEach(p => p.EndInterrupt());
                _TaskDic[type].Clear();
            }

        }

        public void ClearTaskAndComplete(ITask task)
        {
            string type = task.GetType().FullName;
            if (_TaskDic.ContainsKey(type))
            {
                List<ITask> tmp = _TaskDic[type];
                tmp.ForEach(p => p.EndComplete());
                _TaskDic[type].Clear();
            }

        }

        public void ClearTask(Type type)
        {
            if (type.IsSubclassOf(typeof(ITask)))
            {
                if (_TaskDic.ContainsKey(type.FullName))
                {
                    List<ITask> tasks = _TaskDic[type.FullName];
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        tasks[i].EndInterrupt();
                    }

                    _TaskDic[type.FullName].Clear();
                }
                else if (_SangleTaskDic.ContainsKey(type.FullName))
                {
                    List<ITask> tasks = _SangleTaskDic[type.FullName];
                    tasks[0].EndInterrupt();
                    _TaskDic[type.FullName].Clear();
                }
                else
                {
                    Debug.LogWarning("没有该类型的任务");
                }

            }
            else
            {

                Debug.LogError("传递的 type " + type + " 不是Itask的子类");
            }
        }

        public void Clear()
        {
            _TaskDic.Clear();
            _RemoveTask.Clear();
            _SangleTaskDic.Clear();
            _SangleRemoveTask.Clear();
        }

        /// <summary>
        /// 情况任务列表
        /// </summary>
        public void ClearTask()
        {
            List<ITask> tmpList = new List<ITask>();
            foreach (List<ITask> list in _TaskDic.Values)
            {
                tmpList.AddRange(list);
            }

            for (int i = 0; i < tmpList.Count; i++)
            {
                ClearTask(tmpList[i]);
            }
        }

        public void ClearTaskAndComplete()
        {
            List<ITask> tmpList = new List<ITask>();
            foreach (List<ITask> list in _TaskDic.Values)
            {
                tmpList.AddRange(list);
            }

            for (int i = 0; i < tmpList.Count; i++)
            {
                ClearTaskAndComplete(tmpList[i]);
            }
        }

        /// <summary>
        /// 得到当前任务数量
        /// </summary>
        /// <param UIName="type"></param>
        /// <returns></returns>
        public int GetTaskNumber(Type type)
        {
            if (type.IsSubclassOf(typeof(ITask)))
            {
                if (_TaskDic.ContainsKey(type.FullName))
                {
                    return _TaskDic[type.FullName].Count;
                }
                else if (_SangleTaskDic.ContainsKey(type.FullName))
                {
                    return _SangleTaskDic[type.FullName].Count;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                Debug.LogError("传递的 type " + type + " 不是Itask的子类");
                return 0;
            }
        }
    }
}
