/*
 *    描述:
 *          1. 链表对象池
 *                     对象池使用链表来实现
 *
 *    开发人: 邓平
 */
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace LtFramework.Util.Pools
{
    public delegate bool RemovePoolByDic(string ObjName);

    internal class ListPool : IPoolBase
    {
        private List<PoolNodeBase> _List = new List<PoolNodeBase>();

        public static Dictionary<string, List<PoolNodeBase>> _activeNode = new Dictionary<string, List<PoolNodeBase>>();


        public List<PoolNodeBase> GetList
        {
            get { return _List; }
        }


        //获得一个对象池 回调
        private readonly UnityAction<PoolNodeBase> _ActionOnGet;

        //释放一个对象池 回调
        private readonly UnityAction<PoolNodeBase> _ActionOnRelease;

        //对象销毁
        private readonly UnityAction<PoolNodeBase> _ActionOnDestory;

        private readonly UnityAction _DestoryPoolCallBack;

        private readonly RemovePoolByDic _RemovePoolByDic;

        //创建Node
        private CreatNode<PoolNodeBase> _creatNode;



        //对象池总数
        public int countAll { get; private set; }

        //对象池 激活数
        public int countActive
        {
            get { return countAll - countInactive; }
        }

        //对象池 未激活数
        public int countInactive
        {
            get { return _List.Count; }
        }


        internal ListPool(CreatNode<PoolNodeBase> creatNode, UnityAction<PoolNodeBase> actionOnGet,
            UnityAction<PoolNodeBase> actionOnRelease, RemovePoolByDic removePoolByDic)
        {
            _creatNode = creatNode;
            _ActionOnGet = actionOnGet;
            _ActionOnRelease = actionOnRelease;
            _RemovePoolByDic = removePoolByDic;
        }


        public PoolNodeBase GetNode(string ObjName = null)
        {
            PoolNodeBase node = Get();
            return node;
        }

        internal PoolNodeBase Get()
        {
            PoolNodeBase element;
            if (_List.Count == 0)
            {
                //根据需求 创建Item
                element = _creatNode();
                element.InitNode(this);
                countAll++;
            }
            else
            {
                element = _List[_List.Count - 1];
                _List.RemoveAt(_List.Count - 1);
            }

            if (element.nodeParent != null)
            {
                element.transform.SetParent(element.nodeParent);
            }

            if (_ActionOnGet != null)
                _ActionOnGet(element);

            return element;
        }

        public static void AddActiveNode(string ObjName, PoolNodeBase node)
        {
            if (node != null)
            {
                if (!_activeNode.ContainsKey(ObjName))
                {
                    List<PoolNodeBase> list = new List<PoolNodeBase>();
                    _activeNode.Add(ObjName, list);
                }

                _activeNode[ObjName].Add(node);
            }
        }

        public static void ReduceActiveNode(PoolNodeBase node, string ObjName)
        {
            if (_activeNode.ContainsKey(ObjName) && _activeNode[ObjName].Contains(node))
            {
                _activeNode[ObjName].Remove(node);
            }
        }

        public static List<PoolNodeBase> GetAllActiveNode(string ObjName)
        {
            List<PoolNodeBase> list;
            _activeNode.TryGetValue(ObjName, out list);
            return list;
        }

        public static List<PoolNodeBase> GetAllActiveNode()
        {
            List<PoolNodeBase> list = new List<PoolNodeBase>();
            foreach (List<PoolNodeBase> activeNodeValue in _activeNode.Values)
            {
                list.AddRange(activeNodeValue);
            }

            return list;
        }

        public static void ReleaseAllActive(string ObjName)
        {
            List<PoolNodeBase> list = GetAllActiveNode(ObjName);
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].ReleaseNode();
                }
            }
        }

        public static void ReleaseAllActive()
        {
            List<PoolNodeBase> list = GetAllActiveNode();
            if (list != null && list.Count > 0)
            {
                foreach (PoolNodeBase node in list)
                {
                    node.ReleaseNode();
                }
            }
        }


        public void ReleaseNode(PoolNodeBase nodeBase, string ObjName)
        {
            Release(nodeBase);

            ReduceActiveNode(nodeBase, ObjName);
        }

        internal void Release(PoolNodeBase element)
        {
            if (_List.Count > 0 && _List.Contains(element))
                Debug.LogWarning("对象池释放错误。试图释放一个已经释放的对象池" + element.name);
            if (_ActionOnRelease != null)
                _ActionOnRelease(element);
            _List.Add(element);
        }


        void IPoolBase.ElementDestory(PoolNodeBase element)
        {
            ElementDestory(element);
            ReduceActiveNode(element, element.name);
        }

        internal void ElementDestory(PoolNodeBase element)
        {
            //判断栈中是否存在 如果存在 则移除
            string name = element.gameObject.name;
            if (_List.Contains(element))
            {
                _List.Remove(element);

            }

            countAll--;
            if (countAll <= 0)
            {
                RemovePoolListByDic(name);
            }
        }

        internal virtual bool RemovePoolListByDic(string ObjName)
        {
            return _RemovePoolByDic(ObjName);
        }

    }


}
