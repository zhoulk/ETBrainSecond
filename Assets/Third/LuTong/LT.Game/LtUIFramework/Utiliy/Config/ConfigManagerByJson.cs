/*
*    描述:
*          1. 基于Json 配置文件的“配置管理器”  
*
*    开发人: 邓平
*/
using System.Collections.Generic;
using UnityEngine;

namespace LtFramework.Util
{
    public class ConfigManagerByJson : IConfigManager
    {
        //保存（键值对）应用设置集合
        private static Dictionary<string, string> _AppSettingDic;

        /// <summary>
        /// 只读属性： 得到应用设置（键值对集合）
        /// </summary>
        public Dictionary<string, string> AppSetting
        {
            get { return _AppSettingDic; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param UIName="jsonPath">Json配置文件路径</param>
        public ConfigManagerByJson(string jsonPath)
        {
            _AppSettingDic = new Dictionary<string, string>();
            //初始化解析Json 数据，加载到（appSettingDic）集合。
            InitAndAnalysisJson(jsonPath);
        }

        /// <summary>
        /// 得到AppSetting 的最大数值
        /// </summary>
        /// <returns></returns>
        public int GetAppSettingMaxNumber()
        {
            if (_AppSettingDic != null && _AppSettingDic.Count >= 1)
            {
                return _AppSettingDic.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 初始化解析Json 数据，加载到集合众。
        /// </summary>
        /// <param UIName="jsonPath"></param>
        private void InitAndAnalysisJson(string jsonPath)
        {
            TextAsset configInfo = null;
            KeyValuesInfo keyvalueInfoObj = null;

            //参数检查
            if (string.IsNullOrEmpty(jsonPath)) return;
            //解析Json 配置文件
            try
            {
                configInfo = Resources.Load<TextAsset>(jsonPath);
                keyvalueInfoObj = JsonUtility.FromJson<KeyValuesInfo>(configInfo.text);
            }
            catch
            {
                throw new JsonAnlysisException(
                    GetType() + "/InitAndAnalysisJson()/Json Analysis Exception ! Parameter jsonPath=" + jsonPath);
            }

            //数据加载到AppSetting 集合中
            foreach (KeyValuesNode nodeInfo in keyvalueInfoObj.ConfigInfo)
            {
                _AppSettingDic.Add(nodeInfo.Key, nodeInfo.Value);
            }
        }
    }
}