/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/12/11
 * 模块描述：Sdk公用类实现,提供二维码扫码下载图，支付等功能
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using LT.MonoDriver;
using LT.Json;
using LT.Net;
using LT.GamepadServer;

namespace LT.Sdk
{
    /// <summary>
    /// Sdk工具类
    /// </summary>
    internal class SdkTool : ISdkTool
    {
        private IMonoDriver m_MonoDriver;
        private IJson m_Json;
        private ILog m_Log;
        private AndroidJavaClass m_BridgeClass;
        private LTTaskCompletionSource<PaymentResult> m_PaymentTcs;

        /// <summary>
        /// 构建Sdk工具类
        /// </summary>
        public SdkTool(IMonoDriver monoDriver, ILog log, IJson json, Unity2AndroidHelper unity2Android, Component component = null)
        {
            this.m_MonoDriver = monoDriver;
            this.m_Json = json;
            this.m_Log = log;

            if (component != null)
            {
                InitComponent(component);
            }

            m_BridgeClass = unity2Android.BridgeClass;
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        /// <param name="component">Unity组件</param>
        private void InitComponent(Component component)
        {
            Android2UnityBridge behaviour = component.gameObject.AddComponent<Android2UnityBridge>();
            behaviour.SetDriver(this);
        }

        /// <inheritdoc />
        public LTTask<Sprite> GetQRCode2In1Async(int width, int height)
        {
            string url = "";

            if (m_BridgeClass != null)
            {
                url = m_BridgeClass.CallStatic<string>("getQRCode2In1", width, height, "QRCode2In1.png");
            }

            LTLog.Debug($"url:{url}");
            LTTaskCompletionSource<Sprite> tcs = LTTaskCompletionSource.New<Sprite>();
            m_MonoDriver.StartCoroutine(InnerLoadGamepadQRCode(tcs, url));
            return tcs.Task;
        }

        /// <inheritdoc />
        public LTTask<Sprite> GetDownloadQRCodeAsync(int width = 188, int height = 188)
        {
            string url = "";

            if (m_BridgeClass != null)
            {
                url = m_BridgeClass.CallStatic<string>("getDownLoadHandQRPath", width, height, "DownloadQRCode.png");
            }

            LTLog.Debug($"url:{url}");
            LTTaskCompletionSource<Sprite> tcs = LTTaskCompletionSource.New<Sprite>();
            m_MonoDriver.StartCoroutine(InnerLoadGamepadQRCode(tcs, url));
            return tcs.Task;
        }

        /// <inheritdoc />
        public LTTask<Sprite> GetLinkQRCodeAsync(int width = 188, int height = 188)
        {
            string url = "";

            if (m_BridgeClass != null)
            {
                url = m_BridgeClass.CallStatic<string>("getQRcodePathByIP", width, height, "LinkQRCode.png");
            }

            LTLog.Debug($"url:{url}");
            LTTaskCompletionSource<Sprite> tcs = LTTaskCompletionSource.New<Sprite>();
            m_MonoDriver.StartCoroutine(InnerLoadGamepadQRCode(tcs, url));
            return tcs.Task;
        }

        /// <inheritdoc />
        public LTTask<PaymentResult> PaymentAsync(string payid)
        {
            m_PaymentTcs = LTTaskCompletionSource.New<PaymentResult>();

            if (m_BridgeClass == null)
            {
                //如果未接sdk，则模拟一个支付成功的结果
                PaymentResult result = new PaymentResult();
                result.code = "100";
                result.payid = payid;
                result.text = "订购成功.";

                m_PaymentTcs.SetResult(result);
            }
            else
            {
                m_BridgeClass.CallStatic("payment", payid, "");
            }

            return m_PaymentTcs.Task;
        }

        //// <inheritdoc />
        public void AddProductTable<T>(List<T> productTable)
        {
            string str = m_Json.ToJson(productTable);

            LTLog.Debug($"AddProductTable:{str}");
            if (m_BridgeClass != null)
            {
                m_BridgeClass.CallStatic("addProductTable", str);
            }
        }

        /// <inheritdoc />
        public TConfig GetGameConfig<TConfig>() where TConfig : new()
        {
            if (m_BridgeClass == null)
            {
                return new TConfig();
            }
            else
            {
                string buf = m_BridgeClass.CallStatic<string>("getGameConfig");

                TConfig config = m_Json.FromJson<TConfig>(buf);
                return config;
            }
        }

        //// <inheritdoc />
        public void AddGoldCoin(int value) { }

        /// <inheritdoc />
        public int GetGoldCoin() { return 0; }

        /// <inheritdoc />
        public void ExitApp()
        {
            if (m_BridgeClass == null)
                UnityEngine.Application.Quit();
            else
                m_BridgeClass.CallStatic("exitApp");
        }

        /// <inheritdoc />
        public void SetSDKConfig(string json)
        {
            if (m_BridgeClass != null)
            {
                m_BridgeClass.CallStatic("setSDKConfig", json);
            }
        }

        /// <summary>
        /// 设置Sdk配置
        /// </summary>
        /// <param name="json">json对象</param>
        public void SetSDKConfig(object json)
        {
            if (m_BridgeClass != null)
            {
                m_BridgeClass.CallStatic("setSDKConfig", this.m_Json.ToJson(json));
            }
        }

        /// <inheritdoc />
        public void OpenMXLYTestData()
        {
            if (m_BridgeClass != null)
            {
                m_BridgeClass.CallStatic("openMXLYTestData");
            }
        }

        /// <inheritdoc />
        public void BroadcastDataToGamePad(string json)
        {
            m_Log.Debug($"BroadcastDataToGamePad:{json}");

            if (m_BridgeClass != null)
            {
                m_BridgeClass.CallStatic("broadcastDataToGamePad", json);
            }
        }

        /// <inheritdoc />
        public void BroadcastDataToGamePad<T>(T data)
        {
            if (m_BridgeClass != null)
            {
                string content = m_Json.ToJson(data);

                m_Log.Debug($"Sdk BroadcastDataToGamePad : {content}");

                IMessage msg = MessagesFactory.Msg(content);
                string base64 = Convert.ToBase64String(msg.Encode());

                m_Log.Debug($"Sdk BroadcastDataToGamePad Base64: {base64}");

                m_BridgeClass.CallStatic("broadcastDataToGamePad", base64);
            }
            else
            {
                if (App.HasBind<IGamepadServer>())
                {
                    string content = m_Json.ToJson(data);
                    m_Log.Debug($"BroadcastDataToGamePad : {content}");

                    IMessage msg = MessagesFactory.Msg(content);
                    App.Make<IGamepadServer>().BroadcastToGamepad(msg.Encode());
                }
                else
                {
                    LTLog.Debug($"GamepadServer not register, Broadcast cannot be used. ");
                }
            }
        }

        /// <inheritdoc />
        public void SaveGameQuality(int level)
        {
            m_Log.Debug($"SaveGameQuality:{level}");

            if (m_BridgeClass != null)
            {
                m_BridgeClass.CallStatic("saveGameQuality", level.ToString());
            }
        }

        #region 非接口实现方法
        /// <summary>
        /// 设置支付结果
        /// </summary>
        /// <param name="buf"></param>
        public void SetPaymentResult(string buf)
        {
            m_Log.Debug($"OnPaymentResult: {buf}.");

            PaymentResult result = JsonUtility.FromJson<PaymentResult>(buf);
            m_PaymentTcs.SetResult(result);
        }

        /// <summary>
        /// 加载手柄二维码
        /// </summary>
        /// <param name="tcs">任务</param>
        /// <param name="url">二维码地址</param>
        /// <returns></returns>
        private IEnumerator InnerLoadGamepadQRCode(LTTaskCompletionSource<Sprite> tcs, string url)
        {
            UnityWebRequest request = UnityWebRequest.Get($"file://{url}");
            request.downloadHandler = new DownloadHandlerTexture(true);
            yield return request.SendWebRequest();

            if (request.downloadHandler.isDone)
            {
                Texture2D texture = (request.downloadHandler as DownloadHandlerTexture).texture;
                tcs.SetResult(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)));
            }
            else if (request.isNetworkError || request.isHttpError)
            {
                m_Log.Error("General RQCode Error:" + request.error);
                tcs.SetResult(null);
            }
        }
        #endregion
    }
}