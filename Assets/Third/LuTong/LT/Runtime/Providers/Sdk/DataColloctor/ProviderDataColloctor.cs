/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：数据采集服务
 * 
 * ------------------------------------------------------------------------------*/
using UnityEngine;
using LT.Container;

namespace LT.Sdk
{
    /// <summary>
    /// 数据采集服务
    /// </summary>
    public class ProviderDataColloctor : MonoBehaviour, IServiceProvider
    {
        /// <summary>
        /// 注册服务
        /// </summary>
        public void Register()
        {
            App.SingletonIf<Unity2AndroidHelper>();
            App.Singleton<IDataColloctor, DataColloctor>();
        }

        /// <summary>
        /// 初始化服务
        /// </summary>
        public void Init()
        {
            App.Make<IDataColloctor>();
        }
    }
}