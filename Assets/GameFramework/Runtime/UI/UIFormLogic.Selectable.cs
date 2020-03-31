using LtFramework.UI;
using LtFramework.Util;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 界面逻辑基类。
    /// </summary>
    public abstract partial class UIFormLogic : MonoBehaviour
    {
        [NonSerialized]
        public CursorType CursorType = CursorType.Both;

        private SelectableCtrl _SelectableCtrl;
        /// <summary>
        /// 选中操作控制类
        /// </summary>
        internal SelectableCtrl SelectableCtrl
        {
            get
            {
                if (_SelectableCtrl == null)
                {
                    _SelectableCtrl = new SelectableCtrl(this);
                }

                return _SelectableCtrl;
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
                cacheFocus = UIMonoManager.Instance.GetFocus(this);
                if (cacheFocus == null)
                {
                    UIMonoManager.Instance.SaveFocus(this, button1p, button2p);
                }
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

        /// <summary>
        /// 开启选择控制
        /// </summary>
        /// <param UIName="isOpen">是否是打开</param>
        internal void EnableSelect(bool isOpen)
        {
            SelectableCtrl.EnableSelect(this, isOpen);
        }
    }
}