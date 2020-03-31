/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/12/19
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;
using System;
using LT.Json;
using LT.MonoDriver;

namespace LT.Setting
{
    /// <summary>
    /// 默认数据配置类，封装了Unity的PlayerPrefs
    /// </summary>
    internal sealed class SettingDefault : ISetting, IOnApplicationQuit
    {
        /// <summary>
        /// json解析器
        /// </summary>
        private IJson m_Json;

        /// <summary>
        /// 构造方法
        /// </summary>
        public SettingDefault(IJson json)
        {
            this.m_Json = json;
        }

        /// <summary>
        /// 加载配置。
        /// </summary>
        /// <returns>是否加载配置成功。</returns>
        public bool Load()
        {
            return true;
        }

        /// <summary>
        /// 保存配置。
        /// </summary>
        /// <returns>是否保存配置成功。</returns>
        public bool Save()
        {
            PlayerPrefs.Save();
            return true;
        }

        /// <summary>
        /// 检查是否存在指定配置项。
        /// </summary>
        /// <param name="settingName">要检查配置项的名称。</param>
        /// <returns>指定的配置项是否存在。</returns>
        public bool HasSetting(string settingName)
        {
            return PlayerPrefs.HasKey(settingName);
        }

        /// <summary>
        /// 移除指定配置项。
        /// </summary>
        /// <param name="settingName">要移除配置项的名称。</param>
        public void RemoveSetting(string settingName)
        {
            PlayerPrefs.DeleteKey(settingName);
        }

        /// <summary>
        /// 清空所有配置项。
        /// </summary>
        public void RemoveAllSettings()
        {
            PlayerPrefs.DeleteAll();
        }

        /// <summary>
        /// 从指定配置项中读取布尔值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string settingName)
        {
            return PlayerPrefs.GetInt(settingName) != 0;
        }

        /// <summary>
        /// 从指定配置项中读取布尔值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string settingName, bool defaultValue)
        {
            return PlayerPrefs.GetInt(settingName, defaultValue ? 1 : 0) != 0;
        }

        /// <summary>
        /// 向指定配置项写入布尔值。
        /// </summary>
        /// <param name="settingName">要写入配置项的名称。</param>
        /// <param name="value">要写入的布尔值。</param>
        public void SetBool(string settingName, bool value)
        {
            PlayerPrefs.SetInt(settingName, value ? 1 : 0);
        }

        /// <summary>
        /// 从指定配置项中读取整数值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string settingName)
        {
            return PlayerPrefs.GetInt(settingName);
        }

        /// <summary>
        /// 从指定配置项中读取整数值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string settingName, int defaultValue)
        {
            return PlayerPrefs.GetInt(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定配置项写入整数值。
        /// </summary>
        /// <param name="settingName">要写入配置项的名称。</param>
        /// <param name="value">要写入的整数值。</param>
        public void SetInt(string settingName, int value)
        {
            PlayerPrefs.SetInt(settingName, value);
        }

        /// <summary>
        /// 从指定配置项中读取浮点数值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string settingName)
        {
            return PlayerPrefs.GetFloat(settingName);
        }

        /// <summary>
        /// 从指定配置项中读取浮点数值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string settingName, float defaultValue)
        {
            return PlayerPrefs.GetFloat(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定配置项写入浮点数值。
        /// </summary>
        /// <param name="settingName">要写入配置项的名称。</param>
        /// <param name="value">要写入的浮点数值。</param>
        public void SetFloat(string settingName, float value)
        {
            PlayerPrefs.SetFloat(settingName, value);
        }

        /// <summary>
        /// 从指定配置项中读取字符串值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string settingName)
        {
            return PlayerPrefs.GetString(settingName);
        }

        /// <summary>
        /// 从指定配置项中读取字符串值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string settingName, string defaultValue)
        {
            return PlayerPrefs.GetString(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定配置项写入字符串值。
        /// </summary>
        /// <param name="settingName">要写入配置项的名称。</param>
        /// <param name="value">要写入的字符串值。</param>
        public void SetString(string settingName, string value)
        {
            PlayerPrefs.SetString(settingName, value);
        }

        /// <summary>
        /// 从指定配置项中读取对象。
        /// </summary>
        /// <typeparam name="T">要读取对象的类型。</typeparam>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <returns>读取的对象。</returns>
        public T GetObject<T>(string settingName)
        {
            return m_Json.FromJson<T>(PlayerPrefs.GetString(settingName));
        }

        /// <summary>
        /// 从指定配置项中读取对象。
        /// </summary>
        /// <param name="objectType">要读取对象的类型。</param>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <returns></returns>
        public object GetObject(Type objectType, string settingName)
        {
            return m_Json.FromJson(PlayerPrefs.GetString(settingName), objectType);
        }

        /// <summary>
        /// 从指定配置项中读取对象。
        /// </summary>
        /// <typeparam name="T">要读取对象的类型。</typeparam>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <param name="defaultObj">当指定的配置项不存在时，返回此默认对象。</param>
        /// <returns>读取的对象。</returns>
        public T GetObject<T>(string settingName, T defaultObj)
        {
            string json = PlayerPrefs.GetString(settingName, "");
            if (json.Equals(""))
            {
                return defaultObj;
            }

            return this.m_Json.FromJson<T>(json);
        }

        /// <summary>
        /// 从指定配置项中读取对象。
        /// </summary>
        /// <param name="objectType">要读取对象的类型。</param>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <param name="defaultObj">当指定的配置项不存在时，返回此默认对象。</param>
        /// <returns></returns>
        public object GetObject(Type objectType, string settingName, object defaultObj)
        {
            string json = PlayerPrefs.GetString(settingName, "");
            if (json.Equals(""))
            {
                return defaultObj;
            }

            return this.m_Json.FromJson(json, objectType);
        }

        /// <summary>
        /// 向指定配置项写入对象。
        /// </summary>
        /// <param name="settingName">要写入配置项的名称。</param>
        /// <param name="obj">要写入的对象。</param>
        public void SetObject(string settingName, object obj)
        {
            PlayerPrefs.SetString(settingName, m_Json.ToJson(obj));
        }

        /// <summary>
        /// Monobehavior OnApplicationQuit
        /// </summary>
        public void OnApplicationQuit()
        {
            //当应用退出时，自动保存数据
            Save();
        }
    }
}