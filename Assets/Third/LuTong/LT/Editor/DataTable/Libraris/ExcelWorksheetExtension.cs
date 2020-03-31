/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/09/27
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ExcelWorksheet扩展方法
/// 将下标访问从1开始调整为从0开始
/// </summary>
public static class ExcelWorksheetExtension
{
    /// <summary>
    /// 获取表值，下标从0开始
    /// </summary>
    /// <param name="self"></param>
    /// <param name="row">行</param>
    /// <param name="cloumn">列</param>
    /// <returns></returns>
    public static object GetValueEx(this ExcelWorksheet self, int row, int cloumn)
    {
        return self.GetValue(row + 1, cloumn + 1);
    }

    /// <summary>
    /// 获取表值，下标从0开始
    /// </summary>
    /// <typeparam name="TResult">结果类型</typeparam>
    /// <param name="self"></param>
    /// <param name="row">行</param>
    /// <param name="cloumn">列</param>
    /// <returns></returns>
    public static TResult GetValueEx<TResult>(this ExcelWorksheet self, int row, int cloumn)
    {
        return self.GetValue<TResult>(row + 1, cloumn + 1);
    }

    /// <summary>
    /// 获取行数据,出现空值则停止读取
    /// </summary>
    /// <param name="self"></param>
    /// <param name="row"></param>
    /// <returns></returns>
    public static string[] GetRow(this ExcelWorksheet self, int row)
    {
        List<string> list = new List<string>();
        for (int i = 0; i < self.Dimension.Columns; i++)
        {
            var value = self.GetValueEx<string>(row, i);
            list.Add(value);
        }
        return list.ToArray();
    }
}