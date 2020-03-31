/*
 *    描述:
 *          1. AssetBundle管理类
 *
 *    开发人: 邓平
 */
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using LtFramework.Util;
using LtFramework.Util.Pools;
using LtFramework.Util.Tools;


namespace LtFramework.ResKit
{
    public class AssetBundleMgr : LtFramework.Util.Singleton<AssetBundleMgr>
    {
        private string _AssetBundlePath = string.Empty;

        public string AssetBundlePath
        {
            get
            {
                if (string.IsNullOrEmpty(_AssetBundlePath))
                {
#if UNITY_EDITOR
                    _AssetBundlePath = Application.dataPath + "/../AssetBundle/" +
                                       UnityEditor.EditorUserBuildSettings.activeBuildTarget;

#else
                    _AssetBundlePath = ResKitConst.AssetBundlePath;
#endif
                }

                return _AssetBundlePath;
            }
        }

        private AssetBundleMgr()
        {

        }

        //资源关系依赖配置表   key -Crc  value -ResItem
        protected Dictionary<uint, ResItem> _ResouceItemDic = new Dictionary<uint, ResItem>();

        //存储已加载的AB包 key - Crc 
        protected Dictionary<uint, AssetBundleItem> _AssetBundleCacheDic = new Dictionary<uint, AssetBundleItem>();

        //AssetBundleItem类对象池
        protected ClassObjectPool<AssetBundleItem> _AssetBundleItemPool =
            ObjManager.Instance.GetOrCreateClassPool<AssetBundleItem>(500);

        /// <summary>
        /// 加载AB配置表
        /// </summary>
        /// <returns></returns>
        public void LoadAssetBundleConfig()
        {
            _ResouceItemDic.Clear();

            string configPath = AssetBundlePath + "/assetbundleconfig";
#if UNITY_EDITOR
            if (!File.Exists(configPath))
            {
                Debug.LogError("assetbundleconfig 文件不存在, 没有进行打包 path " + configPath);
            }
#endif
            AssetBundle configAB = AssetBundle.LoadFromFile(configPath);
            TextAsset textAsset = configAB.LoadAsset<TextAsset>("AssetBundleConfig.bytes");

            AssetBundleConfig config;
            using (MemoryStream ms = new MemoryStream(textAsset.bytes))
            {
                BinaryFormatter bf = new BinaryFormatter();
                config = (AssetBundleConfig) bf.Deserialize(ms);
            }

            for (int i = 0; i < config.ABList.Count; i++)
            {
                ABBase abBase = config.ABList[i];
                ResItem item = new ResItem();
                item.Crc = abBase.Crc;
                item.AssetName = abBase.AssetName;
                item.ABName = abBase.ABName;
                item.DependAssetBundle = abBase.ABDependence;

                if (_ResouceItemDic.ContainsKey(item.Crc))
                {
                    Debug.LogError("重复的Crc  资源名:" + item.AssetName + "ab包名:" + item.ABName);
                }
                else
                {
                    _ResouceItemDic.Add(item.Crc, item);
                }
            }
        }


        public ResItem LoadResourceAssetBundle(uint crc)
        {
            ResItem item = null;
            if (!_ResouceItemDic.TryGetValue(crc, out item) || item == null)
            {
                return item;
            }

            if (item.AssetBundle != null)
            {
                return item;
            }

            item.AssetBundle = LoadAssetBundle(item.ABName);

            if (item.DependAssetBundle != null)
            {
                for (int i = 0; i < item.DependAssetBundle.Count; i++)
                {
                    LoadAssetBundle(item.DependAssetBundle[i]);
                }
            }

            return item;
        }


        /// <summary>
        /// 根据名字 加载单个AssetBundle
        /// </summary>
        /// <param UIName="name"></param>
        /// <returns></returns>
        private AssetBundle LoadAssetBundle(string name)
        {
            AssetBundleItem item = null;
            uint crc = Crc32.GetCrc32(name);
            if (!_AssetBundleCacheDic.TryGetValue(crc, out item))
            {
                AssetBundle assetBundle = null;
                string fullPath = AssetBundlePath + "/" + name;
                if (File.Exists(fullPath))
                {
                    assetBundle = AssetBundle.LoadFromFile(fullPath);
                }

                if (assetBundle == null)
                {
                    Debug.LogError("Load AsetBundle Error: " + fullPath);
                }

                item = _AssetBundleItemPool.Spawn(true);
                item.assetBundle = assetBundle;
                item.RefCount++;
                _AssetBundleCacheDic.Add(crc, item);
            }
            else
            {
                item.RefCount++;
            }

            return item.assetBundle;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param UIName="item"></param>
        public void ReleaseAsset(ResItem item)
        {
            if (item == null)
            {
                return;
            }

            if (item.DependAssetBundle != null && item.DependAssetBundle.Count > 0)
            {
                for (int i = 0; i < item.DependAssetBundle.Count; i++)
                {
                    UnLoadAssetBundle(item.DependAssetBundle[i]);
                }
            }

            UnLoadAssetBundle(item.ABName);

        }

        private void UnLoadAssetBundle(string name)
        {
            AssetBundleItem item = null;
            uint crc = Crc32.GetCrc32(name);
            if (_AssetBundleCacheDic.TryGetValue(crc, out item) && item != null)
            {
                item.RefCount--;
                if (item.RefCount <= 0 && item.assetBundle != null)
                {
                    item.assetBundle.Unload(true);
                    item.Rest();
                    _AssetBundleItemPool.Recycle(item);
                    _AssetBundleCacheDic.Remove(crc);
                }
            }
        }

        /// <summary>
        /// 根据crc查找ResourceItem
        /// </summary>
        /// <param UIName="crc"></param>
        /// <returns></returns>
        public ResItem FindResourceItem(uint crc)
        {
            ResItem item = null;
            _ResouceItemDic.TryGetValue(crc, out item);
            return item;
        }

    }

    public class AssetBundleItem
    {
        public AssetBundle assetBundle = null;
        public int RefCount = 0;

        public void Rest()
        {
            assetBundle = null;
            RefCount = 0;
        }
    }

}
