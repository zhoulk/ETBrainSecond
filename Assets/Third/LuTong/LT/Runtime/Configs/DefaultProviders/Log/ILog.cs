/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/05/16
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;

namespace LT
{
    /// <summary>
    /// 框架内部使用的日志接口
    /// </summary>
	public interface ILog
    {
        void Debug(object message);

        void Warning(object message);

        void Error(object message);
    }
}