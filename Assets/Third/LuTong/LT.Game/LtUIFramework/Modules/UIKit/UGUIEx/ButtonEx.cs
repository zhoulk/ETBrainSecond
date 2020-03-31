/*
 *    描述:
 *          1. UGUI扩展类
 *
 *    开发人: 邓平
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace LtFramework.UI
{
    public enum CtrlHandler
    {
        P1,
        P2
    }

    public class ButtonEx : Button
    {
        public enum SelecteType
        {
            All,
            Ctrl1P,
            Ctrl2P
        }

        #region static

        /// <summary>
        /// 当前所打开的UI页面所有激活的按钮
        /// </summary>
        private static List<ButtonEx> allLtButton = new List<ButtonEx>();

        /// <summary>
        /// 1P可选择的按钮
        /// </summary>
        private static List<ButtonEx> s_List1P = new List<ButtonEx>();
        
        //当前1P选择的层
        protected static ButtonEx currentButton1P;
        /// <summary>
        /// 2P可选择的按钮
        /// </summary>
        private static List<ButtonEx> s_List2P = new List<ButtonEx>();
        //当前2P选择的层
        protected static ButtonEx currentButton2P;

        #endregion

        /// <summary>
        /// 按钮选择类型
        /// </summary>
        public SelecteType CtrlType = SelecteType.All;

        /// <summary>
        /// 按钮层级 不同层级按钮不能自动导航 只能手动设置
        /// </summary>
        public int SelecteLayer = 0;
        /// <summary>
        /// 该按钮是否可以同时被选中
        /// </summary>
        public bool CanBothSelecte = true;

        private UnityAction _OnClickBy1P;
        private UnityAction _OnClickBy2P;

        public void OnClickBy1P()
        {
            if (_OnClickBy1P != null)
            {
                _OnClickBy1P();
                //AudioMgr.Instance.PlayEffect(SoundType.UI.UI_confirm);
            }

            if (SelectedShow1P)
            {
                ILtButtonCtrl ltButtonCtrl = SelectedShow1P.GetComponent<ILtButtonCtrl>();
                if (ltButtonCtrl != null)
                {
                    ltButtonCtrl.OnClick();
                }
            }
        }

        public void OnClickBy2P()
        {
            if (_OnClickBy2P != null)
            {
                _OnClickBy2P();
            }
            if (SelectedShow2P)
            {
                ILtButtonCtrl ltButtonCtrl = SelectedShow2P.GetComponent<ILtButtonCtrl>();
                if (ltButtonCtrl != null)
                {
                    ltButtonCtrl.OnClick();
                }
            }
        }


        public GameObject SelectedShow1P;
        public GameObject SelectedShow2P;

        protected override void Awake()
        {
            base.Awake();
            if (SelectedShow1P == null)
            {

                var p1 = transform.Find("Selected1P");
                if (p1)
                {
                    SelectedShow1P = p1.gameObject;
                }

            }

            if (SelectedShow2P == null)
            {

                var p2 = transform.Find("Selected2P");
                if (p2)
                {
                    SelectedShow2P = p2.gameObject;
                }

            }
        }

        public void AddListener1P(UnityAction click)
        {
            _OnClickBy1P = click;
            onClick.AddListener(click);
        }

        public void AddListener2P(UnityAction click)
        {
            _OnClickBy2P = click;
        }

        protected override void Start()
        {
            base.Start();
            if (baseUiForm != null)
            {
                baseUiForm.SelectableCtrl.LtButtons.Add(this);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SetIsSelected(true);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            SetIsSelected(false);
        }

        /// <summary>
        /// 设置当前按钮是否可以被选中
        /// </summary>
        /// <param UIName="select"></param>
        public void SetIsSelected(bool select)
        {
            if (select)
            {
                if (!allLtButton.Contains(this))
                    allLtButton.Add(this);
            }
            else
            {
                if (allLtButton.Contains(this))
                    allLtButton.Remove(this);
            }
        }

        /// <summary>
        /// 当被选中
        /// </summary>
        /// <param UIName="ctrl"></param>
        public void OnSelected(CtrlHandler ctrl)
        {
            switch (ctrl)
            {
                case CtrlHandler.P1:
                    currentButton1P = this;
                    if (SelectedShow1P)
                    {
                        ILtButtonCtrl ltButtonCtrl = SelectedShow1P.GetComponent<ILtButtonCtrl>();
                        if (ltButtonCtrl != null)
                        {
                            ltButtonCtrl.OnSelected();
                        }
                    }
                    UpdateAdd1PList();
                        break;
                case CtrlHandler.P2:
                    currentButton2P = this;
                    if (SelectedShow2P != null)
                    {
                        ILtButtonCtrl ltButtonCtrl = SelectedShow2P.GetComponent<ILtButtonCtrl>();
                        if (ltButtonCtrl != null)
                        {
                            ltButtonCtrl.OnSelected();
                        }
                    }
                    UpdateAdd2PList();
                    break;
            }
        }
        /// <summary>
        /// 当移除选中
        /// </summary>
        /// <param UIName="ctrl"></param>
        public void OnDeselect(CtrlHandler ctrl)
        {
            switch (ctrl)
            {
                case CtrlHandler.P1:
                    if (SelectedShow1P)
                    {
                        ILtButtonCtrl ltButtonCtrl = SelectedShow1P.GetComponent<ILtButtonCtrl>();
                        if (ltButtonCtrl != null)
                        {
                            ltButtonCtrl.OnDiselected();
                        }
                    }
                    UpdateREmove2PList();
                    break;
                case CtrlHandler.P2:
                    if (SelectedShow2P != null)
                    {
                        ILtButtonCtrl ltButtonCtrl = SelectedShow2P.GetComponent<ILtButtonCtrl>();
                        if (ltButtonCtrl != null)
                        {
                            ltButtonCtrl.OnDiselected();
                        }
                    }
                    UpdateRemove1PList();
                    break;
            }
        }


        private UIFormLogic _baseUiForm;

        private UIFormLogic baseUiForm
        {
            get {
                if (_baseUiForm == null)
                {
                    _baseUiForm = GetComponentInParent<UIFormLogic>();
                }

                return _baseUiForm;
            }
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            baseUiForm.SelectableCtrl.CurrentSelectable1P = this as LtButton;
            OnSelected(CtrlHandler.P1);
            baseUiForm.OnSelected(this as LtButton, CtrlHandler.P1);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            baseUiForm.SelectableCtrl.BeforeSelectable1P = this as LtButton;
            OnDeselect(CtrlHandler.P1);
            baseUiForm.OnDisSelected(this as LtButton, CtrlHandler.P1);
        }

        #region 可选择按钮更新

        void UpdateAdd1PList()
        {
            if (allLtButton.Count > 0 && !s_List1P.Contains(this))
            {
                s_List1P.Clear();
                foreach (ButtonEx buttonEx in allLtButton)
                {
                    if (buttonEx.SelecteLayer == this.SelecteLayer && buttonEx.CtrlType != SelecteType.Ctrl2P)
                    {
                        s_List1P.Add(buttonEx);
                    }
                }
            }
        }

        void UpdateAdd2PList()
        {
            if (allLtButton.Count > 0 && !s_List2P.Contains(this))
            {
                s_List2P.Clear();
                foreach (ButtonEx buttonEx in allLtButton)
                {
                    if (buttonEx.SelecteLayer == this.SelecteLayer && buttonEx.CtrlType != SelecteType.Ctrl1P)
                    {
                        s_List2P.Add(buttonEx);
                    }
                }
            }
        }

        void UpdateRemove1PList()
        {
            if (this.CtrlType == SelecteType.All && CanBothSelecte == false)
            {
                if (s_List1P.Count > 0)
                {
                    ButtonEx button = s_List1P[0];
                    if (button.SelecteLayer == this.SelecteLayer && !s_List1P.Contains(this))
                    {
                        s_List1P.Add(this);
                    }
                }
            }
        }

        void UpdateREmove2PList()
        {
            if (this.CtrlType == SelecteType.All && CanBothSelecte == false)
            {
                if (s_List2P.Count > 0)
                {
                    ButtonEx button = s_List2P[0];
                    if (button.SelecteLayer == this.SelecteLayer && !s_List2P.Contains(this))
                    {
                        s_List2P.Add(this);
                    }
                }
            }
        }
        #endregion

        #region 导航

        public Selectable FindSelectable(Vector3 dir,List<ButtonEx> list)
        {
            dir = dir.normalized;
            Vector3 localDir = Quaternion.Inverse(transform.rotation) * dir;
            Vector3 pos = transform.TransformPoint(GetPointOnRectEdge(transform as RectTransform, localDir));
            float maxScore = Mathf.NegativeInfinity;
            Selectable bestPick = null;
            for (int i = 0; i < list.Count; ++i)
            {
                Selectable sel = list[i];

                if (sel == this || sel == null)
                    continue;

                if (!sel.IsInteractable() || sel.navigation.mode == Navigation.Mode.None)
                    continue;

                var selRect = sel.transform as RectTransform;
                Vector3 selCenter = selRect != null ? (Vector3)selRect.rect.center : Vector3.zero;
                Vector3 myVector = sel.transform.TransformPoint(selCenter) - pos;

                // Value that is the distance out along the direction.
                float dot = Vector3.Dot(dir, myVector);

                // Skip elements that are in the wrong direction or which have zero distance.
                // This also ensures that the scoring formula below will not have a division by zero error.
                if (dot <= 0)
                    continue;

                float score = dot / myVector.sqrMagnitude;

                if (score > maxScore)
                {
                    maxScore = score;
                    bestPick = sel;
                }
            }
            return bestPick;
        }

        private static Vector3 GetPointOnRectEdge(RectTransform rect, Vector2 dir)
        {
            if (rect == null)
                return Vector3.zero;
            if (dir != Vector2.zero)
                dir /= Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
            dir = rect.rect.center + Vector2.Scale(rect.rect.size, dir * 0.5f);
            return dir;
        }



        public override Selectable FindSelectableOnLeft()
        {
            if (navigation.mode == Navigation.Mode.Explicit)
            {
                return navigation.selectOnLeft;
            }
            if ((navigation.mode & Navigation.Mode.Horizontal) != 0)
            {
                return FindSelectable(transform.rotation * Vector3.left,s_List1P);
            }

            return null;
        }

        // Find the selectable object to the right of this one.
        public override Selectable FindSelectableOnRight()
        {
            if (navigation.mode == Navigation.Mode.Explicit)
            {
                return navigation.selectOnRight;
            }
            if ((navigation.mode & Navigation.Mode.Horizontal) != 0)
            {
                return FindSelectable(transform.rotation * Vector3.right, s_List1P);
            }
            return null;
        }

        // Find the selectable object above this one
        public override Selectable FindSelectableOnUp()
        {
            if (navigation.mode == Navigation.Mode.Explicit)
            {
                return navigation.selectOnUp;
            }
            if ((navigation.mode & Navigation.Mode.Vertical) != 0)
            {
                return FindSelectable(transform.rotation * Vector3.up, s_List1P);
            }
            return null;
        }

        // Find the selectable object below this one.
        public override Selectable FindSelectableOnDown()
        {
            if (navigation.mode == Navigation.Mode.Explicit)
            {
                return navigation.selectOnDown;
            }
            if ((navigation.mode & Navigation.Mode.Vertical) != 0)
            {
                return FindSelectable(transform.rotation * Vector3.down, s_List1P);
            }
            return null;
        }

        public virtual Selectable FindSelectableOnLeft2P()
        {
            if (navigation.mode == Navigation.Mode.Explicit)
            {
                return navigation.selectOnLeft;
            }
            if ((navigation.mode & Navigation.Mode.Horizontal) != 0)
            {
                return FindSelectable(transform.rotation * Vector3.left, s_List2P);
            }
            return null;
        }

        // Find the selectable object to the right of this one.
        public virtual Selectable FindSelectableOnRight2P()
        {
            if (navigation.mode == Navigation.Mode.Explicit)
            {
                return navigation.selectOnRight;
            }
            if ((navigation.mode & Navigation.Mode.Horizontal) != 0)
            {
                return FindSelectable(transform.rotation * Vector3.right, s_List2P);
            }
            return null;
        }

        // Find the selectable object above this one
        public virtual Selectable FindSelectableOnUp2P()
        {
            if (navigation.mode == Navigation.Mode.Explicit)
            {
                return navigation.selectOnUp;
            }
            if ((navigation.mode & Navigation.Mode.Vertical) != 0)
            {
                return FindSelectable(transform.rotation * Vector3.up, s_List2P);
            }
            return null;
        }

        // Find the selectable object below this one.
        public virtual Selectable FindSelectableOnDown2P()
        {
            if (navigation.mode == Navigation.Mode.Explicit)
            {
                return navigation.selectOnDown;
            }
            if ((navigation.mode & Navigation.Mode.Vertical) != 0)
            {
                return FindSelectable(transform.rotation * Vector3.down, s_List2P);
            }
            return null;
        }

        #endregion


    }

}