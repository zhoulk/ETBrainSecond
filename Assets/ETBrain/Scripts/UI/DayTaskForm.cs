
using UnityEngine.UI;

namespace ETBrain
{
    public class DayTaskForm: UGuiForm
    {
        protected internal override void OnInit(object userData)
        {
            base.OnInit(userData);

            ButtonDefaultFocus("btn_start");

            ButtonOnClick("btn_start", OnStartClick);
            ButtonOnClick("closeBtn", OnCloseClick);
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

        }
    }
}
