using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    internal sealed partial class AssetBundleEditor : EditorWindow
    {
        private enum MenuState
        {
            Normal,
            Add,
            Rename,
            Remove,
        }
    }
}
