/*
 *
 *    描述:
 *          1. 对象池基类
 *              3个基类方法 和 一个节点创建委托
 *
 *    开发人: 邓平
 */
namespace LtFramework.Util.Pools
{
    //创建节点委托事件
    public delegate T CreatNode<out T>();

    public interface IPoolBase
    {
        //获得节点
        PoolNodeBase GetNode(string objName = null);

        //释放节点
        void ReleaseNode(PoolNodeBase nodeBase, string objName = null);

        //节点销毁
        void ElementDestory(PoolNodeBase element);
    }
}

