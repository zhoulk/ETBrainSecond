/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/12/19
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using LitJson;

namespace LT.Json
{
    /// <summary>
    /// Json适配器
    /// </summary>
    public class LitJsonAdapter : IJson
    {
        /// <summary>
        /// Json转化为Object
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="json">json数据</param>
        /// <returns>反序列的类型</returns>
        public T FromJson<T>(string json)
        {
            return JsonMapper.ToObject<T>(json);
        }

        /// <summary>
        /// Json转化为Object
        /// </summary>
        /// <param name="json">json数据</param>
        /// <param name="type">反序列的类型</param>
        /// <returns>反序列的结果</returns>
        public object FromJson(string json, Type type)
        {
            return JsonMapper.ToObject(json, type);
        }

        /// <summary>
        /// Object转化为Json
        /// </summary>
        /// <param name="item">序列化的目标</param>
        /// <returns>序列化的结果</returns>
        public string ToJson(object item)
        {
            return JsonMapper.ToJson(item);
        }
    }
}