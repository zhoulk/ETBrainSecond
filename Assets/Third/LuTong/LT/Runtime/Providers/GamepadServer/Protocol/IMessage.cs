/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：消息接口
 * 
 * ------------------------------------------------------------------------------*/

namespace LT.Net
{
    /// <summary>
    /// 消息接口
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// 获取消息类型
        /// </summary>
        MessageType GetMessageType();

        /// <summary>
        /// 加密
        /// </summary>
        byte[] Encode();

        /// <summary>
        /// 解码
        /// </summary>
        int Decode(byte[] value, int startIndex);

        /// <summary>
        /// 字符串
        /// </summary>
        string ConvertString();
    }
}
