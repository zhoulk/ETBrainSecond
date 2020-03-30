
using UnityEngine.UI;

namespace ETBrain
{
    public class HomeForm: UGuiForm
    {
        private Button mCloseBtn;

        protected internal override void OnInit(object userData)
        {
            base.OnInit(userData);
            mCloseBtn = CachedTransform.Find("home_page/closeBtn").GetComponent<Button>();

            mCloseBtn.onClick.AddListener(OnCloseClick);
        }

        protected internal override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        void OnCloseClick()
        {
            GameEntry.UI.CloseUIForm(this);
        }
    }
}
