/*
 *    描述:
 *          1.
 *
 *    开发人: 邓平
 */

using System;
using System.Collections.Generic;
using LtFramework.Util.Pools;
using UnityEngine;

namespace LtFramework.ResKit
{
    public class ResouceObj: IClassPoolNode<ResouceObj>
    {
        //路径对应的crc
        public uint Crc = 0;

        //资源路径
        public string Path = string.Empty;
        //未实例化资源
        public ResItem ResItem = null;

        //实例化处理的Object
        public GameObject CloneObj = null;

        //跳场景是否清除
        public bool ClearByChangeScene = true;

        //存储GUID
        public int Guid = 0;

        //是否已经放回对象池
        public bool AlreadyRelease = false;

        //-------------------------//
        //是否放到场景节点下
        public bool SetParent = false;

        //实例化资源加载完成后的回调
        public OnAsyncObjFinish OnFinish = null;

        //异步参数
        public readonly List<object> ParamList = new List<object>();


        public object[] ParamValues
        {
            get { return ParamList.ToArray(); }
        }

        public ClassObjectPool<ResouceObj> Pool { get; set; }

        public Func<ResouceObj, bool> Recycle { get; set; }

        public void Reset()
        {
            Crc = 0;
            ResItem = null;
            CloneObj = null;
            ClearByChangeScene = true;
            Guid = 0;
            AlreadyRelease = false;
            SetParent = false;
            OnFinish = null;
            ParamList.Clear();
        }

    }

}
