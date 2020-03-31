/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/09/27
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace LT.DataTable
{
    /// <summary>
    /// 数据表资源基类
    /// </summary>
    public abstract class ScriptableObjectBase : ScriptableObject
    {
        public abstract bool AddDataRow(string[] dataRows);
    }

    /// <summary>
    /// 数据表资源泛型基类
    /// </summary>
    public class ScriptableObjectBase<T> : ScriptableObjectBase where T : IDataRow, new()
    {
        public List<T> dataSet = new List<T>();

        public override bool AddDataRow(string[] dataRows)
        {
            try
            {
                T dataRow = new T();
                if (!dataRow.ParseDataRow(dataRows))
                {
                    return false;
                }

                dataSet.Add(dataRow);
                return true;
            }
            catch (Exception e)
            {
                throw new LogicException(string.Format("Can not parse data table '{0}' with exception '{1}'.", Utility.Text.GetFullName<T>(string.Empty), e.ToString()), e);
            }
        }
    }
}