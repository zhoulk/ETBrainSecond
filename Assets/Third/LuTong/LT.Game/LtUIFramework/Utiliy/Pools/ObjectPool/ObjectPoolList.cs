/*
 *    描述:
 *          1. 链表对象池
 *                     使用字典管理对象池
 *
 *    开发人: 邓平
 */
using System.Collections.Generic;
using UnityEngine;

namespace LtFramework.Util.Pools
{
    
    public class ObjectPoolList:IPoolStructBase
    {
        private readonly Dictionary<string, ListPool> _dictPool = new Dictionary<string, ListPool>();

        private Dictionary<string ,CreatNode<PoolNodeBase>> _creatNode = new Dictionary<string, CreatNode<PoolNodeBase>>();

        private Dictionary<string, GameObject> objDic = new Dictionary<string, GameObject>();

        /// <summary>
        /// 增加节点
        /// </summary>
        /// <param UIName="node">节点对象</param>
        /// <param UIName="creatNode">节点创建方法</param>
        public void AddNode(GameObject node,CreatNode<PoolNodeBase> creatNode)
        {
            _creatNode.Add(node.name,creatNode);
        }

        public void AddNode(GameObject node)
        {
            objDic.Add(node.name, node);
            _creatNode.Add(node.name, null);
        }

        /// <summary>
        /// 增加节点
        /// </summary>
        /// <param UIName="node">节点对象名字</param>
        /// <param UIName="creatNode">节点创建方法</param>
        public void AddNode(string nodeName, CreatNode<PoolNodeBase> creatNode)
        {
            _creatNode.Add(nodeName,creatNode);
        }

        
        private ListPool GetListPool(string ObjName)
        {
            //通过 对象名 获得对应的对象池
            ListPool pool = null;        
            if (_dictPool.Count > 0 && _dictPool.ContainsKey(ObjName))
            {
                pool = _dictPool[ObjName];
                if (pool != null)
                {
                    return pool;
                }
            }

            CreatNode<PoolNodeBase> creatNode;
            _creatNode.TryGetValue(ObjName, out creatNode);
            if (creatNode == null)
            {
                ObjName = ObjName.Replace("(Clone)", "");
                _creatNode.TryGetValue(ObjName, out creatNode);
            }

            if (creatNode != null)
            {
                pool = new ListPool(creatNode, s =>  s.GetNodeCallBack() , s => s.ReleaseNodeCallBack(),RemovePoolByDic);
                _dictPool.Add(ObjName, pool);
            }
            else
            {
                creatNode = () =>
                {
                    GameObject go = Object.Instantiate(objDic[ObjName]);
                    go.name = go.name.Replace("(Clone)", "");
                    PoolNodeBase node = go.GetComponent<PoolNodeBase>();
                    if(node == null)
                    {
                        Debug.LogError("Don'Current Hvae PoolNodeBase");
                    }

                    node.nodeParent = IPoolMgrBase.Instance.transform;
 
                    
                    return go.GetComponent<PoolNodeBase>();
                };
                pool = new ListPool(creatNode, s => s.GetNodeCallBack(), s => s.ReleaseNodeCallBack(), RemovePoolByDic);
                _dictPool.Add(ObjName, pool);
                //Debug.LogError("对象没有创建方法" +ObjName);
            }

            return pool;
        }
        
        private ListPool GetListPoolNotCreat(string ObjName)
        {
            ListPool pool = null;
            ObjName = ObjName.Replace("(Clone)", "");
            if (_dictPool.Count > 0 && _dictPool.ContainsKey(ObjName))
            {
                pool = _dictPool[ObjName];
            }
            return pool;
        }

        
        public PoolNodeBase GetNode(string ObjName)
        {
            ListPool pool = GetListPool(ObjName);
            PoolNodeBase node = pool.Get();
            return node;
        }

        
        public void ReleaseNode( PoolNodeBase nodeBase,string ObjName)
        {
            ListPool pool = GetListPool(ObjName);
            pool.Release(nodeBase);
        }
        
        /// <summary>
        /// 获得对象总数
        /// 通过对象池生成的对象总数
        /// 销毁的不计数
        /// </summary>
        /// <param UIName="ObjName">对象名称</param>
        /// <returns>通过对象池创建的对象总数</returns>
        public int GetCountAll(string ObjName)
        {
            int num = 0;
            ListPool pool = GetListPoolNotCreat(ObjName);
            if (pool != null)
            {
                num = pool.countAll;
            }
            else
            {
                Debug.LogWarning("该对象池不存在："+ObjName);
            }
            return num;
        }

        /// <summary>
        /// 激活的对象
        /// 通过对象池生成的对象
        /// </summary>
        /// <param UIName="ObjName"> 对象名称 </param>
        /// <returns>当前激活的对象数</returns>
        public int GetCountActiObjve(string ObjName)
        {
            int num = 0;
            ListPool pool = GetListPoolNotCreat(ObjName);
            if (pool != null)
            {
                num = pool.countActive;
            }
            else
            {
                Debug.LogWarning("该对象池不存在："+ObjName);
            }

            return num;
        }




        /// <summary>
        /// 对象池 未激活数
        /// </summary>
        /// <param UIName="ObjName"> 对象名称 </param>
        /// <returns> 当前未激活的对象数 </returns>
        public int GetCountInactive(string ObjName)
        {
            int num = 0;
            ListPool pool = GetListPoolNotCreat(ObjName);
            if (pool != null)
            {
                num = pool.countInactive;
            }
            else
            {
                Debug.LogWarning("该对象池不存在："+ObjName);
            }
            return num;
        }
        
        /// <summary>
        /// 对象销毁调取的方法
        /// 将对象从对象池中移除
        /// 不要主动调取
        /// </summary>
        /// <param UIName="Obj"></param>
        public void ElementDestory(PoolNodeBase Obj)
        {
            ListPool pool = GetListPool(Obj.gameObject.name);
            pool.ElementDestory(Obj);
        }

        
        public bool IsExitPool(string ObjName)
        {
            return _dictPool.ContainsKey(ObjName);
        }

        public int GetPoolsCount()
        {
            return _dictPool.Count;
        }

        /// <summary>
        /// 移除对象池 返回对象池
        /// </summary>
        /// <param UIName="ObjName"> 对象名称 </param>
        /// <returns> 移除的对象池 </returns>
        public List<PoolNodeBase> RemovePool(string ObjName)
        {
            List<PoolNodeBase> List = null;
            if (_dictPool.ContainsKey(ObjName))
            {
                ListPool pool;
                _dictPool.TryGetValue(ObjName, out pool);
                List = pool.GetList;
                _dictPool.Remove(ObjName);
            }

            return List;
        }

        public void ReleaseAllActive(string ObjName)
        {
            ListPool.ReleaseAllActive(ObjName);
        }

        public List<PoolNodeBase> GetAllActiveNode(string ObjName)
        {
            return ListPool.GetAllActiveNode(ObjName);
        }

        public void ReleaseAllActive()
        {
            ListPool.ReleaseAllActive();
        }

        /// <summary>
        /// 对象池对象销毁后，没对象时 移除对象池
        /// </summary>
        /// <returns></returns>
        private bool RemovePoolByDic(string ObjName)
        {
            if (_dictPool.ContainsKey(ObjName))
            {
                return _dictPool.Remove(ObjName);
            }
            else
            {
                Debug.LogWarning("不存在该对象池："+ObjName);
            }

            return false;
        }


    }

}

