/*
 *    描述:
 *          1. 对象池管理基类
 *
 *    开发人: 邓平
 */
using System.Collections.Generic;

namespace LtFramework.Util.Pools
{ 
    
    public interface IPoolStructBase:IPoolBase
    {
        //判断是否存在
        bool IsExitPool(string objName);
        //获得对象池总数
        int GetPoolsCount();
        
        //移除对象池
        List<PoolNodeBase> RemovePool(string objName);
        

    }
}

