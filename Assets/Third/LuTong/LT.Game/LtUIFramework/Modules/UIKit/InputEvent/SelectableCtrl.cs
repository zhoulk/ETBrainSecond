/*
 *    描述:
 *          1. UI操作响应类
 *
 *    开发人: 邓平
 */
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace LtFramework.UI
{
    public class SelectableCtrl
    {
        private readonly UIFormLogic _UIForm;
        internal bool IsSaveFocuseButton;
        private bool _IsCtrl = false;
        public List<ButtonEx> LtButtons = new List<ButtonEx>();

        //1p开始预设的选择
        public LtButton StartSelect1P;

        public LtButton SaveSelect1P;

        //1p上一个选择
        internal LtButton BeforeSelectable1P;

        //1p当前的选择
        internal LtButton CurrentSelectable1P;

        public LtButton StartSelect2P;
        public LtButton SaveSelect2P;
        private LtButton _BeforeSelectable2P;
        private LtButton _CurrentSelectable2P;

        internal void EnableSelect(UIFormLogic uiForm, bool isOpen)
        {
            for (int i = 0; i < LtButtons.Count; i++)
            {
                if (LtButtons[i])
                {
                    LtButtons[i].enabled = true;
                }
            }

            if (StartSelect1P == null)
            {
                if (isOpen)
                {
                    if (IsSaveFocuseButton && BeforeSelectable1P != null)
                    {
                        currentSelectable1P = BeforeSelectable1P;
                    }
                    else if (!IsSaveFocuseButton && SaveSelect1P != null)
                    {
                        currentSelectable1P = SaveSelect1P;
                    }
                }
                else
                {
                    if (BeforeSelectable1P != null)
                    {
                        currentSelectable1P = BeforeSelectable1P;
                    }
                }

            }

            if (StartSelect2P == null)
            {
                if (isOpen)
                {
                    if (IsSaveFocuseButton && _BeforeSelectable2P != null)
                    {
                        currentSelectable2P = _BeforeSelectable2P;
                    }
                    else if (!IsSaveFocuseButton && SaveSelect2P)
                    {
                        currentSelectable2P = SaveSelect2P;
                    }
                }
                else if (StartSelect2P == null)
                {
                    if (_BeforeSelectable2P != null)
                    {
                        currentSelectable2P = _BeforeSelectable2P;
                    }
                }
            }

            _IsCtrl = true;
        }

        public void DisableSelect()
        {
            _IsCtrl = false;
            if (currentSelectable1P != null)
            {
                BeforeSelectable1P = currentSelectable1P;
                currentSelectable1P = null;
            }

            if (currentSelectable2P != null)
            {
                beforeSelectable2P = currentSelectable2P;
                currentSelectable2P = null;
            }

            for (int i = 0; i < LtButtons.Count; i++)
            {
                if (LtButtons[i])
                {
                    LtButtons[i].enabled = false;
                }
            }
        }

        //public SelectableCtrl(IBaseUIForm uiForm)
        public SelectableCtrl(UIFormLogic uiForm)
        {
            this._UIForm = uiForm;
        }

        public LtButton currentSelectable1P
        {
            get
            {
                return CurrentSelectable1P;
            }
            set
            {
                BeforeSelectable1P = CurrentSelectable1P;
                CurrentSelectable1P = value;
                if (CurrentSelectable1P != null)
                {
                    UIMonoManager.Instance.EventSystem.SetSelectedGameObject(value.gameObject);
                }
                else
                {
                    //if (UIMonoManager.Instance.eventSystem.sendNavigationEvents)
                    {
                        UIMonoManager.Instance.EventSystem.SetSelectedGameObject(null);
                    }
                }
            }
        }

        public LtButton beforeSelectable1P
        {
            get { return BeforeSelectable1P; }
            set { BeforeSelectable1P = value; }
        }


        public LtButton currentSelectable2P
        {
            get
            {
                return _CurrentSelectable2P;
            }
            set
            {
                beforeSelectable2P = currentSelectable2P;
                _CurrentSelectable2P = value;
                if (value == null)
                {
                    return;
                }

                value.OnSelected(CtrlHandler.P2);
                _UIForm.OnSelected(value, CtrlHandler.P2);
            }
        }

        public LtButton beforeSelectable2P
        {
            get { return _BeforeSelectable2P; }
            set
            {
                if (value == null) return;
                _BeforeSelectable2P = value;
                value.OnDeselect(CtrlHandler.P2);
                _UIForm.OnDisSelected(value, CtrlHandler.P2);
            }
        }

        //Update更新
        public virtual void InputUpdate(UIFormLogic ui)
        {
            //Log.Info("{0} {1}", _IsCtrl, StartSelect1P);
            if (!_IsCtrl) return;

            if (StartSelect1P != null)
            {
                currentSelectable1P = StartSelect1P;
                SaveSelect1P = StartSelect1P;
                StartSelect1P = null;
            }

            if (StartSelect2P != null)
            {
                currentSelectable2P = StartSelect2P;
                SaveSelect2P = StartSelect2P;
                StartSelect2P = null;
            }

            Ctrl1P();
            OnBack();
            Ctrl2P();
        }

        #region 设置选择

        public void SetSelected1P(LtButton ltButton)
        {
            if (ltButton)
            {
                currentSelectable1P = ltButton;
            }
        }

        public void SetSelected2P(LtButton ltButton)
        {
            if (ltButton)
            {
                currentSelectable2P = ltButton;
            }
        }

        #endregion

        //1p控制
        void Ctrl1P()
        {

            if (currentSelectable1P == null) return;

            if (UIFrameInput.GetKey_Up_Down)
            {
                LtButton button = null;
                if (currentSelectable1P.FindSelectableOnUp())
                {
                    button = currentSelectable1P.FindSelectableOnUp().GetComponent<LtButton>();
                }

                button = _UIForm.OnUpSelected(currentSelectable1P, button, CtrlHandler.P1);
                if (button)
                {
                    SetSelected1P(button);
                }
            }
            else if (UIFrameInput.GetKey_Down_Down)
            {
                LtButton button = null;
                if (currentSelectable1P.FindSelectableOnDown())
                {
                    button = currentSelectable1P.FindSelectableOnDown().GetComponent<LtButton>();
                }

                button = _UIForm.OnDownSelected(currentSelectable1P, button, CtrlHandler.P1);
                if (button)
                {
                    SetSelected1P(button);
                }
            }
            else if (UIFrameInput.GetKey_Left_Down)
            {
                LtButton button = null;
                if (currentSelectable1P.FindSelectableOnLeft())
                {
                    button = currentSelectable1P.FindSelectableOnLeft().GetComponent<LtButton>();
                }

                button = _UIForm.OnLeftSelected(currentSelectable1P, button, CtrlHandler.P1);
                if (button)
                {
                    SetSelected1P(button);
                }
            }
            else if (UIFrameInput.GetKey_Right_Down)
            {
                LtButton button = null;
                if (currentSelectable1P.FindSelectableOnRight())
                {
                    button = currentSelectable1P.FindSelectableOnRight().GetComponent<LtButton>();
                }

                button = _UIForm.OnRightSelected(currentSelectable1P, button, CtrlHandler.P1);
                if (button)
                {
                    SetSelected1P(button);
                }
            }

            if (UIFrameInput.GetKey_A_Down)
            {
                if (currentSelectable1P == null) return;
                currentSelectable1P.OnClickBy1P();
            }
        }

        private void OnBack()
        {
            if (UIFrameInput.GetKey_B_Down)
            {
                _UIForm.OnBackClickDown(CtrlHandler.P1);
            }
            else if (UIFrameInput.GetKey_B_Down_2P)
            {
                if (_UIForm.CursorType == CursorType.Single)
                {
                    if (currentSelectable2P != null)
                        _UIForm.OnBackClickDown(CtrlHandler.P2);
                }
                else if(_UIForm.CursorType == CursorType.Both)
                {
                    _UIForm.OnBackClickDown(CtrlHandler.P1);
                }

            }

        }

        //2p控制
        void Ctrl2P()
        {
            if (_UIForm.CursorType == CursorType.Single)
            {
                if (currentSelectable2P == null) return;
                if (UIFrameInput.GetKey_Up_Down_2P)
                {

                    LtButton button = null;
                    if (currentSelectable2P.FindSelectableOnUp2P())
                    {
                        button = currentSelectable2P.FindSelectableOnUp2P().GetComponent<LtButton>();
                    }

                    button = _UIForm.OnUpSelected(currentSelectable2P, button, CtrlHandler.P2);
                    if (button)
                    {
                        SetSelected2P(button);
                    }

                }
                else if (UIFrameInput.GetKey_Down_Down_2P)
                {
                    LtButton button = null;
                    if (currentSelectable2P.FindSelectableOnDown2P())
                    {
                        button = currentSelectable2P.FindSelectableOnDown2P().GetComponent<LtButton>();
                    }

                    button = _UIForm.OnDownSelected(currentSelectable2P, button, CtrlHandler.P2);
                    if (button)
                    {
                        SetSelected2P(button);
                    }

                }
                else if (UIFrameInput.GetKey_Left_Down_2P)
                {
                    LtButton button = null;
                    if (currentSelectable2P.FindSelectableOnLeft2P())
                    {
                        button = currentSelectable2P.FindSelectableOnLeft2P().GetComponent<LtButton>();
                    }

                    button = _UIForm.OnLeftSelected(currentSelectable2P, button, CtrlHandler.P2);
                    if (button)
                    {
                        SetSelected2P(button);
                    }

                }
                else if (UIFrameInput.GetKey_Right_Down_2P)
                {
                    LtButton button = null;
                    if (currentSelectable2P.FindSelectableOnRight2P())
                    {
                        button = currentSelectable2P.FindSelectableOnRight2P().GetComponent<LtButton>();
                    }

                    button = _UIForm.OnRightSelected(currentSelectable2P, button, CtrlHandler.P2);
                    if (button)
                    {
                        SetSelected2P(button);
                    }
                }

                if (UIFrameInput.GetKey_A_Down_2P)
                {
                    if (currentSelectable2P == null) return;

                    currentSelectable2P.OnClickBy2P();
                }
            }
            else if(_UIForm.CursorType == CursorType.Both)
            {
                if (currentSelectable1P == null) return;

                if (UIFrameInput.GetKey_Up_Down_2P)
                {
                    LtButton button = null;
                    if (currentSelectable1P.FindSelectableOnUp())
                    {
                        button = currentSelectable1P.FindSelectableOnUp().GetComponent<LtButton>();
                    }

                    button = _UIForm.OnUpSelected(currentSelectable1P, button, CtrlHandler.P1);
                    if (button)
                    {
                        SetSelected1P(button);
                    }
                }
                else if (UIFrameInput.GetKey_Down_Down_2P)
                {
                    LtButton button = null;
                    if (currentSelectable1P.FindSelectableOnDown())
                    {
                        button = currentSelectable1P.FindSelectableOnDown().GetComponent<LtButton>();
                    }

                    button = _UIForm.OnDownSelected(currentSelectable1P, button, CtrlHandler.P1);
                    if (button)
                    {
                        SetSelected1P(button);
                    }
                }
                else if (UIFrameInput.GetKey_Left_Down_2P)
                {
                    LtButton button = null;
                    if (currentSelectable1P.FindSelectableOnLeft())
                    {
                        button = currentSelectable1P.FindSelectableOnLeft().GetComponent<LtButton>();
                    }

                    button = _UIForm.OnLeftSelected(currentSelectable1P, button, CtrlHandler.P1);
                    if (button)
                    {
                        SetSelected1P(button);
                    }
                }
                else if (UIFrameInput.GetKey_Right_Down_2P)
                {
                    LtButton button = null;
                    if (currentSelectable1P.FindSelectableOnRight())
                    {
                        button = currentSelectable1P.FindSelectableOnRight().GetComponent<LtButton>();
                    }

                    button = _UIForm.OnRightSelected(currentSelectable1P, button, CtrlHandler.P1);
                    if (button)
                    {
                        SetSelected1P(button);
                    }
                }

                if (UIFrameInput.GetKey_A_Down_2P)
                {
                    if (currentSelectable1P == null) return;
                    currentSelectable1P.OnClickBy1P();
                }
            }

        }
    }
}
