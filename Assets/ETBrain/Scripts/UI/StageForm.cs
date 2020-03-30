using UnityEngine;
using UnityEngine.UI;

namespace ETBrain
{
    public class StageForm: UGuiForm
    {
        private Button btn_base;
        private Button btn_enhance;
        private Button btn_advance;

        protected internal override void OnInit(object userData)
        {
            base.OnInit(userData);
            btn_base = CachedTransform.Find("root/btn_base").GetComponent<Button>();
            btn_enhance = CachedTransform.Find("root/btn_enhance").GetComponent<Button>();
            btn_advance = CachedTransform.Find("root/btn_advance").GetComponent<Button>();

            btn_base.onClick.AddListener(OnBaseBtnClick);
            btn_enhance.onClick.AddListener(OnEnhanceBtnClick);
            btn_advance.onClick.AddListener(OnAdvanceBtnClick);
        }

        protected internal override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            if (!(bool)GameEntry.DataNode.GetData(Constant.DataNode.IsWXBind).GetValue())
            {
                GameEntry.UI.OpenUIForm(UIFormId.LoginForm);
            }
        }

        void OnBaseBtnClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.MapForm, MapForm.MapFormParams.Create(this, Game.Stage.Base));
            GameEntry.UI.CloseUIForm(this);
        }

        void OnEnhanceBtnClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.MapForm, MapForm.MapFormParams.Create(this, Game.Stage.Enhance));
            GameEntry.UI.CloseUIForm(this);
        }

        void OnAdvanceBtnClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.MapForm, MapForm.MapFormParams.Create(this, Game.Stage.Advance));
            GameEntry.UI.CloseUIForm(this);
        }
    }
}
