/*
 *    描述:
 *          1.
 *
 *    开发人: 邓平
 */
using System.Collections.Generic;

namespace LtFramework.ResKit
{

    public class CMapList<T> where T : class, new()
    {
        private readonly DoubleLinedList<T> _Doublelink = new DoubleLinedList<T>();

        private readonly Dictionary<T, DoubleLinkedListNode<T>> _FindMap = new Dictionary<T, DoubleLinkedListNode<T>>();

        ~CMapList()
        {
            Clear();
        }

        /// <summary>
        /// 清除列表
        /// </summary>
        public void Clear()
        {
            while (_Doublelink.Tail != null)
            {
                Remove(_Doublelink.Tail.Current);
            }

            _FindMap.Clear();
        }

        /// <summary>
        /// 插入一个节点到表头
        /// </summary>
        /// <param UIName="t"></param>
        public void InsertToHead(T t)
        {
            DoubleLinkedListNode<T> node = null;
            if (_FindMap.TryGetValue(t, out node) && node != null)
            {
                _Doublelink.AddToHeader(node);
                return;
            }

            _Doublelink.AddToHeader(t);
            _FindMap.Add(t, _Doublelink.Head);
        }

        /// <summary>
        /// 从表尾弹出一个节点
        /// </summary>
        public void Pop()
        {
            if (_Doublelink.Tail != null)
            {
                Remove(_Doublelink.Tail.Current);
            }
        }

        /// <summary>
        /// 删除某个节点
        /// </summary>
        /// <param UIName="t"></param>
        public void Remove(T t)
        {
            DoubleLinkedListNode<T> node = null;
            if (!_FindMap.TryGetValue(t, out node) || node == null)
            {
                return;
            }

            _Doublelink.RemoveNode(node);
            _FindMap.Remove(t);
        }

        /// <summary>
        /// 获取到尾部节点
        /// </summary>
        /// <returns></returns>
        public T Back()
        {
            return _Doublelink.Tail == null ? null : _Doublelink.Tail.Current;
        }

        /// <summary>
        /// 返回节点个数
        /// </summary>
        /// <returns></returns>
        public int Size()
        {
            return _FindMap.Count;
        }

        /// <summary>
        /// 查找是否存在该节点
        /// </summary>
        /// <param UIName="t"></param>
        /// <returns></returns>
        public bool Find(T t)
        {
            DoubleLinkedListNode<T> node = null;
            if (!_FindMap.TryGetValue(t, out node) || node == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 把节点移动到头部
        /// </summary>
        /// <param UIName="t"></param>
        /// <returns></returns>
        public bool Reflesh(T t)
        {
            DoubleLinkedListNode<T> node = null;
            if (!_FindMap.TryGetValue(t, out node) || node == null)
            {
                return false;
            }

            _Doublelink.MoveToHead(node);
            return true;
        }
    }

}
