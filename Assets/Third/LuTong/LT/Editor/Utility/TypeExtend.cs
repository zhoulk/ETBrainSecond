/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/05/20
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Reflection;

namespace LT.Editor
{
    internal static class TypeExtend
    {
        /// <summary>
        /// 获取指定基类的所有子类的名称。
        /// </summary>
        /// <param name="typeBase">基类类型。</param>
        /// <returns>指定基类的所有子类的名称。</returns>
        internal static string[] GetSubTypeNames(this Type typeBase)
        {
            var assembiles = Arr.Filter(AppDomain.CurrentDomain.GetAssemblies(), InspectorTool.TestCheckInAssembiles);

            List<string> typeNames = new List<string>();
            foreach (var assembly in assembiles)
            {
                if (assembly == null)
                {
                    continue;
                }

                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (type.IsClass && !type.IsAbstract && typeBase.IsAssignableFrom(type))
                    {
                        typeNames.Add(type.FullName);
                    }
                }
            }

            typeNames.Sort();
            return typeNames.ToArray();
        }
    }
}