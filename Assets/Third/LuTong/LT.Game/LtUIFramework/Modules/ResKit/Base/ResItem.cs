/*
 *    描述:
 *          1.
 *
 *    开发人: 邓平
 */

using System;
using System.Collections.Generic;
using LtFramework.Util.Pools;
using LtFramework.Util.Tools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LtFramework.ResKit
{
    public class ResItem : IClassPoolNode<ResItem>
    {
        public ClassObjectPool<ResItem> Pool { get; set; }

        public void Reset()
        {
            Guid = 0;
            Obj = null;
            Crc = 0;
            AssetName = string.Empty;

            ABName = string.Empty;
            AssetBundle = null;
            DependAssetBundle = null;
            LastUseTime = 0.0f;
            (RefCount as RefCounter).Reset();
            Clear = true;
        }

        public Func<ResItem, bool> Recycle { get; set; }

        //资源唯一标识
        public int Guid = 0;

        //资源对象
        public Object Obj = null;

        //资源路径的Crc
        public uint Crc = 0;

        //资源的文件名
        public string AssetName = string.Empty;

        //资源所在的AssetBundle名字
        public string ABName = string.Empty;

        //该资源加载完的AB包
        public AssetBundle AssetBundle = null;

        //资源所依赖的AssetBundle
        public List<string> DependAssetBundle = null;

        //记录最后使用时间
        public float LastUseTime = 0.0f;

        //引用计数
        public IRefCounter RefCount { get; }

        //跳场景是否清掉
        public bool Clear = true;

        public ResItem()
        {
            RefCount = new RefCounter(OnZeroRef);
        }

        private void OnZeroRef()
        {
            LastUseTime = Time.realtimeSinceStartup;
        }
    }
}
