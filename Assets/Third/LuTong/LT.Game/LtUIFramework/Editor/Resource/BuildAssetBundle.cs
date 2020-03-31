/*
 *    描述:
 *          1. AssetBundle 打包
 *
 *    开发人: 邓平
 */
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using LtFramework.ResKit;
using LtFramework.Util.Tools;
using UnityEngine;
using UnityEditor;

namespace LtFramework.Editor
{

    public class BuildAssetBundle
    {
        //AssetBundle配置文件路径
        private const string ABConfigPath = "Assets/Resources/";
        //AssetBundle二进制配置文件文件路径
        private const string ABBytePath = "Assets/ABConfig/";
        //AssetBundle二进制配置文件名
        private const string ABByteName = ABBytePath + "AssetBundleConfig.bytes";

        //AssetBundle打包路径
        private static string BundleTargetPath = Application.dataPath + "/../AssetBundle/" + EditorUserBuildSettings.activeBuildTarget.ToString();


        /// <summary>
        /// key是ab包名,value是路径,所有文件夹ab包dic
        /// </summary>
        private static readonly Dictionary<string, string> _AllFileDir = new Dictionary<string, string>();

        /// <summary>
        /// 所有单个资源
        /// </summary>
        private static readonly Dictionary<string,string> _AllAssetsAB = new Dictionary<string, string>();

        //单个Prefab的ab包
        private static readonly Dictionary<string, List<string>> _AllPrefabDir = new Dictionary<string, List<string>>();

        //过滤的list
        private static readonly List<string> _AllFilterAB = new List<string>();

        //存储所有有效路径
        private static readonly List<string> _ConfigFile = new List<string>();

        #region 配置

        private static ABConfig abConfig = null;

        [MenuItem("Assets/AssetBundle/Add ABAsset")]
        private static void AddResAsset()
        {
            if (!Directory.Exists(ABConfigPath))
            {
                Directory.CreateDirectory(ABConfigPath);
            }
            string fileName = ABConfigPath + typeof(ABConfig).Name + ".asset";

            var abConfigOld = Resources.Load<ABConfig>(typeof(ABConfig).Name);
            if (abConfig == null)
            {
                abConfig = ScriptableObject.CreateInstance<ABConfig>();
            }
            else
            {

                abConfig = ScriptableObject.CreateInstance<ABConfig>();
                abConfig.Init(abConfigOld);
            }

            Dictionary<string ,int> abConfigDic = new Dictionary<string, int>();
            for (int i = 0; i < abConfig.AllAssetAB.Count; i++)
            {
                abConfigDic.Add(abConfig.AllAssetAB[i].Path, i);
            }

            for (int i = 0; i < abConfig.AllFileDirAB.Count; i++)
            {
                abConfigDic.Add(abConfig.AllFileDirAB[i].Path, i);
            }

            object[] select = Selection.objects;
            for (int i = 0; i < select.Length; i++)
            {
                if(select[i].GetType() == typeof(MonoScript))continue;
                string path = AssetDatabase.GetAssetPath(select[i] as Object);

                string[] temp = {};
                if (select[i].GetType() ==  typeof(TextAsset))
                {
                    
                    temp = (select[i] as TextAsset).name.Split(' ');
                }
                else
                {
                    temp = select[i].ToString().Split(' ');
                }

                string name = string.Empty;
                if (temp.Length > 1)
                {
                    
                    for (int j = 0; j < temp.Length - 1; j++)
                    {
                        Debug.Log(temp[j]);
                        name += temp[j] + " ";
                    }

                    name = name.Trim().TrimEnd().ToLower();
                }
                else if(temp.Length == 1)
                {
                    name = temp[0].Trim().TrimEnd().ToLower();
                }

                if (select[i].GetType() == typeof(DefaultAsset))
                {
                    if (abConfigDic.ContainsKey(path))
                    {
                        int index = abConfigDic[path];
                        var item = abConfig.AllFileDirAB[index];
                        item.ABName = name;
                        item.Path = path;
                    }
                    else
                    {
                        var item = new ABConfig.ABConfigItem();
                        item.ABName = name;
                        item.Path = path;
                        abConfig.AllFileDirAB.Add(item);
                    }
                }
                else
                {
                    if (abConfigDic.ContainsKey(path))
                    {
                        int index = abConfigDic[path];
                        var item = abConfig.AllAssetAB[index];
                        item.ABName = name;
                        item.Path = path;
                    }
                    else
                    {
                        var item = new ABConfig.ABConfigItem();
                        item.ABName = name;
                        item.Path = path;
                        abConfig.AllAssetAB.Add(item);
                    }
                }
            }

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            UnityEditor.AssetDatabase.CreateAsset(abConfig, fileName);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }

        [MenuItem("Assets/AssetBundle/Remove ABAsset")]
        private static void RemoveResAsset()
        {
            if (!Directory.Exists(ABConfigPath))
            {
                Directory.CreateDirectory(ABConfigPath);
            }

            string fileName = ABConfigPath + typeof(ABConfig).Name + ".asset";

            var abConfigOld = Resources.Load<ABConfig>(typeof(ABConfig).Name);
            if (abConfig == null)
            {
                abConfig = ScriptableObject.CreateInstance<ABConfig>();
            }
            else
            {

                abConfig = ScriptableObject.CreateInstance<ABConfig>();
                abConfig.Init(abConfigOld);
            }

            Dictionary<string, int> abConfigDic = new Dictionary<string, int>();
            for (int i = 0; i < abConfig.AllAssetAB.Count; i++)
            {
                abConfigDic.Add(abConfig.AllAssetAB[i].Path, i);
            }

            for (int i = 0; i < abConfig.AllFileDirAB.Count; i++)
            {
                abConfigDic.Add(abConfig.AllFileDirAB[i].Path, i);
            }

            object[] select = Selection.objects;
            for (int i = 0; i < select.Length; i++)
            {
                if (select[i].GetType() == typeof(MonoScript)) continue;
                string path = AssetDatabase.GetAssetPath(select[i] as Object);
                if (select[i].GetType() == typeof(DefaultAsset))
                {
                    if (abConfigDic.ContainsKey(path))
                    {
                        int index = abConfigDic[path];
                        abConfig.AllFileDirAB.RemoveAt(index);
                    }
                }
                else
                {
                    if (abConfigDic.ContainsKey(path))
                    {
                        int index = abConfigDic[path];
                        abConfig.AllAssetAB.RemoveAt(index);
                    }
                }
            }

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            UnityEditor.AssetDatabase.CreateAsset(abConfig, fileName);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }


        #endregion

        #region 打包

        static void Clear()
        {
            _AllFileDir.Clear();
            _AllAssetsAB.Clear();
            _AllFilterAB.Clear();
            _AllPrefabDir.Clear();
            _ConfigFile.Clear();
        }


        [MenuItem("LtUIFrame/AssetBundle/Build")]
        public static void Build()
        {
            BuildAB();

#if UNITY_EDITOR_WIN
            string path = BundleTargetPath.Replace("/", "\\");

            System.Diagnostics.Process.Start("explorer.exe", path);
#endif
        }

        public static void BuildAB()
        {
            Clear();

            if (!Directory.Exists(BundleTargetPath))
            {
                Directory.CreateDirectory(BundleTargetPath);
            }

            if (!Directory.Exists(ABBytePath))
            {
                Directory.CreateDirectory(ABBytePath);
            }

            string abConfigPath = ABConfigPath + typeof(ABConfig).Name + ".asset";
            ABConfig abConfig = AssetDatabase.LoadAssetAtPath<ABConfig>(abConfigPath);
            if (abConfig == null)
            {
                Debug.LogError("请创建ABConfig 配置AssetBundle路径");
                return;
            }

            foreach (ABConfig.ABConfigItem fileDir in abConfig.AllFileDirAB)
            {
                if (_AllFileDir.ContainsKey(fileDir.ABName))
                {
                    Debug.LogError("AB包配置名字重复,请检查! abName:" + fileDir.ABName);
                }
                else
                {
                    _AllFileDir.Add(fileDir.ABName, fileDir.Path);
                    _AllFilterAB.Add(fileDir.Path);
                    _ConfigFile.Add(fileDir.Path);
                }
            }

            foreach (ABConfig.ABConfigItem fileDir in abConfig.AllAssetAB)
            {
                if (_AllAssetsAB.ContainsKey(fileDir.ABName))
                {
                    Debug.LogError("AB包配置名字重复,请检查! abName:" + fileDir.ABName);
                }
                else
                {
                    _AllAssetsAB.Add(fileDir.ABName, fileDir.Path);
                    _AllFilterAB.Add(fileDir.Path);
                    _ConfigFile.Add(fileDir.Path);
                }
            }

            if (abConfig.AllPrefabPathAB.Count > 0)
            {
                string[] allStr = AssetDatabase.FindAssets("t:Prefab", abConfig.AllPrefabPathAB.ToArray());
                for (int i = 0; i < allStr.Length; i++)
                {
                    string path = AssetDatabase.GUIDToAssetPath(allStr[i]);
                    EditorUtility.DisplayProgressBar("查找Prefab", "Prefab:" + path, i * 1.0f / allStr.Length);
                    _ConfigFile.Add(path);
                    if (!ContainAllFileAB(path))
                    {
                        GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                        //获得依赖项
                        string[] allDepend = AssetDatabase.GetDependencies(path);
                        List<string> allDependPath = new List<string>();
                        for (int j = 0; j < allDepend.Length; j++)
                        {
                            if (!ContainAllFileAB(allDepend[j]) && !allDepend[j].EndsWith(".cs"))
                            {
                                _AllFilterAB.Add(allDepend[j]);
                                allDependPath.Add(allDepend[j]);
                            }
                        }

                        if (_AllPrefabDir.ContainsKey(obj.name))
                        {
                            Debug.LogError("存在相同明星的Prefab UIName" + obj.name);
                        }
                        else
                        {
                            _AllPrefabDir.Add(obj.name, allDependPath);
                        }
                    }
                }
            }

            foreach (string name in _AllFileDir.Keys)
            {
                SetABName(name, _AllFileDir[name]);
            }

            foreach (string name in _AllAssetsAB.Keys)
            {
                SetABName(name, _AllAssetsAB[name]);
            }

            foreach (string name in _AllPrefabDir.Keys)
            {
                SetABName(name, _AllPrefabDir[name]);
            }

            BuidAssetBundle();

            //清除设置好的ab标签
            ClearABTagName();

            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }


        /// <summary>
        /// 打包
        /// </summary>
        static void BuidAssetBundle()
        {
            string[] allBundles = AssetDatabase.GetAllAssetBundleNames();
            //key 全路径 value 包名
            Dictionary<string, string> resPathDic = new Dictionary<string, string>();
            for (int i = 0; i < allBundles.Length; i++)
            {
                string[] allBundlePath = AssetDatabase.GetAssetPathsFromAssetBundle(allBundles[i]);
                for (int j = 0; j < allBundlePath.Length; j++)
                {
                    if (allBundlePath[j].EndsWith(".cs"))
                    {
                        continue;
                    }

                    Debug.Log("此aB包:" + allBundles[i] + "下面包含资源文件路径:" + allBundlePath[j]);
                    if (ValidPaht(allBundlePath[j]))
                    {
                        resPathDic.Add(allBundlePath[j], allBundles[i]);
                    }
                }
            }

            DeletAB();
            //生成自己的AB包配置表
            WriteData(resPathDic);

            AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(BundleTargetPath,
                BuildAssetBundleOptions.ChunkBasedCompression,
                EditorUserBuildSettings.activeBuildTarget);

            if (manifest == null)
            {
                Debug.LogError("AssetBundle打包失败");
            }
            else
            {
                Debug.Log("AssetBundle 打包完毕");
            }
        }

        /// <summary>
        /// 是否是有效路径
        /// </summary>
        /// <param UIName="path"></param>
        /// <returns></returns>
        static bool ValidPaht(string path)
        {
            for (int i = 0; i < _ConfigFile.Count; i++)
            {
                if (path.Contains(_ConfigFile[i]))
                {
                    Debug.Log(path+"__" + _ConfigFile[i]);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        static void WriteData(Dictionary<string, string> resPathDic)
        {
            AssetBundleConfig config = new AssetBundleConfig();
            config.ABList = new List<ABBase>();

            foreach (string path in resPathDic.Keys)
            {
                ABBase abBase = new ABBase
                {
                    Path = path,
                    Crc = Crc32.GetCrc32(path),
                    ABName = resPathDic[path],
                    AssetName = path.Remove(0, path.LastIndexOf('/') + 1),
                    ABDependence = new List<string>()
                };

                string[] resDependency = AssetDatabase.GetDependencies(path);
                for (int i = 0; i < resDependency.Length; i++)
                {
                    string tempPath = resDependency[i];
                    if (tempPath == path || path.EndsWith(".cs"))
                    {
                        continue;
                    }

                    if (resPathDic.TryGetValue(tempPath, out var abName))
                    {
                        if (abName == resPathDic[path])
                        {
                            continue;
                        }

                        if (!abBase.ABDependence.Contains(abName))
                        {
                            abBase.ABDependence.Add(abName);
                        }
                    }
                }

                config.ABList.Add(abBase);
            }

            //写入xml
            string xmlPath = Application.dataPath + "/AssetBundleConfig.xml";
            if (File.Exists(xmlPath))
            {
                File.Delete(xmlPath);
            }

            FileStream fileStream = new FileStream(xmlPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
            XmlSerializer xs = new XmlSerializer(config.GetType());
            xs.Serialize(sw, config);
            sw.Close();
            fileStream.Close();

            //写入二进制
            foreach (ABBase abBase in config.ABList)
            {
                abBase.Path = "";
            }

            FileStream fs = new FileStream(ABByteName, FileMode.Create, FileAccess.ReadWrite,
                FileShare.ReadWrite);
            fs.Seek(0, SeekOrigin.Begin);
            fs.SetLength(0);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, config);
            fs.Close();
            AssetDatabase.Refresh();
            SetABName("assetbundleconfig", ABByteName);
        }

        /// <summary>
        /// 删除没用的ab包
        /// </summary>
        static void DeletAB()
        {
            string[] allBundlesName = AssetDatabase.GetAllAssetBundleNames();
            DirectoryInfo direction = new DirectoryInfo(BundleTargetPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (ContainABName(files[i].Name, allBundlesName) || files[i].Name.EndsWith(".meta") ||
                    files[i].Name.EndsWith(".manifest") || files[i].Name.EndsWith("assetbundleconfig"))
                {
                    continue;
                }
                else
                {
                    Debug.Log("此ab包已经被删或者改名了:" + files[i].Name);
                    if (File.Exists(files[i].FullName))
                    {
                        File.Delete(files[i].FullName);
                        string manifest = files[i].FullName + ".manifest";
                        if (File.Exists(manifest))
                        {
                            File.Delete(manifest);
                        }
                    }
                }
            }
        }

        static bool ContainABName(string name, string[] strs)
        {
            for (int i = 0; i < strs.Length; i++)
            {
                if (name == strs[i])
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// 清除ab包标签名
        /// </summary>
        static void ClearABTagName()
        {
            string[] oldABNames = AssetDatabase.GetAllAssetBundleNames();
            for (int i = 0; i < oldABNames.Length; i++)
            {
                AssetDatabase.RemoveAssetBundleName(oldABNames[i], true);
                EditorUtility.DisplayProgressBar("清除AB包名", "名字:" + oldABNames[i], 1.0f / oldABNames.Length);
            }
        }

        /// <summary>
        /// 设置资源ab标记名字
        /// </summary>
        /// <param UIName="name"></param>
        /// <param UIName="path"></param>
        static void SetABName(string name, string path)
        {
            //得到文件类
            AssetImporter assetImporter = AssetImporter.GetAtPath(path);
            if (assetImporter == null)
            {
                Debug.LogError("不存在此路径文件 Path: " + path);
            }
            else
            {
                assetImporter.assetBundleName = name;
            }
        }

        static void SetABName(string name, List<string> path)
        {
            for (int i = 0; i < path.Count; i++)
            {
                SetABName(name, path[i]);
            }
        }



        /// <summary>
        /// 是否包含在已经有的AB包里,用来做ab包冗余剔除
        /// 判断打包缓存中是否存在 当前路径
        /// </summary>
        /// <param UIName="path"></param>
        /// <returns></returns>
        static bool ContainAllFileAB(string path)
        {
            for (int i = 0; i < _AllFilterAB.Count; i++)
            {
                if (path == _AllFilterAB[i] ||
                    (path.Contains(_AllFilterAB[i]) && path.Replace(_AllFilterAB[i], "")[0] == '/'))
                {
                    return true;
                }
            }

            return false;
        }


        #endregion

    }
}