/*
 *    描述:
 *          1. UI命令数据
 *
 *    开发人: 邓平
 */
using UnityEngine;
using System;
using System.Collections.Generic;

namespace LtFramework.UI
{
    public enum UICommandType
    {
        Null,


        OpenUI,
        OpenEndUI,

        CloseUI,
        CloseEndUI,

        FreezeUI,
        FreezeEndUI,

        ThawUI,
        ThawEndUI,

        ShowUI,
        DestroyUI,
    }

    public class UICommandData
    {
        public int ID { get; private set; }
        public int Hash { get; private set; }

        public float Timer;

        public string uiName;

        //开始参数
        private readonly List<object> _ParamValues = new List<object>();
        internal UICommandType CommandType { get; private set; }
        public IBaseUIForm OptionUI { get; private set; }
        internal Action<float, object[]> ExecuteStart;
        internal Action<float, object[]> ExecuteUpdate;
        internal Action<object[]> ExecuteEnd;

        public object[] ParamVaules => _ParamValues.ToArray();

        internal void Init(UICommandType type, IBaseUIForm uiForm,
            float timer = 0, params object[] paramValues)
        {
            ID = uiForm.ID;
            Hash = GetHashCode();
            CommandType = type;
            Timer = timer;
            if (paramValues != null && paramValues.Length > 0)
            {
                foreach (object value in paramValues)
                {
                    _ParamValues.Add(value);
                }
            }

            OptionUI = uiForm;
            uiName = OptionUI.name;
            Init();
        }

        private void Init()
        {
            if (OptionUI == null)
            {
                Debug.LogError("操作的uI类为空");
                return;
            }


            switch (CommandType)
            {
                case UICommandType.OpenUI:
                    ExecuteStart = OptionUI.OnOpenUIStart;
                    ExecuteUpdate = OptionUI.OnOpenUIUpdate;
                    break;
                case UICommandType.OpenEndUI:
                    ExecuteEnd = OptionUI.OnOpenUIEnd;
                    break;
                case UICommandType.ShowUI:
                    ExecuteStart = OptionUI.OnShowUIStart;
                    ExecuteUpdate = OptionUI.OnShowUIUpdate;
                    ExecuteEnd = OptionUI.OnShowUIEnd;
                    break;
                case UICommandType.FreezeUI:
                    ExecuteStart = OptionUI.OnFreezeStart;
                    ExecuteUpdate = OptionUI.OnFreezeUpdate;
                    break;
                case UICommandType.FreezeEndUI:
                    ExecuteEnd = OptionUI.OnFreezeEnd;
                    break;
                case UICommandType.ThawUI:
                    ExecuteStart = OptionUI.OnThawStart;
                    ExecuteUpdate = OptionUI.OnThawUpdate;
                    break;
                case UICommandType.ThawEndUI:
                    ExecuteEnd = OptionUI.OnThawEnd;
                    break;
                case UICommandType.CloseUI:
                    ExecuteStart = OptionUI.OnCloseStart;
                    ExecuteUpdate = OptionUI.OnCloseUpdate;
                    break;
                case UICommandType.CloseEndUI:
                    ExecuteEnd = OptionUI.OnCloseEnd;
                    break;
                case UICommandType.DestroyUI:
                    break;
                default:
                    Debug.LogError("当前类型 " + CommandType);
                    break;

            }
        }

        public void Reset()
        {
            Timer = 0;
            ID = 0;
            _ParamValues.Clear();
            OptionUI = null;
            ExecuteStart = null;
            ExecuteUpdate = null;
            ExecuteEnd = null;
            CommandType = UICommandType.Null;
        }
    }
}