/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：解析器接口
 * 
 * ------------------------------------------------------------------------------*/

namespace LT.Net
{
    /// <summary>
    /// 打包接口
    /// </summary>
    public interface IPackager
    {
        IMessage Decode(byte[] bytes);
        IMessage Decode(string data);
    }
}
