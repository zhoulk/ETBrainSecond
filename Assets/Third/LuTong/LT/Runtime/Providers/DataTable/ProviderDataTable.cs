/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/07/01
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;

namespace LT.DataTable
{
    /// <summary>
    /// 数据表服务
    /// </summary>
    public class ProviderDataTable : MonoBehaviour, IServiceProvider
    {
        /// <inheritdoc />
        public void Register()
        {
            App.Singleton<IDataTableManager, DataTableManager>();
        }

        /// <inheritdoc />
        public void Init()
        {
            App.Make<IDataTableManager>();
        }
    }
}