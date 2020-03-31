/*
*    描述:
*          1. 配置
*
*    开发人: 邓平
*/
using System;
using UnityEngine;

namespace LtFramework.Util
{
    [Serializable]
    public class GameConfigProxy
    {
        private static GameConfigProxy _Instance;

        public static GameConfigProxy Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new GameConfigProxy();
                }

                return _Instance;
            }
        }

        private GameConfigProxy()
        {
        }


        private ConfigInfo _ConfigInfo;
        private readonly string _JsonPath = "Json/ConfigInfo";

        public ConfigInfo configInfo
        {
            get { return _ConfigInfo; }
        }

        public void Init()
        {
            CustomsJson();
        }

        private void CustomsJson()
        {
            try
            {
                //得到文件流
                TextAsset tableObj = Resources.Load<TextAsset>(_JsonPath);
                //反序列化 从文件到对象
                ConfigInfo info = JsonUtility.FromJson<ConfigInfo>(tableObj.text);
                //显示对象中数据
                _ConfigInfo = info;
            }
            catch (Exception)
            {
                Debug.LogError("获取配置文件出错 Path:" + _JsonPath);
            }

        }

        public void SetConfigInfo(ConfigInfo info)
        {
            _ConfigInfo = info;
        }

        public override string ToString()
        {
            return string.Format(
                "isSDK ：是否对接SDK ->{0}\nopenShouBing : 打开手柄 ->{1}\nopenDouble : 是否打开双人模式 ->{2}\n" +
                "openErWeiMa : 是否显示二维码 ->{3}\nisInAppPurchase : 是否内购 ->{4}\nisLocalSave : 游戏存档是否存本地 ->{5}\n" +
                "openMonth : 是否打开包月 ->{6}\nopenStageFree : 是否打开阶段免费进入 ->{7}\n", _ConfigInfo.IsSdk,
                _ConfigInfo.OpenShouBing, _ConfigInfo.OpenDouble, _ConfigInfo.OpenErWeiMa, _ConfigInfo.IsInAppPurchase,
                _ConfigInfo.IsLocalSave,
                _ConfigInfo.OpenMonth, _ConfigInfo.OpenStageFree
            );
        }

    }

}
