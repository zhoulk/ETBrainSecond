/*
 *    描述:
 *          1. 对象池 管理基类
 *
 *    开发人: 邓平
 */
using System.Collections.Generic;
using UnityEngine;

namespace LtFramework.Util.Pools
{
    public abstract class IPoolMgrBase : MonoBehaviourEx
    {
        /// <summary>
        /// 对象池单例
        /// </summary>
        public static IPoolMgrBase Instance;
        public virtual void Awake()
        {
            Instance = this;
        }

        protected override void OnDestroyMono()
        {
            base.OnDestroyMono();
            Instance = null;
        }


        //1.创建对象池
        private ObjectPoolList _pools = new ObjectPoolList();

        /// <summary>
        /// 获得当前对象池
        /// </summary>
        public ObjectPoolList pools
        {
            get { return _pools; }
        }


        #region 预加载

        private List<string> _preLoadList;
        private List<PoolNodeBase> _preLoadNode;
        private int _preLoadIndex;

        /// <summary>
        /// 设置预加载对象
        /// </summary>
        /// <param UIName="dic">预加载对象  路径 个数</param>
        public void SetPreLoad(Dictionary<string, int> dic)
        {
            _preLoadList = DicToList(dic);
            _preLoadNode = new List<PoolNodeBase>();
            _preLoadIndex = 0;
        }

        /// <summary>
        /// 将字典元素存储到链表中
        /// </summary>
        /// <param UIName="dic"> 路径 个数 </param>
        /// <returns></returns>
        private List<string> DicToList(Dictionary<string, int> dic)
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, int> pair in dic)
            {
                for (int i = 0; i < pair.Value; i++)
                {
                    list.Add(pair.Key);
                }
            }

            return list;
        }

        /// <summary>
        /// 得到预加载进度
        /// </summary>
        public float GetPreLoadProgress
        {
            get
            {
                if (_preLoadList == null || _preLoadList.Count < 1) return 0;

                return (float) _preLoadIndex / _preLoadList.Count;
            }
        }

        /// <summary>
        /// Update中 对象预加载
        /// </summary>
        private void UpdatePreLoad()
        {
            if (_preLoadList == null || _preLoadIndex == _preLoadList.Count) return;

            for (int i = _preLoadIndex; i < _preLoadList.Count;)
            {
                PoolNodeBase node = _pools.GetNode(_preLoadList[i]);
                _preLoadNode.Add(node);
                node.gameObject.SetActive(false);
                _preLoadIndex++;
                break;
            }

            if (_preLoadIndex == _preLoadList.Count)
            {
                foreach (PoolNodeBase node in _preLoadNode)
                {
                    node.ReleaseNode();
                }
            }
        }

        protected virtual void Update()
        {
            UpdatePreLoad();
        }

        #endregion

        /// <summary>
        /// 释放所有激活对象
        /// </summary>
        public void ReleaseAllActive()
        {
            _pools.ReleaseAllActive();
        }

        /// <summary>
        /// 释放指定名字的所有激活对象
        /// </summary>
        /// <param UIName="nodeName">对象名称</param>
        public void ReleaseAllActive(string nodeName)
        {
            _pools.ReleaseAllActive(nodeName);
        }

        
    }
}
