/*
 *    描述:
 *          1. 对象节点Base类
 *                所有对象池节点都要继承该类
 *                重写  下面两个方法
 *                    GetNodeCallBack
 *                    ReleaseNodeCallBack
 *
 *    开发人: 邓平
 */
using System.Collections;
using UnityEngine;

namespace LtFramework.Util.Pools
{
    public class PoolNodeBase : MonoBehaviourEx, IPoolNode
    {
        protected GameObject node;
        protected Transform parent = null;
        private IPoolBase _objectPool;//所属对象池
        private bool isDestory = false;

        public Transform nodeParent
        {
            get { return parent; }
            set { parent = value; }
        }

        public IPoolBase objectPool
        {
            get { return _objectPool; }
        }

        public void InitNode(IPoolBase pool)
        {
            _objectPool = pool;
            node = this.gameObject;
        }

        protected override void OnDestroyMono()
        {
            base.OnDestroyMono();
            DestoryNodeCallBack();
        }

        #region 释放

        /// <summary>
        /// 释放 对象
        /// </summary>
        /// <param UIName="time">time > 0延迟释放时间  ,time <=0 立即释放</param>
        public void ReleaseNode(float time = -1)
        {
            //为了防止延迟销毁带来的错误
            if(isDestory) return;

            if (parent != null)
            {
                gameObject.transform.SetParent(parent);
            }

            if (time <= 0)
            {
                timer = -1;
                if (objectPool != null)
                {
                    objectPool.ReleaseNode(this, gameObject.name);
                }
                else
                {
                    Debug.LogWarning("该对象没有对象池管理 objectPool == null  :"+this.GetType());
                    Destroy(this.gameObject);
                }

            }
            else
            {
                timer = time;
            }
        }

        public void DestoryNode(float time = 0)
        {
            Destroy(this.gameObject, time);
        }

        private float timer = 0;

        protected virtual void LateUpdate()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    objectPool.ReleaseNode( this,gameObject.name);
                }
            }
        }

        #endregion

        #region 回调函数
        
        public virtual void GetNodeCallBack()
        {
            Debug.Log("获得Node 回调");
        }

        public virtual void ReleaseNodeCallBack()
        {
            Debug.Log("释放Node  回调");
        }

        public void DestoryNodeCallBack()
        {
            isDestory = true;
            if (objectPool != null)
                objectPool.ElementDestory(this);
        }
        
        #endregion

    }
}
