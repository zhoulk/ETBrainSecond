/*
 *    描述:
 *          1. UI命令更新脚本
 *
 *    开发人: 邓平
 */
using System.Collections.Generic;
using UnityEngine;

namespace LtFramework.UI
{
    public class CommandInvoker
    {
        /// <summary>
        /// ui操作命令
        /// </summary>
        private readonly Dictionary<int, List<UICommand>> _UICommandDic = new Dictionary<int, List<UICommand>>();

        private readonly List<int> _UICommandList = new List<int>();

        /// <summary>
        /// 当前执行的UI命令
        /// </summary>
        private readonly Dictionary<int, UICommand> _CurrentExecCommandDic = new Dictionary<int, UICommand>();

        private readonly List<int> _CurrentExecCommandList = new List<int>();

        /// <summary>
        /// 添加UI命令
        /// </summary>
        /// <param UIName="command"></param>
        internal void AddCommand(UICommand command)
        {
            if (!_UICommandDic.ContainsKey(command.ID) &&
                !_CurrentExecCommandDic.ContainsKey(command.ID))
            {
                if (command.commandType == UICommandType.OpenUI
                    || command.commandType == UICommandType.CloseUI
                    || command.commandType == UICommandType.FreezeUI
                    || command.commandType == UICommandType.ThawUI
                )
                {
                    command.ExecuteStart();
                    if (command.timer.time > 0)
                    {
                        AddCommandToList(command);
                    }
                    else
                    {
                        //command.Execute();
                        command.ExecuteDone();
                    }
                }
                else
                {
                    AddCommandToList(command);
                }
            }
            else
            {
                AddCommandToList(command);
            }
        }

        private void AddCommandToList(UICommand command)
        {
            List<UICommand> templist = null;
            if (!_UICommandDic.TryGetValue(command.ID, out templist) ||
                System.Object.ReferenceEquals(templist, null))
            {
                _UICommandDic[command.ID] = new List<UICommand>();
            }

            _UICommandDic[command.ID].Add(command);

            if (!_UICommandList.Contains(command.ID))
            {
                _UICommandList.Insert(0, command.ID);
            }

        }


        /// <summary>
        /// 添加UI命令道倒数第二个
        /// </summary>
        /// <param UIName="command"></param>
        internal void AddCommandLastSecond(UICommand command)
        {
            List<UICommand> templist = null;
            if (!_UICommandDic.TryGetValue(command.ID, out templist) || System.Object.ReferenceEquals(templist, null))
            {
                _UICommandDic[command.ID] = new List<UICommand>();
            }

            int count = _UICommandDic[command.ID].Count;
            if (count > 0 &&
                (_UICommandDic[command.ID][count - 1].commandType == UICommandType.OpenEndUI ||
                 _UICommandDic[command.ID][count - 1].commandType == UICommandType.CloseEndUI ||
                 _UICommandDic[command.ID][count - 1].commandType == UICommandType.FreezeEndUI ||
                 _UICommandDic[command.ID][count - 1].commandType == UICommandType.ThawEndUI
                ))
            {
                _UICommandDic[command.ID].Insert(count - 1, command);

            }
            else
            {
                _UICommandDic[command.ID].Add(command);
            }


            if (!_UICommandList.Contains(command.ID))
            {
                _UICommandList.Insert(0, command.ID);
            }

        }

        internal void CancelShowUI(IBaseUIForm optionUI)
        {
            if (_CurrentExecCommandDic.ContainsKey(optionUI.ID))
            {
                _CurrentExecCommandDic[optionUI.ID].timer.SetTimer(0);
                Debug.Log(_CurrentExecCommandDic[optionUI.ID].timer.timer);
            }
            else
            {
                Debug.LogError("当前UI " + optionUI.name + "没有执行 ShowUI ");
            }
        }

        internal void Update()
        {
            ExecCommand();
        }


        private void ExecCommand()
        {
            AddExecCommand();
            ExecCurrentCommand();
        }

        /// <summary>
        /// 添加执行命令
        /// </summary>
        private void AddExecCommand()
        {
            //添加执行命名
            for (int i = _UICommandList.Count - 1; i >= 0; i--)
            {
                if (!_CurrentExecCommandDic.ContainsKey(_UICommandList[i]))
                {
                    UICommandDicToCurrent(_UICommandList[i]);
                }
            }
        }

        /// <summary>
        /// 从命令缓存中添加命名到执行列表
        /// </summary>
        /// <param UIName="key"></param>
        private bool UICommandDicToCurrent(int key)
        {
            List<UICommand> commandList = null;
            UICommand command = null;
            if (_UICommandDic.TryGetValue(key, out commandList) &&
                !System.Object.ReferenceEquals(commandList, null))
            {
                command = commandList[0];
                commandList.RemoveAt(0);
                if (commandList.Count <= 0)
                {
                    _UICommandDic.Remove(key);
                    _UICommandList.Remove(key);
                }
            }
            else
            {
                //Debug.LogError("当前命令链表为空 : " + key);
            }

            if (!System.Object.ReferenceEquals(command, null))
            {
                _CurrentExecCommandDic.Add(key, command);
                _CurrentExecCommandList.Insert(0, key);
                return true;
            }
            else
            {
                Debug.LogError("从链表取出的命令为null : " + key);
            }

            return false;
        }

        /// <summary>
        /// 执行当前命令
        /// </summary>
        private void ExecCurrentCommand()
        {
            for (int i = _CurrentExecCommandList.Count - 1; i >= 0; i--)
            {
                ExecOnceCommand(_CurrentExecCommandList[i]);
            }
        }

        /// <summary>
        /// 执行每一个命令
        /// </summary>
        /// <param UIName="key"></param>
        private void ExecOnceCommand(int key)
        {
            UICommand command = _CurrentExecCommandDic[key];
            //Debug.Log(key);
            if (command != null)
            {
                command.ExecuteStart();
                command.Execute();
                if (command.UpdateTimer())
                {
                    command.ExecuteDone();
                    command.Recycle(command);
                    _CurrentExecCommandDic.Remove(key);
                    _CurrentExecCommandList.Remove(key);
                    //_CurrentExecCommandDic.Remove(key);
                    //if (_UICommandDic.ContainsKey(key) && !removeCommand.Contains(key))
                    //{
                    //    if (UICommandDicToCurrent(key))
                    //    {
                    //        ExecOnceCommand(key);
                    //    }
                    //}
                }

            }
            else
            {
                Debug.LogError("当前执行命令为null :" + key);
            }
        }
    }
}
