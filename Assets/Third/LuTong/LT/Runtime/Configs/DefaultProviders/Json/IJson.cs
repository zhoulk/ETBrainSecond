/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/12/19
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;

namespace LT.Json
{
    /// <summary>
    /// Json接口
    /// </summary>
    public interface IJson
    {
        /// <summary>
        /// Json转化为Object
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="json">json数据</param>
        /// <returns>反序列的类型</returns>
        T FromJson<T>(string json);

        /// <summary>
        /// Json转化为Object
        /// </summary>
        /// <param name="json">json数据</param>
        /// <param name="type">反序列的类型</param>
        /// <returns>反序列的结果</returns>
        object FromJson(string json, Type type);

        /// <summary>
        /// Object转化为Json
        /// </summary>
        /// <param name="item">序列化的目标</param>
        /// <returns>序列化的结果</returns>
        string ToJson(object item);
    }
}
