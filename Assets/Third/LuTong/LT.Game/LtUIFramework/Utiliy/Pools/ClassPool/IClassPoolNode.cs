/*
 *    描述:
 *          1. Class对象池对象接口
 *
 *    开发人: 邓平
 */
using System;

namespace LtFramework.Util.Pools
{
    public interface IClassPoolNode<T> where T : class, new()
    {
        /// <summary>
        /// 所属对象池
        /// </summary>
        ClassObjectPool<T> Pool { get; set; }

        /// <summary>
        /// 重置方法
        /// </summary>
        void Reset();

        /// <summary>
        /// 回收
        /// </summary>
        Func<T,bool> Recycle { get; set; }
    }
}
