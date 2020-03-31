/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：Sdk公共类服务
 * 
 * ------------------------------------------------------------------------------*/
using UnityEngine;
using LT.Container;

namespace LT.Sdk
{
    /// <summary>
    /// sdk公共服务,包括调起支付，获取二维码图片等功能
    /// </summary>
    public class ProviderSdkTool : MonoBehaviour, IServiceProvider
    {
        /// <summary>
        /// 注册服务
        /// </summary>
        public void Register()
        {
            App.SingletonIf<Unity2AndroidHelper>();
            App.Singleton<ISdkTool, SdkTool>();
        }

        /// <summary>
        /// 初始化服务
        /// </summary>
        public void Init()
        {
            App.Make<ISdkTool>();
        }
    }
}