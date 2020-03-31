/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/3/21
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System;

namespace LT
{
    /// <summary>
    /// 类型查询器引导
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class BootstrapTypeFinder : IBootstrap
    {
        /// <summary>
        /// 程序集列表
        /// </summary>
        private readonly IDictionary<string, int> assemblies;

        /// <summary>
        /// 构建一个类型查询器引导
        /// </summary>
        /// <param name="assembly">需要添加的程序集</param>
        public BootstrapTypeFinder(IDictionary<string, int> assembly = null)
        {
            assemblies = new Dictionary<string, int>();
            Dict.AddRange(assemblies, Assemblys.Assembly);
            Dict.AddRange(assemblies, assembly);
        }

        /// <summary>
        /// 引导程序接口
        /// </summary>
        public void Bootstrap()
        {
            if (assemblies.Count <= 0)
            {
                return;
            }

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                int sort;
                if (!assemblies.TryGetValue(assembly.GetName().Name, out sort))
                {
                    continue;
                }

                var localAssembly = assembly;
                App.OnFindType((finder) => localAssembly.GetType(finder), sort);
            }
        }
    }
}