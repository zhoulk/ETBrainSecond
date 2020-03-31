/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/12/19
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

namespace LT.Json
{
    /// <summary>
    /// json服务提供者
    /// </summary>
    public class ProviderJson : IServiceProvider
    {
        /// <summary>
        /// 当注册服务提供者
        /// </summary>
        public void Register()
        {
            App.Singleton<IJson, LitJsonAdapter>();
        }

        /// <summary>
        /// 服务提供者初始化
        /// </summary>
        public void Init()
        {
            App.Make<IJson>();
        }
    }
}
