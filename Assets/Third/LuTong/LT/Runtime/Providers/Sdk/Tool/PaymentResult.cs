/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/29
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/
using System;

namespace LT.Sdk
{
    /// <summary>
    /// 支付结果
    /// </summary>
    [Serializable]
    public class PaymentResult
    {
        /// <summary>
        /// code == 100 为成功
        /// </summary>
        public string code;

        /// <summary>
        /// 支付id
        /// </summary>
        public string payid;

        /// <summary>
        /// 文本
        /// </summary>
        public string text;
    }
}