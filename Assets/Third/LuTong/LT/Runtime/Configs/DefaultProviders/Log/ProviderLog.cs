/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/05/16
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

namespace LT
{
    /// <summary>
    /// 日志服务，一般在框架内部使用。
    /// 目的是为了让框架与UnityEngine.Debug解耦
    /// </summary>
    public class ProviderLog : IServiceProvider
    {
        /// <summary>
        /// 注册服务提供者
        /// </summary>
        public void Register()
        {
            App.Singleton<ILog, UnityDebugAdapter>();
        }

        /// <summary>
        /// 服务提供者初始化
        /// </summary>
        public void Init()
        {
            App.Make<ILog>();
        }
    }
}