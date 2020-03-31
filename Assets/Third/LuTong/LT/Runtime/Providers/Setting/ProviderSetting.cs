/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/12/19
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/
using UnityEngine;

namespace LT.Setting
{
    /// <summary>
    /// 配置服务
    /// </summary>
    public class ProviderSetting : MonoBehaviour, IServiceProvider
    {
        /// <summary>
        /// 当注册服务提供者
        /// </summary>
        public void Register()
        {
            App.Singleton<ISetting, SettingJson>();
        }

        /// <summary>
        /// 服务提供者初始化
        /// </summary>
        public void Init()
        {
            App.Make<ISetting>();
        }
    }
}