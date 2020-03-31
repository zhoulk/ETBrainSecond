/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/06/26
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.IO;
using LT.Json;
using LT.MonoDriver;
using LitJson;

namespace LT.Setting
{
    /// <summary>
    /// Json配置，可修改存储路径
    /// </summary>
    public sealed class SettingJson : ISetting, IOnDestroy, IOnApplicationQuit
    {
        /// <summary>
        /// 存储路径
        /// </summary>
        public static string SavedPath = $"{UnityEngine.Application.persistentDataPath}/PlayerPrefs.txt";

        /// <summary>
        /// json解析器
        /// </summary>
        private IJson m_Json;

        /// <summary>
        /// 缓存
        /// </summary>
        private JsonData m_PlayerPrefs;

        /// <summary>
        /// 构造方法
        /// </summary>
        public SettingJson(IJson json)
        {
            this.m_Json = json;
            this.m_PlayerPrefs = new JsonData();

            Load();
        }

        /// <summary>
        /// 加载配置。
        /// </summary>
        /// <returns>是否加载配置成功。</returns>
        public bool Load()
        {
            if (App.DebugLevel == DebugLevel.Development)
            {
                LTLog.Debug($"{typeof(SettingJson)}.LoadPath = {SavedPath}");
            }

            if (!File.Exists(SavedPath))
            {
                return true;
            }

            this.m_PlayerPrefs = JsonMapper.ToObject(File.ReadAllText(SavedPath));
            return true;
        }

        /// <summary>
        /// 保存配置。
        /// </summary>
        /// <returns>是否保存配置成功。</returns>
        public bool Save()
        {
            string data = this.m_PlayerPrefs.ToJson();

            if (App.DebugLevel == DebugLevel.Development)
            {
                LTLog.Debug($"{typeof(SettingJson)}.SavePath = {SavedPath}");
            }

            File.WriteAllText(SavedPath, data);
            return true;
        }

        /// <summary>
        /// 检查是否存在指定配置项。
        /// </summary>
        /// <param name="settingName">要检查配置项的名称。</param>
        /// <returns>指定的配置项是否存在。</returns>
        public bool HasSetting(string settingName)
        {
            return m_PlayerPrefs.ContainsKey(settingName);
        }

        /// <summary>
        /// 移除指定配置项。
        /// </summary>
        /// <param name="settingName">要移除配置项的名称。</param>
        public void RemoveSetting(string settingName)
        {
            if (m_PlayerPrefs.ContainsKey(settingName))
                m_PlayerPrefs.Remove(settingName);
        }

        /// <summary>
        /// 清空所有配置项。
        /// </summary>
        public void RemoveAllSettings()
        {
            m_PlayerPrefs.Clear();
        }

        /// <summary>
        /// 从指定配置项中读取布尔值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string settingName)
        {
            if (m_PlayerPrefs.ContainsKey(settingName))
            {
                return (bool)m_PlayerPrefs[settingName];
            }

            return false;
        }

        /// <summary>
        /// 从指定配置项中读取布尔值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string settingName, bool defaultValue)
        {
            if (m_PlayerPrefs.ContainsKey(settingName))
            {
                return (bool)m_PlayerPrefs[settingName];
            }

            return defaultValue;
        }

        /// <summary>
        /// 向指定配置项写入布尔值。
        /// </summary>
        /// <param name="settingName">要写入配置项的名称。</param>
        /// <param name="value">要写入的布尔值。</param>
        public void SetBool(string settingName, bool value)
        {
            m_PlayerPrefs[settingName] = value;
        }

        /// <summary>
        /// 从指定配置项中读取整数值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string settingName)
        {
            if (m_PlayerPrefs.ContainsKey(settingName))
            {
                return (int)m_PlayerPrefs[settingName];
            }
            return 0;
        }

        /// <summary>
        /// 从指定配置项中读取整数值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string settingName, int defaultValue)
        {
            if (m_PlayerPrefs.ContainsKey(settingName))
            {
                return (int)m_PlayerPrefs[settingName];
            }
            return defaultValue;
        }

        /// <summary>
        /// 向指定配置项写入整数值。
        /// </summary>
        /// <param name="settingName">要写入配置项的名称。</param>
        /// <param name="value">要写入的整数值。</param>
        public void SetInt(string settingName, int value)
        {
            m_PlayerPrefs[settingName] = value;
        }

        /// <summary>
        /// 从指定配置项中读取浮点数值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string settingName)
        {
            if (m_PlayerPrefs.ContainsKey(settingName))
            {
                return (float)m_PlayerPrefs[settingName];
            }
            return 0f;
        }

        /// <summary>
        /// 从指定配置项中读取浮点数值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string settingName, float defaultValue)
        {
            if (m_PlayerPrefs.ContainsKey(settingName))
            {
                return (float)m_PlayerPrefs[settingName];
            }
            return defaultValue;
        }

        /// <summary>
        /// 向指定配置项写入浮点数值。
        /// </summary>
        /// <param name="settingName">要写入配置项的名称。</param>
        /// <param name="value">要写入的浮点数值。</param>
        public void SetFloat(string settingName, float value)
        {
            m_PlayerPrefs[settingName] = value;
        }

        /// <summary>
        /// 从指定配置项中读取字符串值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string settingName)
        {
            if (m_PlayerPrefs.ContainsKey(settingName))
            {
                return m_PlayerPrefs[settingName].ToString();
            }
            return "";
        }

        /// <summary>
        /// 从指定配置项中读取字符串值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string settingName, string defaultValue)
        {
            if (m_PlayerPrefs.ContainsKey(settingName))
            {
                return m_PlayerPrefs[settingName].ToString();
            }
            return defaultValue;
        }

        /// <summary>
        /// 向指定配置项写入字符串值。
        /// </summary>
        /// <param name="settingName">要写入配置项的名称。</param>
        /// <param name="value">要写入的字符串值。</param>
        public void SetString(string settingName, string value)
        {
            m_PlayerPrefs[settingName] = value;
        }

        /// <summary>
        /// 从指定配置项中读取对象。
        /// </summary>
        /// <typeparam name="T">要读取对象的类型。</typeparam>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <returns>读取的对象。</returns>
        public T GetObject<T>(string settingName)
        {
            if (m_PlayerPrefs.ContainsKey(settingName))
            {
                return m_Json.FromJson<T>(m_PlayerPrefs[settingName].ToString());
            }

            return default;
        }

        /// <summary>
        /// 从指定配置项中读取对象。
        /// </summary>
        /// <param name="objectType">要读取对象的类型。</param>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <returns></returns>
        public object GetObject(Type objectType, string settingName)
        {
            if (m_PlayerPrefs.ContainsKey(settingName))
            {
                return m_Json.FromJson(m_PlayerPrefs[settingName].ToString(), objectType);
            }
            return default;
        }

        /// <summary>
        /// 从指定配置项中读取对象。
        /// </summary>
        /// <typeparam name="T">要读取对象的类型。</typeparam>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认对象。</param>
        /// <returns>读取的对象。</returns>
        public T GetObject<T>(string settingName, T defaultValue)
        {
            if (m_PlayerPrefs.ContainsKey(settingName))
            {
                return m_Json.FromJson<T>(m_PlayerPrefs[settingName].ToString());
            }

            return defaultValue;
        }

        /// <summary>
        /// 从指定配置项中读取对象。
        /// </summary>
        /// <param name="objectType">要读取对象的类型。</param>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认对象。</param>
        /// <returns></returns>
        public object GetObject(Type objectType, string settingName, object defaultValue)
        {
            if (m_PlayerPrefs.ContainsKey(settingName))
            {
                return m_Json.FromJson(m_PlayerPrefs[settingName].ToString(), objectType);
            }

            return defaultValue;
        }

        /// <summary>
        /// 向指定配置项写入对象。
        /// </summary>
        /// <param name="settingName">要写入配置项的名称。</param>
        /// <param name="value">要写入的对象。</param>
        public void SetObject(string settingName, object value)
        {
            m_PlayerPrefs[settingName] = m_Json.ToJson(value);
        }

        public void OnDestroy()
        {
            Save();
        }

        public void OnApplicationQuit()
        {
            //当应用退出时，自动保存数据
            Save();
        }
    }
}