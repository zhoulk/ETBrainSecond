/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/04/27
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using LT.Container;
using UnityEngine;

namespace LT.Fsm
{
    /// <summary>
    /// 有限状态机管理服务
    /// </summary>
	public class ProviderFsm : MonoBehaviour, IServiceProvider
    {
        /// <summary>
        /// 当注册服务提供者
        /// </summary>
        public void Register()
        {
            App.SingletonIf<IFsmManager, FsmManager>();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns>迭代器</returns>
        public void Init()
        {
            App.Make<IFsmManager>();
        }
    }
}