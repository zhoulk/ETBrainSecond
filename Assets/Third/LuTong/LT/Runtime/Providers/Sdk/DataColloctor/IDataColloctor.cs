/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：数据采集接口,记录游戏时长，等级等数据
 * 
 * ------------------------------------------------------------------------------*/

namespace LT.Sdk
{
    public interface IDataColloctor
    {
        /// <summary>
        /// 统计关卡开始的时间点
        /// </summary>
        /// <param name="level"> 关卡id </param>
        void StartLevel(int level);

        /// <summary>
        /// 统计闯关成功的时间点
        /// </summary>
        /// <param name="level"> 关卡id </param>
        void FinishLevel(int level);

        /// <summary>
        /// 统计闯关失败的时间点
        /// </summary>
        /// <param name="level"> 关卡id  </param>
        void FailLevel(int level);

        /// <summary>
        /// 统计玩家等级
        /// </summary>
        /// <param name="value"> 玩家当前等级 </param>
        void SetPlayerLevel(int value);

        /// <summary>
        /// 统计进入页面的时间点
        /// </summary>
        /// <param name="pageName"> 名称 </param>
        void OnPageStart(string pageName);

        /// <summary>
        /// 统计退出页面的时间点
        /// </summary>
        /// <param name="pageName"> 名称 </param>
        void OnPageEnd(string pageName);

        /// <summary>
        /// 统计单/双人模式的开始时间点
        /// <para> 单人模式 = 1</para>
        /// <para> 双人模式 = 2</para>
        /// </summary>
        /// <param name="value"></param>
        void OnMultiplayerStart(int value);

        /// <summary>
        /// 统计单/双人模式的结束时间点
        /// </summary>
        void OnMultiplayerEnd();

        /// <summary>
        /// 道具消耗接口
        /// </summary>
        /// <param name="item"> 物品名称 </param>
        /// <param name="level"> 关卡id </param>
        /// <param name="number"> 数量 </param>
        /// <param name="goldCoin"> 花费 </param>
        void Use(string item, int level, int number, int goldCoin);

        /// <summary>
        /// 统计事件次数
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="eventID">事件id</param>
        void OnEvent(string eventName, string eventID);
    }
}