/*
 *    描述:
 *          1.
 *
 *    开发人: 邓平
 */

using System.Collections.Generic;
using LtFramework.Util.Pools;

namespace LtFramework.ResKit
{

    /// <summary>
    /// 双向链表结构节点
    /// </summary>
    /// <typeparam UIName="T"></typeparam>
    public class DoubleLinkedListNode<T> where T : class, new()
    {
        //前一个节点
        public DoubleLinkedListNode<T> Prev = null;

        //后一个节点
        public DoubleLinkedListNode<T> Next = null;

        //当前节点
        public T Current = null;

    }

    /// <summary>
    /// 双向链表结构
    /// </summary>
    /// <typeparam UIName="T"></typeparam>
    public class DoubleLinedList<T> where T : class, new()
    {
        //表头
        public DoubleLinkedListNode<T> Head = null;

        //表尾
        public DoubleLinkedListNode<T> Tail = null;

        //双向链表结构类对象池
        protected ClassObjectPool<DoubleLinkedListNode<T>> _DoubleLinkNodePool =
            ObjManager.Instance.GetOrCreateClassPool<DoubleLinkedListNode<T>>(500);

        //个数
        protected int _Count = 0;

        public int Count => _Count;

        /// <summary>
        /// 添加一个节点到头部
        /// </summary>
        /// <param UIName="t"></param>
        /// <returns></returns>
        public DoubleLinkedListNode<T> AddToHeader(T t)
        {
            

            DoubleLinkedListNode<T> pList = _DoubleLinkNodePool.Spawn(true);
            pList.Next = null;
            pList.Prev = null;
            pList.Current = t;
            return AddToHeader(pList);
        }

        /// <summary>
        /// 添加一个节点到头部
        /// </summary>
        /// <param UIName="pNode"></param>
        /// <returns></returns>
        public DoubleLinkedListNode<T> AddToHeader(DoubleLinkedListNode<T> pNode)
        {
            if (pNode == null) return null;

            pNode.Prev = null;
            if (Head == null)
            {
                //头部为空 ,链表没有东西, 添加第一个元素 头部等于尾部
                Head = Tail = pNode;
            }
            else
            {
                pNode.Next = Head;
                Head.Prev = pNode;
                Head = pNode;
            }

            _Count++;
            return Head;
        }

        /// <summary>
        /// 添加节点到尾部
        /// </summary>
        /// <param UIName="t"></param>
        /// <returns></returns>
        public DoubleLinkedListNode<T> AddToTail(T t)
        {
            DoubleLinkedListNode<T> pList = _DoubleLinkNodePool.Spawn(true);
            pList.Next = null;
            pList.Prev = null;
            pList.Current = t;
            return AddToTail(pList);
        }

        /// <summary>
        /// 添加节点到尾部
        /// </summary>
        /// <param UIName="pNode"></param>
        /// <returns></returns>
        public DoubleLinkedListNode<T> AddToTail(DoubleLinkedListNode<T> pNode)
        {
            if (pNode == null)
            {
                return null;
            }

            pNode.Next = null;
            if (Tail == null)
            {
                //尾部为空 ,链表没有东西, 添加第一个元素 头部等于尾部
                Head = Tail = pNode;
            }
            else
            {
                pNode.Prev = Tail;
                Tail.Next = pNode;
                Tail = pNode;
            }

            _Count++;
            return Tail;
        }

        /// <summary>
        /// 移除某个节点
        /// </summary>
        /// <param UIName="pNode"></param>
        public void RemoveNode(DoubleLinkedListNode<T> pNode)
        {
            if (pNode == null)
            {
                return;
            }

            if (pNode == Head)
            {
                Head = pNode.Next;
            }

            if (pNode == Tail)
            {
                Tail = pNode.Prev;
            }

            if (pNode.Prev != null)
            {
                //移除中间
                pNode.Prev.Next = pNode.Next;
            }

            if (pNode.Next != null)
            {
                pNode.Next.Prev = pNode.Prev;
            }

            pNode.Next = pNode.Prev = null;
            pNode.Current = null;
            _DoubleLinkNodePool.Recycle(pNode);
            _Count--;
        }

        /// <summary>
        /// 把某个节点移动到头部
        /// </summary>
        /// <param UIName="pNode"></param>
        public void MoveToHead(DoubleLinkedListNode<T> pNode)
        {
            if (pNode == null || pNode == Head)
            {
                return;
            }

            if (pNode.Prev == null && pNode.Next == null)
            {
                return;
            }

            if (pNode == Tail)
            {
                Tail = pNode.Prev;
            }

            if (pNode.Prev != null)
            {
                pNode.Prev.Next = pNode.Next;
            }

            if (pNode.Next != null)
            {
                pNode.Next.Prev = pNode.Prev;
            }

            pNode.Prev = null;
            pNode.Next = Head;
            Head.Prev = pNode;
            Head = pNode;

            //如果只有两个节点
            if (Tail == null)
            {

                Tail = Head;
            }
        }
    }

}
