
using GameFramework;
using System;
using UnityGameFramework.Runtime;

namespace ETBrain
{
    public partial class WebRequestCBComponent : GameFrameworkComponent
    {
        public void GetDeviceBindList(PlatformDeviceBindRequest request, Action<PlatformDeviceBindResponse> response)
        {
            Request(WebUtility.GetUFUrl("device/bind-list"), request, (res) =>
            {
                ReferencePool.Release(request);
                PlatformDeviceBindResponse deviceBind = Utility.Json.ToObject<PlatformDeviceBindResponse>(res);
                if (deviceBind == null)
                {
                    Log.Error("Parse PlatformDeviceBindResponse failure.");
                    return;
                }
                if (response != null)
                {
                    response(deviceBind);
                }
            });
        }

        public void GetHttpToken(TokenRequest request, Action<UF_TokenResponse> response)
        {
            Request(WebUtility.GetEasyUrl("isg/token"), request, (res) =>
            {
                UF_TokenResponse token = Utility.Json.ToObject<UF_TokenResponse>(res);
                if (token == null)
                {
                    Log.Error("Parse PlatformDeviceBindResponse failure.");
                    return;
                }
                if (response != null)
                {
                    response(token);
                }
            });
        }

        public void GetWXAccessToken(PlatformAccessTokenRequest request, Action<PlatformAccessTokenResponse> response)
        {
            Request(WebUtility.GetEasyUrl("ssg/wechat/get-access-token"), request, (res) =>
            {
                PlatformAccessTokenResponse accessToken = Utility.Json.ToObject<PlatformAccessTokenResponse>(res);
                if (accessToken == null)
                {
                    Log.Error("Parse PlatformDeviceBindResponse failure.");
                    return;
                }
                if (response != null)
                {
                    response(accessToken);
                }
            });
        }
    }
}
