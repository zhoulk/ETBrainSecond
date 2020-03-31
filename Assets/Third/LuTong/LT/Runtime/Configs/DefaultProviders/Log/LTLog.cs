/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/05/22
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

namespace LT
{
    /// <summary>
    /// Log 门面
    /// </summary>
	public sealed class LTLog : Facade<ILog>
    {
        public static void Debug(object message)
        {
            That.Debug(message);
        }

        public static void Warning(object message)
        {
            That.Warning(message);
        }

        public static void Error(object message)
        {
            That.Error(message);
        }
    }
}