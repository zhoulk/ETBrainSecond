
using LtFramework.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETBrain
{
    public class LoginForm: UGuiForm
    {
        private Button mCloseBtn;

        protected internal override void OnInit(object userData)
        {
            base.OnInit(userData);

            ButtonOnClick("BtnLogin", OnWXLoginClick);
            ButtonOnClick("BtnClose", OnCloseClick);
        }

        protected internal override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            ButtonDefaultFocus("BtnLogin");
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
