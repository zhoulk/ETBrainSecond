/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：游戏手柄服务
 * 
 * ------------------------------------------------------------------------------*/
using UnityEngine;

namespace LT.GamepadServer
{
    /// <summary>
    /// 游戏手柄测试服务
    /// </summary>
    public class ProviderTestGamepadServer : MonoBehaviour, IServiceProvider
    {
        /// <summary>
        /// 注册服务
        /// </summary>
        public void Register()
        {
            App.Singleton<IGamepadServer, GamepadServer>();
        }

        /// <summary>
        /// 初始化服务
        /// </summary>
        public void Init()
        {
            App.Make<IGamepadServer>();
        }
    }
}