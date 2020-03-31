/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/04/28
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections;
using UnityEngine;
using LT.Fsm;
using LT.Container;
using LT.EventDispatcher;

namespace LT.Procedure
{
    /// <summary>
    /// 流程管理服务
    /// </summary>
	public class ProviderProcedure : MonoBehaviour, IServiceProvider
    {
        /// <summary>
        /// 可用的流程类型名
        /// </summary>
        [SerializeField]
        private string[] availableProcedureTypeNames;

        /// <summary>
        /// 入口流程
        /// </summary>
        [SerializeField]
        private string entranceProcedureTypeName;

        /// <summary>
        /// 当注册服务提供者
        /// </summary>
        public void Register()
        {
            App.SingletonIf<IFsmManager, FsmManager>();
            App.Singleton<IProcedureManager, ProcedureManager>();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            ProcedureBase entranceProcedure = null;
            ProcedureBase[] procedures = new ProcedureBase[availableProcedureTypeNames.Length];

            for (int i = 0; i < availableProcedureTypeNames.Length; i++)
            {
                Type procedureType = Utility.Assembly.GetType(availableProcedureTypeNames[i]);
                Guard.Requires<ArgumentException>(procedureType != null, $"Can not find procedure type '{availableProcedureTypeNames[i]}'.");

                procedures[i] = (ProcedureBase)Activator.CreateInstance(procedureType);
                Guard.Requires<ArgumentException>(procedures[i] != null, $"Can not find procedure instance '{availableProcedureTypeNames[i]}'.");

                if (entranceProcedureTypeName == availableProcedureTypeNames[i])
                {
                    entranceProcedure = procedures[i];
                }
            }

            App.Make<IProcedureManager>().Initialize(procedures);

            // 注册框架完成事件
            App.Make<IEventDispatcher>().AddListener<StartCompletedEventArgs>((args) =>
            {
                if (entranceProcedure != null)
                {
                    App.Make<IProcedureManager>().StartProcedure(entranceProcedure.GetType());
                }
            });
        }
    }
}