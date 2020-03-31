/*
 *    描述:
 *          1. AssetBundle配置类
 *
 *    开发人: 邓平
 */
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ABConfig : ScriptableObject
{
    //单个文件所在的文件夹路径,会遍历这个文件夹下面所有prefab,文件名字不能重复,必须保证名字唯一
    public List<string> AllPrefabPathAB;
    /// <summary>
    /// 所有单文件ABPath
    /// </summary>
    public List<ABConfigItem> AllAssetAB;
    /// <summary>
    /// 所有文件夹ABPath
    /// </summary>
    public List<ABConfigItem> AllFileDirAB;

    [System.Serializable]
    public struct ABConfigItem
    {
        public string ABName;
        public string Path;
    }

    public ABConfig()
    {
        AllPrefabPathAB = new List<string>();
        AllAssetAB = new List<ABConfigItem>();
        AllFileDirAB = new List<ABConfigItem>();
    }

    public void Init(ABConfig abConfig)
    {
        AllPrefabPathAB = new List<string>(abConfig.AllPrefabPathAB);
        AllAssetAB = new List<ABConfigItem>(abConfig.AllAssetAB);
        AllFileDirAB = new List<ABConfigItem>(abConfig.AllFileDirAB);
    }


    public override string ToString()
    {
        StringBuilder str = new StringBuilder();

        foreach (string path in AllPrefabPathAB)
        {
            str.AppendFormat("Path : {0}\n", path);
        }

        foreach (ABConfigItem item in AllAssetAB)
        {
            str.AppendFormat("Name : {0} --> Path : {1}\n", item.ABName, item.Path);
        }

        foreach (ABConfigItem item in AllFileDirAB)
        {
            str.AppendFormat("Name : {0} --> Path : {1}\n", item.ABName, item.Path);
        }

        return str.ToString();
    }
}
