/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/04/29
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;

namespace LT.Fps
{
    public class ProviderFps : MonoBehaviour, IServiceProvider
    {
        /// <inheritdoc />
        public void Register()
        {
            App.Singleton<Fps>();
        }

        /// <inheritdoc />
        public void Init()
        {
            App.Make<Fps>();
        }
    }
}