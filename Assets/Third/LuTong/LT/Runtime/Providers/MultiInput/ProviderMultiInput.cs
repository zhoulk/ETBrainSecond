/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：多人输入服务类
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;

namespace LT
{
    /// <summary>
    /// 提供多人输入服务
    /// </summary>
    public class ProviderMultiInput : MonoBehaviour, IServiceProvider
    {
        /// <summary>
        /// 注册服务
        /// </summary>
        public void Register()
        {
            App.Singleton<IInput, MultiInput>();
        }

        /// <summary>
        /// 初始化服务
        /// </summary>
        public void Init()
        {
            App.Make<IInput>();
        }
    }
}

