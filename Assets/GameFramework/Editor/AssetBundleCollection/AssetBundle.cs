﻿using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    /// <summary>
    /// 资源包。
    /// </summary>
    public sealed class AssetBundle
    {
        private readonly List<Asset> m_Assets;
        private readonly List<string> m_ResourceGroups;

        private AssetBundle(string name, string variant, AssetBundleLoadType loadType, bool packed, string[] resourceGroups)
        {
            m_Assets = new List<Asset>();
            m_ResourceGroups = new List<string>();

            Name = name;
            Variant = variant;
            Type = AssetBundleType.Unknown;
            LoadType = loadType;
            Packed = packed;

            foreach (string resourceGroup in resourceGroups)
            {
                AddResourceGroup(resourceGroup);
            }
        }

        public string Name
        {
            get;
            private set;
        }

        public string Variant
        {
            get;
            private set;
        }

        public string FullName
        {
            get
            {
                return Variant != null ? Utility.Text.Format("{0}.{1}", Name, Variant) : Name;
            }
        }

        public AssetBundleType Type
        {
            get;
            private set;
        }

        public AssetBundleLoadType LoadType
        {
            get;
            private set;
        }

        public bool Packed
        {
            get;
            private set;
        }

        public static AssetBundle Create(string name, string variant, AssetBundleLoadType loadType, bool packed, string[] resourceGroups)
        {
            return new AssetBundle(name, variant, loadType, packed, resourceGroups);
        }

        public Asset[] GetAssets()
        {
            return m_Assets.ToArray();
        }

        public void Rename(string name, string variant)
        {
            Name = name;
            Variant = variant;
        }

        public void SetLoadType(AssetBundleLoadType loadType)
        {
            LoadType = loadType;
        }

        public void SetPacked(bool packed)
        {
            Packed = packed;
        }

        public void AssignAsset(Asset asset, bool isScene)
        {
            if (asset.AssetBundle != null)
            {
                asset.AssetBundle.Unassign(asset);
            }

            Type = isScene ? AssetBundleType.Scene : AssetBundleType.Asset;
            asset.SetAssetBundle(this);
            m_Assets.Add(asset);
            m_Assets.Sort(AssetComparer);
        }

        public void Unassign(Asset asset)
        {
            asset.SetAssetBundle(null);
            m_Assets.Remove(asset);
            if (m_Assets.Count <= 0)
            {
                Type = AssetBundleType.Unknown;
            }
        }

        public string[] GetResourceGroups()
        {
            return m_ResourceGroups.ToArray();
        }

        public bool HasResourceGroup(string resourceGroup)
        {
            if (string.IsNullOrEmpty(resourceGroup))
            {
                return false;
            }

            return m_ResourceGroups.Contains(resourceGroup);
        }

        public void AddResourceGroup(string resourceGroup)
        {
            if (string.IsNullOrEmpty(resourceGroup))
            {
                return;
            }

            if (m_ResourceGroups.Contains(resourceGroup))
            {
                return;
            }

            m_ResourceGroups.Add(resourceGroup);
            m_ResourceGroups.Sort();
        }

        public bool RemoveResourceGroup(string resourceGroup)
        {
            if (string.IsNullOrEmpty(resourceGroup))
            {
                return false;
            }

            return m_ResourceGroups.Remove(resourceGroup);
        }

        public void Clear()
        {
            foreach (Asset asset in m_Assets)
            {
                asset.SetAssetBundle(null);
            }

            m_Assets.Clear();
            m_ResourceGroups.Clear();
        }

        private int AssetComparer(Asset a, Asset b)
        {
            return a.Guid.CompareTo(b.Guid);
        }
    }
}