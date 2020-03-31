/*
 *    描述:
 *          1. UI命令脚本
 *
 *    开发人: 邓平
 */
using System;
using LtFramework.ResKit;
using LtFramework.Util.Pools;
using LtFramework.Util.Tools;
using UnityEngine;

namespace LtFramework.UI
{
    public class UICommand : IClassPoolNode<UICommand>
    {
        public ClassObjectPool<UICommand> Pool { get; set; }
        public Func<UICommand,bool> Recycle { get; set; }

        public void Reset()
        {
            commandData.Reset();
            timer.SetTimer(0);
            isExec = false;
        }

        public int ID => commandData.ID;

        internal LtTimer timer = new LtTimer();

        //是否执行
        internal bool isExec = false;

        private UICommandData commandData = new UICommandData();

        public UICommandData getcommandData => commandData;

        public void InitDate(UICommandType type, IBaseUIForm uiForm,
            float timer = 0, params object[] paramValues)
        {
            commandData.Init(type, uiForm, timer, paramValues);
            this.timer.SetTimer(timer);
            isExec = false;
        }

        internal void ExecuteStart()
        {
            if (commandData.OptionUI == null) return;

            commandData.OptionUI.timer = this.timer;
            if (commandData.OptionUI.UIStage == UIState.Hide || commandData.OptionUI.UIStage == UIState.Null)
            {
                if (commandType == UICommandType.OpenUI)
                {
                    if (isExec == false)
                    {
                        isExec = true;
                        timer.ResetTimer();

                        UIMonoManager.Instance.AddOpeningUIForm(commandData.OptionUI);
                        commandData.OptionUI.UIStage = UIState.Display;

                        ClildExecStart();
                        commandData.ExecuteStart?.Invoke(timer.time, commandData.ParamVaules);
                    }
                }
                else if (commandType == UICommandType.DestroyUI)
                {
                }
                else
                {
                    Debug.LogError(commandType +" " + GetHashCode());

                    Debug.LogError("该UI未打开,无法执行其他命令,要执行命令请先调用OpenUI  UI:" + commandData.uiName + "command :" +
                                   commandType);
                    Debug.LogError("可能是UI正在打开的状态 调用了CloseUI, 导致先执行了CloseUI, 导致后续操作无效 ,请检测当前UI 关闭逻辑 uiName" +
                                  commandData.uiName);
                }

                return;
            }

            if (isExec == false)
            {
                isExec = true;
                timer.ResetTimer();
                if (commandType == UICommandType.OpenUI)
                {
                    Debug.LogError("当前窗口已经打开,可能多次调用OpenUI  uiName:" + commandData.OptionUI.name);
                    return;
                    //UIMonoManager.Instance.AddOpeningUIForm(commandData.OptionUI);
                    //commandData.OptionUI.UIStage = UIState.Display;
                }
                else if (commandType == UICommandType.CloseUI)
                {
                    commandData.OptionUI.DisableSelect();
                    UIMonoManager.Instance.AddCloseingUIForm(commandData.OptionUI);

                }
                else if (commandType == UICommandType.FreezeUI)
                {
                    commandData.OptionUI.UIStage = UIState.Freeze;
                    commandData.OptionUI.DisableSelect();
                    UIMonoManager.Instance.AddFreezeingUIForm(commandData.OptionUI);
                }
                else if (commandType == UICommandType.ThawUI)
                {
                    commandData.OptionUI.UIStage = UIState.Thaw;
                    UIMonoManager.Instance.AddThawingUIForm(commandData.OptionUI);
                }

                ClildExecStart();
                commandData.ExecuteStart?.Invoke(timer.time, commandData.ParamVaules);

#if UNITY_EDITOR
                if (commandData.ExecuteStart != null)
                {
                    //Debug.Log("Start:" + commandData.optionUI.UIName + " type :" + commandData.commandType + "data :" + commandData.data);
                }
#endif
            }

        }

        internal void Execute()
        {
            if (commandData.OptionUI == null) return;

            if (commandData.OptionUI.UIStage == UIState.Hide) return;

            if (timer.timer > 0)
            {
                ClildExecUpdate(timer.timer, commandData.ParamVaules);
                commandData.ExecuteUpdate?.Invoke(timer.timer, commandData.ParamVaules);

#if UNITY_EDITOR
                if (commandData.ExecuteUpdate != null)
                {
                    //Debug.Log("Update:" + commandData.optionUI.UIName + " type :" + commandData.commandType + "data :" + commandData.data);
                }
#endif
            }
        }

        internal void ExecuteDone()
        {
            if (commandData.OptionUI == null) return;

            if (commandData.OptionUI.UIStage == UIState.Hide)
            {
                if (commandType == UICommandType.DestroyUI)
                {
                    var parent = commandData.OptionUI.UIFormDepend.GetParent;
                    if (parent.Node != null)
                    {
                        parent.Node.UIFormDepend.RemoveChildNode(commandData.OptionUI);
                    }

                    commandData.OptionUI.UIFormDepend.SetParent(null);
                    ObjManager.Release(commandData.OptionUI.gameObject, true, false, false, 0);
                }
                else
                {
                    Debug.LogError("该UI已经关闭,无法执行其他命令 UI:" + commandData.uiName + "command :" + commandType);
                }
            }
            else
            {

                if (commandType == UICommandType.OpenEndUI)
                {
                    UIMonoManager.Instance.RemoveOpeningUIForm(commandData.OptionUI);
                    commandData.OptionUI.EnableSelect(true);
                }
                else if (commandType == UICommandType.CloseEndUI)
                {
                    UIMonoManager.Instance.RemoveCloseingUIForm(commandData.OptionUI);
                    commandData.OptionUI.UIStage = UIState.Hide;
                }
                else if (commandType == UICommandType.FreezeEndUI)
                {
                    UIMonoManager.Instance.RemoveFreezeingUIForm(commandData.OptionUI);
                }
                else if (commandType == UICommandType.ThawEndUI)
                {
                    UIMonoManager.Instance.RemoveThawingUIForm(commandData.OptionUI);
                    commandData.OptionUI.EnableSelect(false);
                }
                else if (commandType == UICommandType.DestroyUI)
                {
                    Debug.LogError("销毁UI之前 请先调用Close : " + commandData.OptionUI.name);
                }
                
                ClildExecUpdate(0, commandData.ParamVaules);
                commandData.ExecuteUpdate?.Invoke(0, commandData.ParamVaules);

                ClildExecEnd();
                commandData.ExecuteEnd?.Invoke(commandData.ParamVaules);

            }


#if UNITY_EDITOR
            if (commandData.ExecuteEnd != null)
            {
                //Debug.Log("End:" + commandData.optionUI.UIName + " type :" + commandData.commandType + "data :" + commandData.data);
            }
#endif

        }

        #region 变量子物体

        private ILtOpenUI[] _ltOpenUIs;
        private ILtShowUI[] _ltShowUIs;
        private ILtCloseUI[] _ltCloseUIs;
        private ILtFreezeUI[] _ltFreezeUIs;
        private ILtThawUI[] _ltThawUIs;

        private void ClildExecStart()
        {
            if (commandType == UICommandType.OpenUI)
            {
                _ltOpenUIs = commandData.OptionUI.GetComponentsInChildren<ILtOpenUI>();
                if (_ltOpenUIs != null)
                {
                    foreach (ILtOpenUI showUI in _ltOpenUIs)
                    {
                        showUI.displayState = DisplayState.OpenUI;
                        showUI.OnOpenUIStart(timer.time, commandData.ParamVaules);
                    }
                }
            }
            else if (commandType == UICommandType.ShowUI)
            {
                _ltShowUIs = commandData.OptionUI.GetComponentsInChildren<ILtShowUI>();
                if (_ltShowUIs != null)
                {
                    foreach (ILtShowUI showUI in _ltShowUIs)
                    {
                        showUI.displayState = DisplayState.ShowUI;
                        showUI.OnShowUIStart(timer.time, commandData.ParamVaules);
                    }
                }
            }
            else if (commandType == UICommandType.CloseUI)
            {
                _ltCloseUIs = commandData.OptionUI.GetComponentsInChildren<ILtCloseUI>();
                if (_ltCloseUIs != null)
                {
                    foreach (ILtCloseUI closeUI in _ltCloseUIs)
                    {
                        closeUI.displayState = DisplayState.CloseUI;
                        closeUI.OnCloseUIStart(timer.time, commandData.ParamVaules);
                    }
                }
            }
            else if (commandType == UICommandType.FreezeUI)
            {
                _ltFreezeUIs = commandData.OptionUI.GetComponentsInChildren<ILtFreezeUI>();
                if (_ltFreezeUIs != null)
                {
                    foreach (ILtFreezeUI freezeUI in _ltFreezeUIs)
                    {
                        freezeUI.displayState = DisplayState.FreezeUI;
                        freezeUI.OnFreezeUIStart(timer.time, commandData.ParamVaules);
                    }
                }
            }
            else if (commandType == UICommandType.ThawUI)
            {
                _ltThawUIs = commandData.OptionUI.GetComponentsInChildren<ILtThawUI>();
                if (_ltThawUIs != null)
                {
                    foreach (ILtThawUI thawUI in _ltThawUIs)
                    {
                        thawUI.displayState = DisplayState.ThawUI;
                        thawUI.OnThawUIStart(timer.time, commandData.ParamVaules);
                    }
                }
            }
        }

        private void ClildExecUpdate(float time, object data)
        {
            if (commandType == UICommandType.OpenUI)
            {
                //_ltOpenUIs = commandData.optionUI.GetComponentsInChildren<ILtOpenUI>();
                if (_ltOpenUIs != null)
                {
                    foreach (ILtOpenUI openUI in _ltOpenUIs)
                    {
                        openUI.displayState = DisplayState.OpenUI;
                        openUI.OnOpenUIUpdata(time, data);
                    }
                }
            }
            else if (commandType == UICommandType.ShowUI)
            {
                //_ltShowUIs = commandData.optionUI.GetComponentsInChildren<ILtShowUI>();
                if (_ltShowUIs != null)
                {
                    foreach (ILtShowUI showUI in _ltShowUIs)
                    {
                        showUI.displayState = DisplayState.ShowUI;
                        showUI.OnShowUIUpdata(time, data);
                    }
                }
            }
            else if (commandType == UICommandType.CloseUI)
            {
                //_ltCloseUIs = commandData.optionUI.GetComponentsInChildren<ILtCloseUI>();
                if (_ltCloseUIs != null)
                {
                    foreach (ILtCloseUI closeUI in _ltCloseUIs)
                    {
                        closeUI.displayState = DisplayState.CloseUI;
                        closeUI.OnCloseUIUpdata(time, data);
                    }
                }
            }
            else if (commandType == UICommandType.FreezeUI)
            {
                //_ltFreezeUIs = commandData.optionUI.GetComponentsInChildren<ILtFreezeUI>();
                if (_ltFreezeUIs != null)
                {
                    foreach (ILtFreezeUI freezeUI in _ltFreezeUIs)
                    {
                        freezeUI.displayState = DisplayState.FreezeUI;
                        freezeUI.OnFreezeUIUpdata(time, data);
                    }
                }
            }
            else if (commandType == UICommandType.ThawUI)
            {
                //_ltThawUIs = commandData.optionUI.GetComponentsInChildren<ILtThawUI>();
                if (_ltThawUIs != null)
                {
                    foreach (ILtThawUI thawUI in _ltThawUIs)
                    {
                        thawUI.displayState = DisplayState.ThawUI;
                        thawUI.OnThawUIUpdata(time, data);
                    }
                }
            }
        }

        private void ClildExecEnd()
        {
            if (commandType == UICommandType.OpenEndUI)
            {
                //_ltOpenUIs = commandData.optionUI.GetComponentsInChildren<ILtOpenUI>();
                if (_ltOpenUIs != null)
                {
                    foreach (ILtOpenUI openUI in _ltOpenUIs)
                    {
                        openUI.displayState = DisplayState.OpenUI;
                        openUI.OnOpenUIEnd(commandData.ParamVaules);
                    }
                }

                _ltOpenUIs = null;
            }
            else if (commandType == UICommandType.ShowUI)
            {
                //_ltShowUIs = commandData.optionUI.GetComponentsInChildren<ILtShowUI>();
                if (_ltShowUIs != null)
                {
                    foreach (ILtShowUI showUI in _ltShowUIs)
                    {
                        showUI.displayState = DisplayState.ShowUI;
                        showUI.OnShowUIEnd(commandData.ParamVaules);
                    }
                }

                _ltShowUIs = null;
            }
            else if (commandType == UICommandType.CloseEndUI)
            {
                //_ltCloseUIs = commandData.optionUI.GetComponentsInChildren<ILtCloseUI>();
                if (_ltCloseUIs != null)
                {
                    foreach (ILtCloseUI closeUI in _ltCloseUIs)
                    {
                        closeUI.displayState = DisplayState.CloseUI;
                        closeUI.OnCloseUIEnd(commandData.ParamVaules);
                    }
                }

                _ltCloseUIs = null;
            }
            else if (commandType == UICommandType.FreezeEndUI)
            {
                //_ltFreezeUIs = commandData.optionUI.GetComponentsInChildren<ILtFreezeUI>();
                if (_ltFreezeUIs != null)
                {
                    foreach (ILtFreezeUI freezeUI in _ltFreezeUIs)
                    {
                        freezeUI.displayState = DisplayState.FreezeUI;
                        freezeUI.OnFreezeUIEnd(commandData.ParamVaules);
                    }
                }

                _ltFreezeUIs = null;
            }
            else if (commandType == UICommandType.ThawEndUI)
            {
                //_ltThawUIs = commandData.optionUI.GetComponentsInChildren<ILtThawUI>();
                if (_ltThawUIs != null)
                {
                    foreach (ILtThawUI thawUI in _ltThawUIs)
                    {
                        thawUI.displayState = DisplayState.ThawUI;
                        thawUI.OnThawUIEnd(commandData.ParamVaules);
                    }
                }

                _ltThawUIs = null;
            }
        }

        #endregion

        public bool UpdateTimer()
        {
            if (commandData.OptionUI.timeScaleType == TimeScaleType.UnscaledDeltaTime)
            {
                return timer.AlarmUnSacleTime();
            }
            else if (commandData.OptionUI.timeScaleType == TimeScaleType.DeltaTime)
            {
                return timer.Alarm();
            }

            return false;
        }

        public UICommandType commandType
        {
            get { return commandData.CommandType; }
        }
    }
}