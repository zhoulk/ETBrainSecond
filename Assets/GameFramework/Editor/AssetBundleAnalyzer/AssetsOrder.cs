using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    public enum AssetsOrder
    {
        AssetNameAsc,
        AssetNameDesc,
        DependencyAssetBundleCountAsc,
        DependencyAssetBundleCountDesc,
        DependencyAssetCountAsc,
        DependencyAssetCountDesc,
        ScatteredDependencyAssetCountAsc,
        ScatteredDependencyAssetCountDesc,
    }
}
