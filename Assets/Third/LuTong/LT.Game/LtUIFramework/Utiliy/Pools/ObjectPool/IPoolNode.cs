/*
 *    描述:
 *          1. 对象池节点接口
 *
 *    开发人: 邓平
 */
namespace LtFramework.Util.Pools
{

    internal interface IPoolNode
    {
        void GetNodeCallBack(); //获得Node 回调

        /// <summary>
        /// 释放Node
        /// </summary>
        /// <param UIName="time"> time大于0延迟释放时间  time小于等于0 立即释放 </param>
        void ReleaseNode(float time);

        void ReleaseNodeCallBack(); //释放Node  回调

    }

}

