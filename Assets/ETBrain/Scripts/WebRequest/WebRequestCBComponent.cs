
using GameFramework;
using GameFramework.Event;
using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace ETBrain
{
    public partial class WebRequestCBComponent : GameFrameworkComponent
    {
        Dictionary<int, Action<string>> m_responseActions = new Dictionary<int, Action<string>>();

        public void InitWebRequest()
        {
            GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
        }

        //private void OnDestroy()
        //{
        //    GameEntry.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
        //    GameEntry.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
        //}

        void Request(string url, object param, Action<string> response)
        {
            string jsonStr = Utility.Json.ToJson(param);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonStr);

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("content-type", "application/json;charset=utf-8");

            int serialId = GameEntry.WebRequest.AddWebRequest(url, bytes, headers);
            m_responseActions[serialId] = response;
        }

        private void OnWebRequestSuccess(object sender, GameEventArgs e)
        {
            WebRequestSuccessEventArgs ne = (WebRequestSuccessEventArgs)e;

            if (!m_responseActions.ContainsKey(ne.SerialId))
            {
                Log.Error("网络请求异常" + ne.WebRequestUri);
            }

            string responseJson = Utility.Converter.GetString(ne.GetWebResponseBytes());
            m_responseActions[ne.SerialId](responseJson);
            m_responseActions.Remove(ne.SerialId);
        }

        private void OnWebRequestFailure(object sender, GameEventArgs e)
        {
            WebRequestFailureEventArgs ne = (WebRequestFailureEventArgs)e;
            Log.Warning(ne.WebRequestUri + "  webRequest failure.");
        }
    }
}
