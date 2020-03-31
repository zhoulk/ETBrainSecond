/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2020/01/16
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using LT.Json;

namespace LT
{
    /// <summary>
    /// Json门面
    /// </summary>
    public sealed class LTJson : Facade<IJson>
    {
        /// <summary>
        /// Json转化为Object
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="json">json数据</param>
        /// <returns>反序列的类型</returns>
        public static T FromJson<T>(string json)
        {
            return That.FromJson<T>(json);
        }

        /// <summary>
        /// Json转化为Object
        /// </summary>
        /// <param name="json">json数据</param>
        /// <param name="type">反序列的类型</param>
        /// <returns>反序列的结果</returns>
        public static object FromJson(string json, Type type)
        {
            return That.FromJson(json, type);
        }

        /// <summary>
        /// Object转化为Json
        /// </summary>
        /// <param name="item">序列化的目标</param>
        /// <returns>序列化的结果</returns>
        public static string ToJson(object item)
        {
            return That.ToJson(item);
        }
    }
}