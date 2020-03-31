/*
 *    描述:
 *          1. Class对象池
 *
 *    开发人: 邓平
 */
using System.Collections.Generic;

namespace LtFramework.Util.Pools
{
    public class ClassObjectPool<T> where T : class, new()
    {
        protected Stack<T> _Pool = new Stack<T>();

        /// <summary>
        /// 最大对象个数, 小于等于0 表示不限个数
        /// </summary>
        protected int _MaxCount = 0;

        /// <summary>
        /// 没有回收的对象个数
        /// </summary>
        protected int _NoRecycleCount = 0;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param UIName="maxCount">最大对象个数, 小于等于0 表示不限个数</param>
        public ClassObjectPool(int maxCount)
        {
            _MaxCount = maxCount;
            for (int i = 0; i < maxCount; i++)
            {
                _Pool.Push(NewClass());
            }
        }

        private T NewClass()
        {
            var obj = new T();
            if (obj is IClassPoolNode<T>)
            {
                (obj as IClassPoolNode<T>).Pool = this;
                (obj as IClassPoolNode<T>).Recycle = this.Recycle;
            }

            return obj;
        }

        /// <summary>
        /// 从池里面取类对象
        /// </summary>
        /// <param UIName="createIfPoolEmpty">如果为空 是否New对象</param>
        /// <returns></returns>
        public T Spawn(bool createIfPoolEmpty = true)
        {
            if (_Pool.Count > 0)
            {
                T rtn = _Pool.Pop();
                if (rtn == null)
                {
                    if (createIfPoolEmpty)
                    {
                        rtn = NewClass();
                    }
                }

                _NoRecycleCount++;
                return rtn;
            }
            else
            {
                if (createIfPoolEmpty)
                {
                    T rtn = NewClass();
                    _NoRecycleCount++;
                    return rtn;
                }
            }

            return null;
        }

        /// <summary>
        /// 回收类对象
        /// </summary>
        /// <param UIName="obj"></param>
        /// <returns></returns>
        public bool Recycle(T obj)
        {
            if (obj == null)
            {
                return false;
            }

            _NoRecycleCount--;
            if (_MaxCount > 0 && _Pool.Count >= _MaxCount)
            {
                obj = null;
                return false;
            }

            if (obj is IClassPoolNode<T>)
            {
                (obj as IClassPoolNode<T>).Reset();
            }

            _Pool.Push(obj);
            return true;
        }
    }
}
