/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/12/10
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

namespace LT
{
    /// <summary>
    /// 轴向接口
    /// </summary>
    public interface IAxis : IDevice
    {
        /// <summary>
        /// 垂直方向的插值输入,Range[-1,1]
        /// </summary>
        float Vertical { get; }

        /// <summary>
        /// 水平方向的插值输入,Range[-1,1]
        /// </summary>
        float Horizontal { get; }

        /// <summary>
        /// 垂直方向输入,Values(-1,0,1)
        /// </summary>
        int VerticalRaw { get; }

        /// <summary>
        /// 水平方向输入,Values(-1,0,1)
        /// </summary>
        int HorizontalRaw { get; }
    }
}
