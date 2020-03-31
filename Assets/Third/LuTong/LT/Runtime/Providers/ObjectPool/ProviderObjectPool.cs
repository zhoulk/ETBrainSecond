/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/08/08
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using LT.Container;
using UnityEngine;

namespace LT.ObjectPool
{
    public class ProviderObjectPool : MonoBehaviour, IServiceProvider
    {
        //
        // 摘要:
        //     服务提供者初始化
        public void Init()
        {
            App.Make<IObjectPoolManager>();
        }

        //
        // 摘要:
        //     注册服务提供者
        public void Register()
        {
            App.SingletonIf<IObjectPoolManager, ObjectPoolManager>();
        }
    }
}
