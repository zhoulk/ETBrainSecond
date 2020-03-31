/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：32位唯一id 生成器
 * 
 * ------------------------------------------------------------------------------*/

using System;

namespace LT
{
    /// <summary>
    /// 32位唯一生成器
    /// </summary>
    public static class ConvGenerator
    {
        static readonly DateTime UtcTime = new DateTime(1970, 1, 1);

        public static uint Conv()
        {
            return (uint)(Convert.ToInt64(DateTime.UtcNow.Subtract(UtcTime).TotalMilliseconds) & 0xffffffff);
        }
    }
}