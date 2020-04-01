
using UnityEngine.UI;

namespace ETBrain
{
    public class DayTaskPopForm: UGuiForm
    {
        private Button mCloseBtn;

        protected internal override void OnInit(object userData)
        {
            base.OnInit(userData);
            mCloseBtn = CachedTransform.Find("mask").GetComponent<Button>();

            //mCloseBtn.onClick.AddListener(OnCloseClick);
            //mStartBtn.onClick.AddListener(OnStartClick);

            ButtonDefaultFocus("btn_startTask");

            ButtonOnClick("btn_startTask", OnStartClick);
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
