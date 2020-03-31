/*
 *    描述:
 *          1. UI窗体基类
 *
 *    开发人: 邓平
 */
using System;
using System.Collections.Generic;
using LtFramework.Util;
using LtFramework.Util.Math;
using UnityEngine;
using UnityEngine.Events;
using LtFramework.Util.Tools;

namespace LtFramework.UI
{
    public abstract class IBaseUIForm : MonoBehaviourEx
    {
        public virtual void InitUIMode()
        {
            CurrentUIType.UIFormsType = UIFormType.Normal;
            CurrentUIType.UIFormsShowMode = UIFormShowMode.Normal;
            CurrentUIType.UIFormLucencyType = UIFormLucenyType.Lucency;
        }

        #region 字段

        [NonSerialized] public CursorType CursorType = CursorType.Both;

        //当前UI的类型
        public UIType CurrentUIType = new UIType();

        public UIFormDepend UIFormDepend = new UIFormDepend();

        private UIState _UIState = UIState.Null;

        //private DisplayState _DeDisplayState;

        /// <summary>
        /// UI操作时间更新类型
        /// </summary>
        private TimeScaleType _TimeScaleType = TimeScaleType.DeltaTime;

        public int ID => GetInstanceID();

        #endregion

        #region UI窗体状态动画

        private readonly List<int> _AnimationIndexs = new List<int>();

        public void SetAnimationIndex(params int[] index)
        {
            _AnimationIndexs.Clear();
            _AnimationIndexs.AddRange(index);
            ILtDisplayStateUI[] states = GetComponentsInChildren<ILtDisplayStateUI>();
            foreach (ILtDisplayStateUI state in states)
            {
                state.AnimationIndexs = _AnimationIndexs;
            }
        }

        [Header("准备显示时间")] public float OpenUITime = 0;
        [Header("准备解冻时间")] public float ThawUITime = 0;
        [Header("准备关闭时间")] public float CloseUITime = 0;
        [Header("准备冻结时间")] public float FreezeUITime = 0;


        internal LtTimer timer;

        /// <summary>
        /// 设置当时命令持续执行时间
        /// </summary>
        /// <param name="time"></param>
        public void SetContinueTime(float time)
        {
            if(time <= 0)
            {
                time = 0;
            }

            if(timer != null)
            {
                timer.SetTimer(time);
            }
        }

        /// <summary>
        /// 打开UI窗口
        /// </summary>
        /// <param UIName="data">打开参数</param>
        /// <param UIName="time">打开时间</param>
        public virtual void OnOpenUIStart(float time, params object[] paramValues)
        {

        }

        /// <summary>
        /// 打开UI窗口
        /// </summary>
        /// <param UIName="data">打开参数</param>
        /// <param UIName="ramainTime">剩余时间</param>
        public virtual void OnOpenUIUpdate(float ramainTime, params object[] paramValues)
        {

        }

        /// <summary>
        /// 页面开始显示完成后 按钮操作 启用开启
        /// </summary>
        public virtual void OnOpenUIEnd(params object[] paramValues)
        {

        }

        /// <summary>
        /// 页面开始显示  用于播放页面显示动画
        /// </summary>
        /// <returns>显示时间</returns>
        public virtual void OnShowUIStart(float time, params object[] paramValues)
        {

        }

        public virtual void OnShowUIUpdate(float remainTime, params object[] paramValues)
        {

        }

        /// <summary>
        /// 页面开始显示完成成 按钮操作还未开启
        /// </summary>
        public virtual void OnShowUIEnd(params object[] paramValues)
        {

        }

        public virtual void OnFreezeStart(float time, params object[] paramValues)
        {

        }

        public virtual void OnFreezeUpdate(float remainTime, params object[] paramValues)
        {

        }

        public virtual void OnFreezeEnd(params object[] paramValues)
        {

        }

        public virtual void OnThawStart(float time, params object[] paramValues)
        {

        }

        public virtual void OnThawUpdate(float remainTime, params object[] paramValues)
        {

        }

        public virtual void OnThawEnd(params object[] paramValues)
        {

        }

        public virtual void OnCloseStart(float time, params object[] paramValues)
        {

        }

        public virtual void OnCloseUpdate(float remainTime, params object[] paramValues)
        {

        }

        public virtual void OnCloseEnd(params object[] paramValues)
        {

        }



        #endregion

        #region  窗体状态

        /// <summary>
        /// 当前UI的状态
        /// </summary>
        public UIState UIStage
        {
            get { return _UIState; }
            internal set
            {
                switch (value)
                {
                    case UIState.Display:
                        Display();
                        break;
                    case UIState.ReDisplay:
                        Redisplay();
                        break;
                    case UIState.Hide:
                        Hide();
                        break;
                    case UIState.Freeze:
                        FreezeState();
                        break;
                    case UIState.Thaw:
                        ThawState();
                        break;
                    case UIState.Normal:
                        break;
                    case UIState.ReadyThaw:
                        break;
                    default:
                        Debug.LogError("没有相应的处理函数 处理 当前改变的状态" + value);
                        break;
                }

                _UIState = value;
            }
        }

        /// <summary>
        /// 显示状态
        /// </summary>
        void Display()
        {
            this.gameObject.SetActive(true);
            switch (CurrentUIType.UIFormsType)
            {
                case UIFormType.Normal:
                    NormalDisplay();
                    break;
                case UIFormType.Fixed:
                    FixedDisplay();
                    break;
                case UIFormType.PopUp:
                    PopUpDisplay();
                    break;
                case UIFormType.Prompt:
                    PromptDisplay();
                    break;
                default:
                    Debug.LogError("没有该类型UI " + CurrentUIType.UIFormsType);
                    break;
            }
        }

        /// <summary>
        /// 重新显示状态
        /// </summary>
        void Redisplay()
        {
            this.gameObject.SetActive(true);
            switch (CurrentUIType.UIFormsType)
            {
                case UIFormType.Normal:
                    NormalReDisplay();
                    break;
                case UIFormType.Fixed:
                    FixedReDisplay();
                    break;
                case UIFormType.PopUp:
                    PopUpReDisplay();
                    break;
                case UIFormType.Prompt:
                    PromptReDisplay();
                    break;
                default:
                    Debug.LogError("没有该类型UI " + CurrentUIType.UIFormsType);
                    break;
            }

            //todo 重新显示
            //AddReadShowUICommand(null);
            //AddOpenUIEndCommand(null);
            //IUICommand readyShowUI = new ReadyShowUICommand(this);
            //IUICommand showUI = new ShowUICommand(this);
            //UIMonoManager.Instance.invoker.AddOpenUICommand(readyShowUI);
            //UIMonoManager.Instance.invoker.AddOpenUICommand(showUI);
            //Debug.Log("Redisplay" + UIName);
        }

        /// <summary>
        /// 隐藏状态  
        /// </summary>
        void Hide()
        {
            switch (CurrentUIType.UIFormsType)
            {
                case UIFormType.Normal:
                    NormalHide();
                    break;
                case UIFormType.Fixed:
                    FixedHide();
                    break;
                case UIFormType.PopUp:
                    PopUpHide();
                    break;
                case UIFormType.Prompt:
                    PromptHide();
                    break;
                default:
                    Debug.Log("没有该类型UI " + CurrentUIType.UIFormsType);
                    break;
            }

            //关闭
            this.gameObject.SetActive(false);
            ResetDisableSelectCount();

        }

        /// <summary>
        /// 解冻
        /// </summary>
        void ThawState()
        {

        }

        /// <summary>
        /// 冻结状态
        /// </summary>
        void FreezeState()
        {
            switch (CurrentUIType.UIFormsType)
            {
                case UIFormType.Normal:
                    NormalFreeze();
                    break;
                case UIFormType.Fixed:
                    FixedFreeze();
                    break;
                case UIFormType.PopUp:
                    PopUpFreeze();
                    break;
                case UIFormType.Prompt:
                    PromptFreeze();
                    break;
                default:
                    Debug.Log("没有该类型UI " + CurrentUIType.UIFormsType);
                    break;
            }
        }

        #region 不同类型的窗体显示 和 隐藏

        void NormalDisplay()
        {
        }

        void FixedDisplay()
        {
        }

        void PopUpDisplay()
        {
            //设置弹窗遮罩
            UIMaskMgr.GetInstance().SetMaskWindow(this.gameObject, CurrentUIType.UIFormLucencyType);
        }

        void PromptReDisplay()
        {
        }

        void NormalReDisplay()
        {
        }

        void FixedReDisplay()
        {
        }

        void PopUpReDisplay()
        {
            //设置弹窗遮罩
            UIMaskMgr.GetInstance().SetMaskWindow(this.gameObject, CurrentUIType.UIFormLucencyType);
        }

        void PromptDisplay()
        {
        }


        void NormalHide()
        {
        }

        void FixedHide()
        {
        }

        void PopUpHide()
        {
            //取消遮罩
            UIMaskMgr.GetInstance().CancelMaskWindow();
        }

        void PromptHide()
        {
        }


        void NormalFreeze()
        {
        }

        void FixedFreeze()
        {
        }

        void PopUpFreeze()
        {
        }

        void PromptFreeze()
        {
        }

        #endregion

        #endregion

        #region 设置参数

        /// <summary>
        /// 设置UI更新时间类型
        /// </summary>
        /// <param UIName="type"></param>
        public void SetTimeScaleType(TimeScaleType type)
        {
            _TimeScaleType = type;
        }


        #endregion

        #region GetUI

        public TBaseUI GetUI<TBaseUI>() where TBaseUI : IBaseUIForm
        {
            string uiNmae = typeof(TBaseUI).Name;
            return UIMonoManager.GetUI(uiNmae) as TBaseUI;
        }

        public IBaseUIForm GetUI(string uiName)
        {
            return UIMonoManager.GetUI(uiName);
        }

        public TimeScaleType timeScaleType
        {
            get { return _TimeScaleType; }
        }

        #endregion

        #region OpenUI

        /// <summary>
        /// 打开UI
        /// </summary>
        /// <param UIName="openUIName">UI 名字</param>
        /// <param UIName="openOP"> 要打开UI的操作 </param>
        /// <param UIName="closeOP"> 当前UI的操作 </param>
        public TBaseUI OpenUI<TBaseUI>(float time = 0, params object[] paramVaules) where TBaseUI : IBaseUIForm
        {
            string uiName = typeof(TBaseUI).Name;
            IBaseUIForm uiForm = UIMonoManager.GetCurrentOpenUI(uiName);
            if (uiForm == null)
            {
                uiForm = GetUI<TBaseUI>();
                return OpenUI(uiForm, time, paramVaules) as TBaseUI;
            }

            return uiForm as TBaseUI;
        }

        public IBaseUIForm OpenUI(float time = 0, params object[] paramVaules)
        {
            if (UIMonoManager.IsOpenUI(this.name) == false)
            {
                OpenUI(this, time, paramVaules);
            }
            else
            {
                Debug.LogWarning("当前窗口已经打开 openUI :" + this.name);
            }

            return this;
        }

        public IBaseUIForm OpenUI(IBaseUIForm uiForm, float time = 0, params object[] paramVaules)
        {
            if (uiForm == null)
            {
                Debug.LogError("要打开的UI窗体为Null");
                return null;
            }

            //设置默认时间
            if (LtFramework.Util.Math.MathUtil.FloatEqual(time, 0f))
            {
                time = uiForm.OpenUITime;
            }

            //添加依赖节点
            if (uiForm != this)
            {
                UIFormDepend.AddChildNode(uiForm);
                uiForm.UIFormDepend.SetParent(this);
            }

            //添加打开命令
            UIMonoManager.Instance.AddOpenCommand(uiForm, time, paramVaules);
            UIMonoManager.Instance.AddOpenUIEndCommand(uiForm, paramVaules);
            return uiForm;
        }

        public IBaseUIForm AddShowUI(float time = 0, params object[] paramValues)
        {
            Debug.Log("ShowUI+ " +this.name + ID) ;
            UIMonoManager.Instance.AddShowUICommand(this, time, paramValues);
            return this;
        }

        public TBaseUI CancelShowUI<TBaseUI>() where TBaseUI : IBaseUIForm
        {
            string uiName = typeof(TBaseUI).Name;
            IBaseUIForm uiForm = UIMonoManager.GetCurrentOpenUI(uiName);
            if (uiForm == null)
            {
                uiForm = GetUI<TBaseUI>();
            }
            UIMonoManager.Instance.CancelShowUI(uiForm);
            return uiForm as TBaseUI;
        }
        public IBaseUIForm CancelShowUI()
        {
            UIMonoManager.Instance.CancelShowUI(this);
            return this;
        }

        #endregion

        #region CloseUI

        /// <summary>
        /// 关闭指定UI
        /// </summary>
        /// <typeparam UIName="TBaseUI"></typeparam>
        /// <param UIName="closeOP"></param>
        public TBaseUI CloseUI<TBaseUI>(float time = 0, params object[] paramValues) where TBaseUI : IBaseUIForm
        {
            string uiName = typeof(TBaseUI).Name;
            IBaseUIForm uiForm = UIMonoManager.GetCurrentOpenUI(uiName);
            if (uiForm != null)
            {
                return CloseUI(uiForm, time, paramValues) as TBaseUI;
            }
            else
            {
                if (UIMonoManager.ExistNewInstanceUI(uiName))
                {
                    Debug.LogError("NewInstance 窗口类型 不能通过泛型方法关闭");
                    return null;
                }

                Debug.LogError("当前窗口没有打开 closeUI :" + uiName);
            }

            return null;
        }

        public TBaseUI MyCloseUI<TBaseUI>(CloseUIType closeUIType = CloseUIType.Destroy, float time = 0, params object[] paramValues) where TBaseUI : IBaseUIForm
        {
            IBaseUIForm uiForm = CloseUI<TBaseUI>(time, paramValues);

            if (closeUIType == CloseUIType.Destroy)
            {
                UIMonoManager.Instance.AddDestroyCommand(uiForm);
            }

            return null;
        }

        public IBaseUIForm MyCloseUI(CloseUIType closeUIType = CloseUIType.Destroy, float time = 0, params object[] paramValues)
        {
            CloseUI(this, time, paramValues);

            if (closeUIType == CloseUIType.Destroy)
            {
                DestoryUI();
                return null;
            }

            return this;
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param UIName="closeUIName"> 不填写名字关闭当前窗体 填写名字关闭指定窗体 </param>
        public IBaseUIForm CloseUI(float time = 0, params object[] paramValues)
        {
            CloseUI(this, time, paramValues);

            return this;
        }

        private IBaseUIForm CloseUI(IBaseUIForm uiForm, float time, params object[] paramValues)
        {
            if (uiForm == null)
            {
                Debug.LogError("要关闭的窗体为Null");
                return null;
            }

            if (LtFramework.Util.Math.MathUtil.FloatEqual(time, 0f))
            {
                time = uiForm.CloseUITime;
            }

            UIMonoManager.Instance.AddCloseCommand(uiForm, time, paramValues);
            UIMonoManager.Instance.AddCloseEndCommand(uiForm, paramValues);
            return uiForm;
        }

        public void DestoryUI()
        {
            UIMonoManager.Instance.AddDestroyCommand(this);
        }

        /// <summary>
        /// 关闭所有子节点
        /// </summary>
        /// <param UIName="circle">是否递归关节点</param>
        /// <param UIName="time">关闭时间</param>
        /// <param UIName="data">参数</param>
        public void CloseClildren(bool circle = true, float time = 0, params object[] paramValues)
        {
            UIFormDepend.CloseAllClidren(circle, time, paramValues);
        }

        #endregion

        #region Freeze

        public TBaseUI FreezeUI<TBaseUI>(float time = 0, params object[] paramValues) where TBaseUI : IBaseUIForm
        {
            string uiName = typeof(TBaseUI).Name;
            IBaseUIForm uiForm = GetUI<TBaseUI>();
            if (uiForm != null)
            {
                return FreezeUI(uiForm, time, paramValues) as TBaseUI;
            }

            return null;
        }

        public IBaseUIForm FreezeUI(float time = 0, params object[] paramValues)
        {
            return FreezeUI(this, time, paramValues);
        }

        public IBaseUIForm FreezeUI(IBaseUIForm uiForm, float time, params object[] paramValues)
        {
            if (uiForm == null)
            {
                Debug.LogError("没有对应的窗体 ");
                return null;
            }

            if (time == 0)
            {
                time = uiForm.FreezeUITime;
            }

            UIMonoManager.Instance.AddFreezeCommand(uiForm, time, paramValues);
            UIMonoManager.Instance.AddFreezeEndCommand(uiForm, paramValues);
            return uiForm;
        }

        /// <summary>
        /// 冻结所有子节点
        /// </summary>
        /// <param UIName="circle">是否递归关节点</param>
        /// <param UIName="time">关闭时间</param>
        /// <param UIName="data">参数</param>
        public IBaseUIForm FreezeClildrenUI(bool circle = true, float time = 0, params object[] paramValues)
        {
            UIFormDepend.FreezeAllClildren(circle, time, paramValues);
            return this;
        }

        public TBaseUI ThawUI<TBaseUI>(float time = 0, params object[] paramValues) where TBaseUI : IBaseUIForm
        {
            string uiName = typeof(TBaseUI).Name;
            IBaseUIForm uiForm = GetUI<TBaseUI>();

            if (uiForm != null)
            {
                return ThawUI(uiForm, time, paramValues) as TBaseUI;
            }

            return null;
        }

        public IBaseUIForm ThawUI(float time = 0, params object[] paramValues)
        {
            return ThawUI(this, time, paramValues);
        }

        public IBaseUIForm ThawUI(IBaseUIForm uiForm, float time, params object[] paramValues)
        {
            if (uiForm == null)
            {
                Debug.Log("没有对应的窗体");
                return null;
            }

            if (time == 0)
            {
                time = uiForm.ThawUITime;
            }
            UIStage = UIState.ReadyThaw;
            UIMonoManager.Instance.AddThawCommand(uiForm, time, paramValues);
            UIMonoManager.Instance.AddThawEndCommand(uiForm, paramValues);
            return uiForm;
        }

        /// <summary>
        /// 解冻所有子节点
        /// </summary>
        /// <param UIName="circle">是否递归关节点</param>
        /// <param UIName="time">关闭时间</param>
        /// <param UIName="data">参数</param>
        public IBaseUIForm ThawClildrenUI(bool circle = true, float time = 0, params object[] paramValues)
        {
            UIFormDepend.ThawAllClildren(circle, time, paramValues);
            return this;
        }

        #endregion

        #region UICtrl

        private List<LtButton> _LtButtons;
        private SelectableCtrl _SelectableCtrl;
        private int _DisableSelectCount = 1;
        private bool _ChangeFreeze = false;
        private bool _ChangeThaw = false;

        internal int DisableSelectCount
        {
            get { return _DisableSelectCount; }
            set
            {
                int oldValue = _DisableSelectCount;
                _DisableSelectCount = value;

                if(_DisableSelectCount < 0)
                {
                    Debug.LogErrorFormat("当前页面{0}在Normal状态下被解冻", gameObject.name);
                }

                _ChangeFreeze = false;
                _ChangeThaw = false;

                if (_DisableSelectCount == 0 && oldValue == 1)
                {
                    _ChangeThaw = true;
                }
                else if (_DisableSelectCount == 1 && oldValue == 0)
                {
                    _ChangeFreeze = true;
                }
            }
        }

        private void ResetDisableSelectCount()
        {
            _DisableSelectCount = 1;
            _ChangeFreeze = false;
            _ChangeThaw = false;
        }

        /// <summary>
        /// 选中操作控制类
        /// </summary>
        internal SelectableCtrl SelectableCtrl
        {
            get
            {
                if (_SelectableCtrl == null)
                {
                    //_SelectableCtrl = new SelectableCtrl(this);
                }
                
                return _SelectableCtrl;
            }
        }

        /// <summary>
        /// 操作的更新
        /// </summary>
        internal void InputUpdate()
        {
            if (IsCanCtrl)
            {
                //SelectableCtrl.InputUpdate(this);
            }
        }

        /// <summary>
        /// 是否可操作当前页面
        /// </summary>
        public bool IsCanCtrl
        {
            get { return DisableSelectCount == 0; }
        }


        /// <summary>
        /// 开启选择控制
        /// </summary>
        /// <param UIName="isOpen">是否是打开</param>
        internal void EnableSelect(bool isOpen)
        {
            //if (_ChangeThaw)
            //{
            //    if (CurrentUIType.UIFormsShowMode == UIFormShowMode.Normal)
            //    {
            //        UIMonoManager.Instance.SendNavigationEvents = true;
            //    }
            //    UIMonoManager.Instance.AddCtrlUIForm(this);
            //    SelectableCtrl.EnableSelect(this, isOpen);
            //    UIStage = UIState.Normal;
            //}
            //else if (DisableSelectCount < 0)
            //{
            //    Debug.LogErrorFormat("当前界面 {0} 可能 被多次解冻", name);
            //}
        }

        /// <summary>
        /// 关闭选择控制
        /// </summary>
        internal void DisableSelect()
        {
            if (_ChangeFreeze)
            {
                if (CurrentUIType.UIFormsShowMode == UIFormShowMode.Normal)
                {
                    UIMonoManager.Instance.SendNavigationEvents = false;
                }
                UIMonoManager.Instance.RemoveCtrlUIForm(this);
                SelectableCtrl.DisableSelect();
            }
        }


        /// <summary>
        /// 设置当前页面的默认焦点
        /// </summary>
        /// <param UIName="buttonName1P"> 1P的默认选择的名字 </param>
        /// <param UIName="isSaveFocusePos"> 是否保存当前页面按钮光标位置 </param>
        public void ButtonDefaultFocus(string buttonName1P, bool isSaveFocusePos = true)
        {
            ButtonDefaultFocus(buttonName1P, null, isSaveFocusePos);
        }

        /// <summary>
        /// 设置当前页面的默认焦点
        /// </summary>
        /// <param UIName="buttonName1P"> 1P的默认选择的名字 </param>
        /// <param UIName="buttonName2P"> 2P的默认选择的名字 </param>
        /// <param UIName="isSaveFocusePos"> 是否保存当前页面按钮光标位置 </param>
        public void ButtonDefaultFocus(string buttonName1P, string buttonName2P, bool isSaveFocusePos = true)
        {
            LtButton button = gameObject.GetChildComponet<LtButton>(buttonName1P);
            LtButton button_2p = null;
            if (buttonName2P != null)
            {
                button_2p = gameObject.GetChildComponet<LtButton>(buttonName2P);
            }

            ButtonDefaultFocus(button, button_2p, isSaveFocusePos);
        }


        /// <summary>
        /// 设置当前页面的默认焦点
        /// </summary>
        /// <param UIName="buttonName1P"> 1P的默认选择 </param>
        /// <param UIName="isSaveFocusePos"> 是否保存当前页面按钮光标位置 </param>
        public void ButtonDefaultFocus(LtButton button1p, bool isSaveFocusePos = true)
        {
            ButtonDefaultFocus(button1p, null, isSaveFocusePos);
        }

        /// <summary>
        /// 设置当前页面的默认焦点
        /// </summary>
        /// <param UIName="buttonName1P"> 1P的默认选择 </param>
        /// <param UIName="buttonName2P"> 2P的默认选择 </param>
        /// <param UIName="isSaveFocusePos"> 是否保存当前页面按钮光标位置 </param>
        public void ButtonDefaultFocus(LtButton button1p, LtButton button2p, bool isSaveFocusePos = true)
        {
            string[] cacheFocus = null;
            if (isSaveFocusePos)
            {
                //cacheFocus = UIMonoManager.Instance.GetFocus(this);
                //if (cacheFocus == null)
                //{
                //    UIMonoManager.Instance.SaveFocus(this, button1p, button2p);
                //}
            }

            SelectableCtrl.StartSelect1P = button1p;
            SelectableCtrl.StartSelect2P = button2p;
            if (button2p != null)
            {
                CursorType = CursorType.Single;
            }

            SelectableCtrl.IsSaveFocuseButton = isSaveFocusePos;
        }


        /// <summary>
        /// 得到1P当前选择的物体
        /// </summary>
        /// <returns></returns>
        public LtButton currentSelected1P
        {
            get { return SelectableCtrl.currentSelectable1P; }
            set { SelectableCtrl.currentSelectable1P = value; }
        }

        /// <summary>
        /// 得到2P当前选择的物体
        /// </summary>
        public LtButton currentSelected2P
        {
            get { return SelectableCtrl.currentSelectable2P; }
            set
            {
                SelectableCtrl.currentSelectable2P = value;
                if (value != null)
                {
                    CursorType = CursorType.Single;
                }
            }
        }

        /// <summary>
        /// 当前页面的LtButton注册
        /// </summary>
        /// <param UIName="buttonName">Button的名字</param>
        /// <param UIName="call">1P事件</param>
        /// <param UIName="call2">2P事件</param>
        protected void ButtonOnClick(string buttonName, UnityAction call, UnityAction call2 = null)
        {
            LtButton button = gameObject.GetChildComponet<LtButton>(buttonName);
            button.ButtonOnClick(call, call2);
        }

        protected void ButtonOnClick(GameObject buttonObj, UnityAction call, UnityAction call2 = null)
        {
            LtButton button = buttonObj.GetComponent<LtButton>();
            button.ButtonOnClick(call, call2);
        }


        /// <summary>
        /// LtButton注册
        /// </summary>
        /// <param UIName="buttonName">Button的名字</param>
        /// <param UIName="call">1P事件</param>
        /// <param UIName="call2">2P事件</param>
        protected void ButtonOnClick(LtButton button, UnityAction call, UnityAction call2 = null)
        {
            button.OnClick(call, call2);
        }

        #endregion

        #region 设置导航

        /// <summary>
        /// 按下返回键的操作
        /// </summary>
        public virtual void OnBackClickDown(CtrlHandler ctrl)
        {

        }

        /// <summary>
        /// 当按钮被选中的回调
        /// </summary>
        /// <param UIName="button"> 选中的按钮 </param>
        /// <param UIName="ctrl"> 操作者 </param>
        public virtual void OnSelected(LtButton button, CtrlHandler ctrl)
        {

        }

        /// <summary>
        /// 当按钮取消选中的回调
        /// </summary>
        /// <param UIName="button"> 取消选中的按钮 </param>
        /// <param UIName="ctrl"> 操作者 </param>
        public virtual void OnDisSelected(LtButton button, CtrlHandler ctrl)
        {

        }


        /// <summary>
        /// 按键自动导航的回调
        /// 执行在导航之前
        /// </summary>
        /// <param UIName="currentButton">当前选择的按钮</param>
        /// <param UIName="autoSelecteButton">自动导航将要设置的按钮</param>
        /// <param UIName="is2P">false 为1P操作，true 为2P操作</param>
        /// <returns> 你需要修改的导航按钮 </returns>
        public virtual LtButton OnLeftSelected(LtButton currentButton, LtButton autoSelecteButton, CtrlHandler ctrl)
        {
            return autoSelecteButton;
        }

        /// <summary>
        /// 按键自动导航的回调
        /// 执行在导航之前
        /// </summary>
        /// <param UIName="currentButton">当前选择的按钮</param>
        /// <param UIName="autoSelecteButton">自动导航将要设置的按钮</param>
        /// <param UIName="is2P">false 为1P操作，true 为2P操作</param>
        /// <returns> 你需要修改的导航按钮 </returns>
        public virtual LtButton OnRightSelected(LtButton currentButton, LtButton autoSelecteButton, CtrlHandler ctrl)
        {
            return autoSelecteButton;
        }

        /// <summary>
        /// 按键自动导航的回调
        /// 执行在导航之前
        /// </summary>
        /// <param UIName="currentButton">当前选择的按钮</param>
        /// <param UIName="autoSelecteButton">自动导航将要设置的按钮</param>
        /// <param UIName="is2P">false 为1P操作，true 为2P操作</param>
        /// <returns> 你需要修改的导航按钮 </returns>
        public virtual LtButton OnUpSelected(LtButton currentButton, LtButton autoSelecteButton, CtrlHandler ctrl)
        {
            return autoSelecteButton;
        }

        /// <summary>
        /// 按键自动导航的回调
        /// 执行在导航之前
        /// </summary>
        /// <param UIName="currentButton">当前选择的按钮</param>
        /// <param UIName="autoSelecteButton">自动导航将要设置的按钮</param>
        /// <param UIName="is2P">false 为1P操作，true 为2P操作</param>
        /// <returns> 你需要修改的导航按钮 </returns>
        public virtual LtButton OnDownSelected(LtButton currentButton, LtButton autoSelecteButton, CtrlHandler ctrl)
        {
            return autoSelecteButton;
        }

        #endregion
    }
}

public enum CloseUIType
{
    Close,
    Destroy
}
