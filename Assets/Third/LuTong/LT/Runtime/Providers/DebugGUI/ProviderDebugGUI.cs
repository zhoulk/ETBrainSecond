/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/05/16
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;

namespace LT.DebugerGUI
{
    /// <summary>
    /// Debug可视化窗口服务
    /// </summary>
    public class ProviderDebugGUI : MonoBehaviour, IServiceProvider
    {
        /// <inheritdoc />
        public void Register()
        {
            App.Singleton<IDebugGUI, DebugGUI>();
        }

        /// <inheritdoc />
        public void Init()
        {
            App.Make<IDebugGUI>();
        }
    }
}