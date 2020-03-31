/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/12/11
 * 模块描述：数据采集实现
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;
using System;

namespace LT.Sdk
{
    /// <summary>
    /// 数据采集实现
    /// </summary>
    internal class DataColloctor : IDataColloctor
    {
        private AndroidJavaClass m_BridgeClass;

        /// <summary>
        /// 构造数据采集服务
        /// </summary>
        public DataColloctor(Unity2AndroidHelper unity2Android)
        {
            m_BridgeClass = unity2Android.BridgeClass;
        }

        /// <summary>
        /// 统计关卡开始的时间点
        /// </summary>
        /// <param name="level"> 关卡id </param>
        public void StartLevel(int level)
        {
            if (m_BridgeClass == null) return;

            m_BridgeClass.CallStatic("startLevel", level);
        }

        /// <summary>
        /// 统计闯关成功的时间点
        /// </summary>
        /// <param name="level"> 关卡id </param>
        public void FinishLevel(int level)
        {
            if (m_BridgeClass == null) return;

            m_BridgeClass.CallStatic("finishLevel", level);
        }

        /// <summary>
        /// 统计闯关失败的时间点
        /// </summary>
        /// <param name="level"> 关卡id  </param>
        public void FailLevel(int level)
        {
            if (m_BridgeClass == null) return;

            m_BridgeClass.CallStatic("failLevel", level);
        }

        /// <summary>
        /// 统计玩家等级
        /// </summary>
        /// <param name="value"> 玩家当前等级 </param>
        public void SetPlayerLevel(int value)
        {
            if (m_BridgeClass == null) return;

            m_BridgeClass.CallStatic("setPlayerLevel", value);
        }

        /// <summary>
        /// 统计进入页面的时间点
        /// </summary>
        /// <param name="pageName"> 名称 </param>
        public void OnPageStart(string pageName)
        {
            if (m_BridgeClass == null) return;

            m_BridgeClass.CallStatic("onPageStart", pageName);
        }

        /// <summary>
        /// 统计退出页面的时间点
        /// </summary>
        /// <param name="pageName"> 名称 </param>
        public void OnPageEnd(string pageName)
        {
            if (m_BridgeClass == null) return;

            m_BridgeClass.CallStatic("onPageEnd", pageName);
        }

        /// <summary>
        /// 统计单/双人模式的开始时间点
        /// </summary>
        /// <param name="value"> 单人模式1，双人模式 2</param>
        public int hid = 1;
        public void OnMultiplayerStart(int value)
        {
            if (m_BridgeClass == null) return;

            hid = value;
           
            m_BridgeClass.CallStatic("onMultiplayerStart", "", value);
        }

        /// <summary>
        /// 统计单/双人模式的结束时间点
        /// </summary>
        public void OnMultiplayerEnd()
        {
            if (m_BridgeClass == null) return;

            m_BridgeClass.CallStatic("onMultiplayerEnd", "", hid);
        }

        /// <summary>
        /// 道具消耗接口
        /// </summary>
        /// <param name="item"> 物品名称 </param>
        /// <param name="level"> 关卡id </param>
        /// <param name="number"> 数量 </param>
        /// <param name="goldCoin"> 花费 </param>
        public void Use(string item, int level, int number, int goldCoin)
        {
            if (m_BridgeClass == null) return;

            m_BridgeClass.CallStatic("use", item, level, number, goldCoin);
        }

        /// <summary>
        /// 统计事件次数
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="eventID">事件id</param>
        public void OnEvent(string eventName, string eventID)
        {
            if (m_BridgeClass == null) return;

            m_BridgeClass.CallStatic("onEvent", eventName, eventID);
        }
    }
}
