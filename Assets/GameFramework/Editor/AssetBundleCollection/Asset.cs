using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace UnityGameFramework.Editor.AssetBundleTools
{
    /// <summary>
    /// 资源。
    /// </summary>
    public sealed class Asset
    {
        private Asset(string guid, AssetBundle assetBundle)
        {
            Guid = guid;
            AssetBundle = assetBundle;
        }

        public string Guid
        {
            get;
            private set;
        }

        public string Name
        {
            get
            {
                return AssetDatabase.GUIDToAssetPath(Guid);
            }
        }

        public AssetBundle AssetBundle
        {
            get;
            private set;
        }

        public static Asset Create(string guid)
        {
            return new Asset(guid, null);
        }

        public static Asset Create(string guid, AssetBundle assetBundle)
        {
            return new Asset(guid, assetBundle);
        }

        public void SetAssetBundle(AssetBundle assetBundle)
        {
            AssetBundle = assetBundle;
        }
    }
}
