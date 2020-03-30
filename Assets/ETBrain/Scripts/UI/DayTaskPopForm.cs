
using UnityEngine.UI;

namespace ETBrain
{
    public class DayTaskPopForm: UGuiForm
    {
        private Button mCloseBtn;
        private Button mStartBtn;

        protected internal override void OnInit(object userData)
        {
            base.OnInit(userData);
            mCloseBtn = CachedTransform.Find("mask").GetComponent<Button>();
            mStartBtn = CachedTransform.Find("popout/btn_startTask").GetComponent<Button>();

            mCloseBtn.onClick.AddListener(OnCloseClick);
            mStartBtn.onClick.AddListener(OnStartClick);
        }

        protected internal override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        void OnCloseClick()
        {
            GameEntry.UI.CloseUIForm(this);
        }

        void OnStartClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.DayTaskForm);
            GameEntry.UI.CloseUIForm(this);
        }
    }
}
