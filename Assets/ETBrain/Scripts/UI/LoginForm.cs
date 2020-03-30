
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETBrain
{
    public class LoginForm: UGuiForm
    {
        private Button mCloseBtn;
        private Button mWXLoginBtn;

        protected internal override void OnInit(object userData)
        {
            base.OnInit(userData);
            mCloseBtn = CachedTransform.Find("closeBtn").GetComponent<Button>();
            mWXLoginBtn = CachedTransform.Find("content/login").GetComponent<Button>();

            mCloseBtn.onClick.AddListener(OnCloseClick);
            mWXLoginBtn.onClick.AddListener(OnWXLoginClick);
        }

        protected internal override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        void OnCloseClick()
        {
            GameEntry.UI.CloseUIForm(this);
        }

        void OnWXLoginClick()
        {
            GameEntry.WebRequestCB.GetHttpToken(new TokenRequest(), (UF_TokenResponse response) =>
            {
                Log.Info(response.data);
            });
            //PlatformAccessTokenRequest request = new PlatformAccessTokenRequest();
            //request.code = "12121212";
            //request.grantType = "authorization_code";
            //request.type = "nldmx";
            //GameEntry.WebRequestCB.GetWXAccessToken(request, (PlatformAccessTokenResponse response) =>
            //{

            //});
        }
    }
}
