/*
 *    描述:
 *          1. Resource资源管理类
 *
 *    开发人: 邓平
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LtFramework.Util;
using LtFramework.Util.Pools;
using LtFramework.Util.Tools;

namespace LtFramework.ResKit
{
    public enum ResLoadMode
    {
        Null,
        Res,
        Ab,
    }

    public class ResManager : ISingleton
    {
        #region 单例

        private ResManager()
        {
            MonoRes monoRes = MonoRes.Instance;
            for (int i = 0; i < (int) LoadResPriority.Res_Num; i++)
            {
                _LoadingAssetList[i] = new List<AsyncLoadResParam>();
            }

            ResKitConfig config = Resources.Load<ResKitConfig>(typeof(ResKitConfig).Name);
            if (config)
            {
                loadResMode = config.LoadResMode;

                if (loadResMode == LoadResMode.AssetBundle && IsEditorLoadMode == false)
                {
                    AssetBundleMgr.Instance.LoadAssetBundleConfig();
                }
            }

            monoRes.StartCoroutine(AsyncLoadCor());
        }


        public void OnSingletonInit()
        {

        }

        internal static ResManager Instance => SingletonProperty<ResManager>.Instance;


        public void Dispose()
        {
            SingletonProperty<ResManager>.Dispose();
        }

        #endregion

        #region API

        /// <summary>
        /// 同步资源加载
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源路径</param>
        /// <returns>资源对象</returns>
        internal static T LoadRes<T>(string path) where T : UnityEngine.Object
        {
            return Instance.LoadResource<T>(path);
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源路径</param>
        /// <param name="onFinish">完成回调</param>
        /// <param name="priority">资源加载优先级</param>
        /// <param name="paramValues">参数列表</param>
        internal static void AsyncLoadRes<T>(string path, OnAsyncObjFinish onFinish,
            LoadResPriority priority = LoadResPriority.Res_Slow, params object[] paramValues)
            where T : UnityEngine.Object
        {
            Instance.AsyncLoadResource<T>(path, onFinish,0,ResLoadMode.Null, priority, paramValues);
        }

        /// <summary>
        /// 释放资源
        ///     释放时要置空资源引用
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="destroyRes">是否清空缓存</param>
        internal static void ReleaseRes(string path, bool destroyRes = true)
        {
            Instance.ReleaseResource(path,0, destroyRes);
        }

        /// <summary>
        /// 释放资源
        ///     释放时要置空资源引用
        /// </summary>
        /// <param name="obj">资源对象</param>
        /// <param name="destroyRes">是否清空缓存</param>
        internal static void ReleaseRes(UnityEngine.Object obj, bool destroyRes = true)
        {
            Instance.ReleaseResource(obj, destroyRes);
        }

        /// <summary>
        /// 资源预加载
        /// </summary>
        /// <param name="path"></param>
        internal static void PreloadRes<T>(string path) where T : UnityEngine.Object
        {
            Instance.PreloadResource<T>(path);
        }

        /// <summary>
        /// 清空 跳场景清掉 的缓存
        /// </summary>
        internal static void Clear()
        {
            Instance.ClearCache();
        }

        #endregion

        #region 资源加载模式

        private const string _loadResModeSave = "LoadResByAssetBundle";
        private static LoadResMode loadResMode = ResKit.LoadResMode.Resource;

#if UNITY_EDITOR
        private const string _EditorLoadMode = "_EditorLoadMode";

        public static bool EditorLoadMode
        {
            get
            {
                if (!UnityEditor.EditorPrefs.HasKey(_EditorLoadMode))
                {
                    UnityEditor.EditorPrefs.SetBool(_EditorLoadMode, true);
                }

                return UnityEditor.EditorPrefs.GetBool(_EditorLoadMode);
            }
            set { UnityEditor.EditorPrefs.SetBool(_EditorLoadMode, value); }
        }
#endif

        internal static bool IsEditorLoadMode
        {
            get
            {
#if UNITY_EDITOR
                return EditorLoadMode;

#else
                return false;
#endif
            }
        }

        #endregion

        #region 字段

        /// <summary>
        /// 最大缓存个数
        /// </summary>
        public const int _MaxCacheCount = 500;

        /// <summary>
        /// 最长连续加载异步资源时间 微秒
        /// </summary>
        private const long _MaxLoadResTime = 200000;

        #endregion

        #region 缓存

        /// <summary>
        /// 缓存使用资源
        /// </summary>
        private readonly Dictionary<uint, ResItem> _UsededResCacheDic = new Dictionary<uint, ResItem>();

        /// <summary>
        /// 缓存没有使用的资源,当达到资源释放标准后根据使用时间进行资源释放
        /// </summary>
        private readonly CMapList<ResItem> _NoRefrenceAssetMapList = new CMapList<ResItem>();

        /// <summary>
        /// 异步加载资源回调类参数 对象池
        /// </summary>
        private readonly ClassObjectPool<AsyncLoadResParam> _AsyncLoadResParamPool =
            new ClassObjectPool<AsyncLoadResParam>(50);

        /// <summary>
        /// 异步加载资源回调类 对象池
        /// </summary>
        private readonly ClassObjectPool<AsyncCallBack> _AsyncCallBackPool = new ClassObjectPool<AsyncCallBack>(100);

        //private readonly ClassObjectPool<ResItem> _ResItemPool = new ClassObjectPool<ResItem>(100);

        /// <summary>
        /// 正在异步加载的资源列表
        /// </summary>
        private readonly List<AsyncLoadResParam>[] _LoadingAssetList =
            new List<AsyncLoadResParam>[(int) LoadResPriority.Res_Num];

        /// <summary>
        /// 正在异步加载的Dic
        /// </summary>
        private readonly Dictionary<uint, AsyncLoadResParam> _LoadingAssetDic =
            new Dictionary<uint, AsyncLoadResParam>();

        #endregion

        #region AssetHelper

        public void ClearAssetCache(int assetHelperHash)
        {
            MonoRes.Instance.ClearAsset(assetHelperHash);
        }

        public void ClearAssetCacheEnumerator(int assetHelperHash)
        {
            List<uint> temp = null;
            Dictionary<uint, int> objDic = null;
            _AssetHelperCache.TryGetValue(assetHelperHash, out temp);
            _AssetHelperObjCache.TryGetValue(assetHelperHash, out objDic);
            if ((temp != null && temp.Count > 0) ||( objDic != null && objDic.Count >0))
            {
                MonoRes.Instance.StartCoroutine(ClearAssetCache(assetHelperHash, temp, objDic));
            }
        }

        private IEnumerator ClearAssetCache(int assetHelperHash, List<uint> list,Dictionary<uint,int> objDic)
        {
            Debug.Log("协同");
            if (list != null)
            {
                foreach (uint crc in list)
                {
                    ReleaseAsset(crc);
                    Debug.Log("卸载" + crc);
                    yield return null;
                }
            }

            if (objDic != null)
            {
                foreach (uint crc in objDic.Keys)
                {
                    ReleaseAsset(crc,objDic[crc]);
                    Debug.Log("卸载" + crc);
                    yield return null;
                }
            }

            Debug.Log("移除" + assetHelperHash);
            _AssetHelperCache.Remove(assetHelperHash);
        }

        #region Res


        Dictionary<int, List<uint>> _AssetHelperCache = new Dictionary<int, List<uint>>();

        private void AddResAsset(uint crc, int assetHelperHash)
        {
            Debug.Log("add " + assetHelperHash);
            if (!_AssetHelperCache.ContainsKey(assetHelperHash))
            {
                _AssetHelperCache.Add(assetHelperHash, new List<uint>());
            }
            List<uint> temp = _AssetHelperCache[assetHelperHash];
            if (!temp.Contains(crc))
            {
                Debug.Log("添加 " + crc);
                _AssetHelperCache[assetHelperHash].Add(crc);
            }
        }

        private void RemoveResAsset(uint crc, int assetHelperHash)
        {
            Debug.Log("remove " + assetHelperHash);
            if (_AssetHelperCache.ContainsKey(assetHelperHash) && _AssetHelperCache[assetHelperHash].Contains(crc))
            {
                var tempList = _AssetHelperCache[assetHelperHash];
                tempList.Remove(crc);
                if (tempList.Count <= 0)
                {
                    Debug.Log("移除 " + crc);
                    _AssetHelperCache.Remove(assetHelperHash);
                }
            }
            else
            {
                Debug.LogError("移除资源crc 不存在");
            }
        }

        #endregion

        #region Obj

        Dictionary<int, Dictionary<uint, int>> _AssetHelperObjCache = new Dictionary<int, Dictionary<uint, int>>();

        internal void AddObjAsset(uint crc, int assetHelperHash)
        {
            Debug.Log("add " + assetHelperHash);
            if (!_AssetHelperObjCache.ContainsKey(assetHelperHash))
            {
                _AssetHelperObjCache.Add(assetHelperHash, new Dictionary<uint, int>());
            }
            Dictionary<uint, int> temp = _AssetHelperObjCache[assetHelperHash];
            if (!temp.ContainsKey(crc))
            {
                temp.Add(crc, 1);
            }
            else
            {
                temp[crc]++;
            }

            Debug.Log("obj 引用数 :" + _AssetHelperObjCache[assetHelperHash][crc]);
        }

        internal void RemoveObjAsset(uint crc, int assetHelperHash)
        {
            //Debug.Log("remove " + assetHelperHash);
            //if (_AssetHelperObjCache.ContainsKey(assetHelperHash) && _AssetHelperObjCache[assetHelperHash].ContainsKey(crc))
            //{
            //    var tempDic = _AssetHelperObjCache[assetHelperHash];
            //    tempDic[crc]--;
            //    Debug.Log("移除 " + crc);
            //    if (tempDic[crc] <= 0)
            //    {
            //        tempDic.Remove(crc);
            //        if (tempDic.Count <= 0)
            //        {
            //            _AssetHelperObjCache.Remove(assetHelperHash);
            //        }
            //    }
            //}
            //else
            //{
            //    Debug.LogError("移除资源crc 不存在");
            //}
        }

        #endregion

        #endregion

        #region Res

        internal ResLoadMode LoadMode(ref string path)
        {
            ResLoadMode mode = ResLoadMode.Null;
            if (path.Length > ResKitConst.ResUrl.Length)
            {
                if (path.Contains(ResKitConst.ResUrl))
                {
                    path = path.Replace(ResKitConst.ResUrl, "");
                    mode = ResLoadMode.Res;
                }
                else if (path.Contains(ResKitConst.AbUrl))
                {
                    path = path.Replace(ResKitConst.AbUrl, "");
                    mode = ResLoadMode.Ab;
                }
            }
            return mode;
        }

        internal LoadResMode JudgeLoadMode(ResLoadMode mode)
        {
            LoadResMode load = LoadResMode.Resource;
            if (mode == ResLoadMode.Null)
            {
                load = loadResMode;
            }
            else if (mode == ResLoadMode.Res)
            {
                load = LoadResMode.Resource;
            }
            else if (mode == ResLoadMode.Ab)
            {
                load = LoadResMode.AssetBundle;
            }

            return load;
        }

        private ResItem LoadResource<T>(ResLoadMode mode, string path, uint crc) where T : UnityEngine.Object
        {
            T obj = null;
            ResItem item = null;
#if UNITY_EDITOR
            if (IsEditorLoadMode)
            {
                if (JudgeLoadMode(mode) == LoadResMode.Resource)
                {
                    obj = Resources.Load<T>(path);
                }
                else
                {
                    obj = LoadAssetByEditor<T>(path);
                }
            }
#endif
            if (obj == null)
            {
                if (JudgeLoadMode(mode) == LoadResMode.Resource)
                {
                    obj = Resources.Load<T>(path);
                }
                else
                {
                    item = AssetBundleMgr.Instance.LoadResourceAssetBundle(crc);
                    if (item != null && item.AssetBundle != null)
                    {
                        if (item.Obj != null)
                        {
                            obj = item.Obj as T;
                        }
                        else
                        {
                            Debug.Log("AssetBundle模型下加载");
                            obj = item.AssetBundle.LoadAsset<T>(item.AssetName);
                        }
                    }
                    else
                    {
                        Debug.LogErrorFormat("加载AssetBundle失败 : 不能从AssetBundleConfig中找到 Path :{0}", path);
                    }
                }
            }

            if (obj == null)
            {
                Debug.LogError("加载资源失败,请检查资源[加载模式] mode ;" + loadResMode + " [路径] Path:" + path);
            }

            if (item == null)
            {
                item = new ResItem();
            }
            item.Crc = crc;
            item.Obj = obj;
            if (obj)
            {
                item.Guid = obj.GetInstanceID();
            }
            return item;
        }

        /// <summary>
        /// 同步资源加载 , 外部直接调用,仅加载不需要实例化的资源,例如Texture,Audio
        /// </summary>
        /// <typeparam UIName="T"></typeparam>
        /// <param UIName="path"></param>
        /// <returns></returns>
        internal T LoadResource<T>(string path, uint crc = 0,ResLoadMode mode = ResLoadMode.Null) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("路径为null");
                return null;
            }

            if (mode == ResLoadMode.Null)
            {
                mode = LoadMode(ref path);
            }
            if (crc == 0)
            {
                crc = Crc32.GetCrc32(path);
            }

            ResItem item = GetCacheResouceItem(crc);
            if (item != null)
            {
                return item.Obj as T;
            }

            //资源加载
            item = LoadResource<T>(mode, path, crc);
            CacheResource(path, ref item, crc);
            //Debug.Log(item.Obj.name);
            return item.Obj as T;
        }

        internal T LoadResource<T>(string path,int assetHelperHash) where T : UnityEngine.Object
        {
            if (!string.IsNullOrEmpty(path))
            {
                ResLoadMode mode = LoadMode(ref path);
                uint crc = Crc32.GetCrc32(path);
                AddResAsset(crc, assetHelperHash);
                var res = LoadResource<T>(path, crc, mode);
                return res;
            }
            else
            {
                Debug.LogError("路径为null");
                return null;
            }
        }

        /// <summary>
        /// 异步加载资源 (仅仅是不需要实例化的资源)
        /// </summary>
        private void AsyncLoadResource<T>(string path, OnAsyncObjFinish onFinish,uint crc = 0,ResLoadMode mode = ResLoadMode.Null,
            LoadResPriority priority = LoadResPriority.Res_Slow, params object[] paramValues)
            where T : UnityEngine.Object
        {
            if (mode == ResLoadMode.Null)
            {
                mode = LoadMode(ref path);
            }
            if (crc == 0)
            {
                crc = Crc32.GetCrc32(path);
            }

            ResItem item = GetCacheResouceItem(crc);
            if (item != null)
            {
                if (onFinish != null)
                {
                    onFinish(path, item.Obj, paramValues);
                }

                return;
            }

            //判断是否在加载中
            AsyncLoadResParam param = null;
            if (!_LoadingAssetDic.TryGetValue(crc, out param) || param == null)
            {
                param = _AsyncLoadResParamPool.Spawn(true);
                param.Crc = crc;
                param.Path = path;
                param.Priority = priority;
                param.ResType = typeof(T);
                param.LoadMode = JudgeLoadMode(mode);
                _LoadingAssetDic.Add(crc, param);
                _LoadingAssetList[(int)priority].Add(param);
            }

            //回调列表里面加回调
            AsyncCallBack callBack = _AsyncCallBackPool.Spawn(true);
            callBack.OnObjFinish = onFinish;
            if (paramValues != null && paramValues.Length > 0)
            {
                foreach (object value in paramValues)
                {
                    callBack.Params.Add(value);
                }
            }

            param.CallBackList.Add(callBack);
        }


        /// <summary>
        /// 异步加载资源 (仅仅是不需要实例化的资源)
        /// </summary>
        internal void AsyncLoadResourceAsset<T>(string path, OnAsyncObjFinish onFinish, int assetHelperHash,
            LoadResPriority priority = LoadResPriority.Res_Slow, params object[] paramValues)
            where T : UnityEngine.Object
        {
            if (!string.IsNullOrEmpty(path))
            {
                ResLoadMode mode = LoadMode(ref path);
                uint crc = Crc32.GetCrc32(path);
                AddResAsset(crc, assetHelperHash);
                AsyncLoadResource<T>(path, onFinish, crc, mode, priority, paramValues);
            }
            else
            {
                Debug.LogError("路径为null");
            }
        }

        /// <summary>
        /// 不需要实例化的资源卸载,根据路径
        /// </summary>
        /// <param UIName="path"></param>
        /// <param UIName="destroyOjb"></param>
        /// <returns></returns>
        internal bool ReleaseResource(string path, uint crc, bool destroyRes = true)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("释放资源的路径为null");
                return false;
            }

            if (crc == 0)
            {
                crc = Crc32.GetCrc32(path);
            }


            ResItem item = null;

            if (!_UsededResCacheDic.TryGetValue(crc, out item) || item == null)
            {
                Debug.LogError("不存在该资源 Path: " + path + " 可能多次释放");
                return false;
            }
            item.RefCount.Decrease();

            DestroyResourceItem(item, destroyRes);
            return true;
        }

        /// <summary>
        /// 不需要实例化的资源卸载,根据路径
        /// </summary>
        /// <param UIName="path"></param>
        /// <param UIName="destroyOjb"></param>
        /// <returns></returns>
        internal bool ReleaseResourceAsset(string path, int assetHelpHash, bool destroyRes = true)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("释放资源的路径为null");
                return false;
            }
            uint crc = Crc32.GetCrc32(path);
            RemoveResAsset(crc, assetHelpHash);
            return ReleaseResource(path, crc, destroyRes);
        }

        /// <summary>
        /// 卸载 不需要的实例化资源 根据对象
        /// </summary>
        /// <param UIName="obj"></param>
        /// <param UIName="destroyOjb"></param>
        /// <returns></returns>
        private bool ReleaseResource(ResItem item, bool destroyRes = true)
        {
            if (item == null)
            {
                Debug.LogError("不存在该资源 obj: " + item.Obj.name + " 可能多次释放");
                return false;
            }

            item.RefCount.Decrease();
            DestroyResourceItem(item, destroyRes);
            return true;
        }

        /// <summary>
        /// 卸载 不需要的实例化资源 根据对象
        /// </summary>
        /// <param UIName="obj"></param>
        /// <param UIName="destroyOjb"></param>
        /// <returns></returns>
        private bool ReleaseResource(Object obj, bool destroyRes = true)
        {
            if (obj == null)
            {
                Debug.LogError("释放资源的对象为null");
                return false;
            }

            ResItem item = null;
            foreach (var res in _UsededResCacheDic.Values)
            {
                if (res.Guid == obj.GetInstanceID())
                {
                    item = res;
                }
            }
            return ReleaseResource(item, destroyRes);
        }

        /// <summary>
        /// 卸载 不需要的实例化资源 根据对象
        /// </summary>
        /// <param UIName="obj"></param>
        /// <param UIName="destroyOjb"></param>
        /// <returns></returns>
        internal bool ReleaseResourceAsset(UnityEngine.Object obj,int assetHelpHash, bool destroyRes = true)
        {
            if (obj == null)
            {
                Debug.LogError("释放资源的对象为null");
                return false;
            }

            ResItem item = null;
            foreach (var res in _UsededResCacheDic.Values)
            {
                if (res.Guid == obj.GetInstanceID())
                {
                    item = res;
                }
            }

            uint crc = item.Crc;
            RemoveResAsset(crc, assetHelpHash);
            return ReleaseResource(item, destroyRes);
        }

        /// <summary>
        /// 预加载资源
        /// </summary>
        public void PreloadResource<T>(string path,uint crc = 0,ResLoadMode mode = ResLoadMode.Null) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            if (mode == ResLoadMode.Null)
            {
                mode = LoadMode(ref path);
            }
            if (crc == 0)
            {
                crc = Crc32.GetCrc32(path);
            }
            ResItem item = GetCacheResouceItem(crc, 0);
            if (item != null)
            {
                return;
            }

            //资源加载
            item = LoadResource<T>(mode, path, crc);

            //缓存
            CacheResource(path, ref item, crc);
            //跳场景不清空缓存
            item.Clear = false;
            ReleaseResource(item.Obj, false);
        }


        /// <summary>
        /// 预加载资源
        /// </summary>
        public void PreloadResourceAsset<T>(string path,int assetHelpHash) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("预加载的资源路径为null");
                return;
            }
            ResLoadMode mode = LoadMode(ref path);
            uint crc = Crc32.GetCrc32(path);
            AddResAsset(crc, assetHelpHash);
            PreloadResource<T>(path, crc, mode);
        }

        /// <summary>
        /// 清空 所有跳场景清除的资源
        /// </summary>
        private void ClearCache()
        {
            List<ResItem> tempList = new List<ResItem>();
            foreach (ResItem item in _UsededResCacheDic.Values)
            {
                if (item.Clear)
                {
                    tempList.Add(item);
                }
            }

            foreach (ResItem item in tempList)
            {
                DestroyResourceItem(item, true);
            }

            tempList.Clear();
        }

        /// <summary>
        /// 清空 所有AssetHelp加载的资源
        /// </summary>
        internal void ClearCacheAsset(int assetHelpHash)
        {
            List<uint> temp = null;
            _AssetHelperCache.TryGetValue(assetHelpHash, out temp);
            if (temp != null && temp.Count > 0)
            {
                foreach (uint crc in temp)
                {
                    ReleaseAsset(crc);
                }

                _AssetHelperCache.Remove(assetHelpHash);
            }
        }

        /// <summary>
        /// 不需要实例化的资源卸载,根据路径 AssetHelp
        /// </summary>
        /// <param UIName="path"></param>
        /// <param UIName="destroyOjb"></param>
        /// <returns></returns>
        internal bool ReleaseAsset(uint crc, int count = 1)
        {
            ResItem item = null;

            if (!_UsededResCacheDic.TryGetValue(crc, out item) || item == null)
            {
                return false;
            }

            //预加载没有使用的资源 引用为0
            if (item.RefCount.count > 0)
            {
                if (count > item.RefCount.count)
                {
                    count = item.RefCount.count;
                }
                item.RefCount.Decrease(count);
            }

            DestroyResourceItem(item, true);
            return true;
        }


        #endregion

        /// <summary>
        /// 资源id
        /// </summary>
        private static long _Guid = 0;

        /// <summary>
        /// 创建唯一的资源ID
        /// </summary>
        /// <returns></returns>
        internal static long CreateGuid()
        {
            return _Guid++;
        }

        /// <summary>
        /// 取消异步加载资源
        /// </summary>
        /// <returns></returns>
        internal bool CancleLoad(ResouceObj res)
        {
            AsyncLoadResParam para = null;
            if (_LoadingAssetDic.TryGetValue(res.Crc, out para) &&
                _LoadingAssetList[(int) para.Priority].Contains(para))
            {
                for (int i = para.CallBackList.Count - 1; i >= 0; i--)
                {
                    AsyncCallBack tempCallBack = para.CallBackList[i];
                    if (tempCallBack != null && res == tempCallBack.ResObj)
                    {
                        para.CallBackList.Remove(tempCallBack);
                        tempCallBack.Reset();
                        _AsyncCallBackPool.Recycle(tempCallBack);
                    }
                }

                if (para.CallBackList.Count <= 0)
                {
                    _LoadingAssetList[(int) para.Priority].Remove(para);
                    para.Reset();
                    _AsyncLoadResParamPool.Recycle(para);

                    _LoadingAssetDic.Remove(res.Crc);
                    return true;
                }

            }

            return false;
        }


        /// <summary>
        /// 同步加载资源,针对ObjectManager的接口
        /// </summary>
        /// <param UIName="path"></param>
        /// <param UIName="resObj"></param>
        /// <returns></returns>
        public ResouceObj LoadRessource(string path, ResLoadMode mode, ResouceObj resObj)
        {
            if (resObj == null)
            {
                return null;
            }

            uint crc = resObj.Crc == 0 ? Crc32.GetCrc32(path) : resObj.Crc;

            ResItem item = GetCacheResouceItem(crc);
            if (item != null)
            {
                resObj.ResItem = item;
                return resObj;
            }

            item = LoadResource<Object>(mode, path, crc);
            CacheResource(path, ref item, crc);
            resObj.ResItem = item;
            item.Clear = resObj.ClearByChangeScene;

            return resObj;
        }

        /// <summary>
        /// 根据ResourceObj 卸载资源
        /// </summary>
        /// <param UIName="resObj"></param>
        /// <param UIName="destroyOjb"></param>
        /// <returns></returns>
        internal bool ReleaseResource(ResouceObj resObj,int poolCacheCount, bool destroyOjb = false)
        {
            if (resObj == null)
            {
                return false;
            }

            ResItem item = null;
            if (!_UsededResCacheDic.TryGetValue(resObj.Crc, out item) || item == null)
            {
                Debug.LogError("不存在该资源 anme: " + resObj.CloneObj.name + " 可能多次释放");
                return false;
            }

            GameObject.Destroy(resObj.CloneObj);

            if (poolCacheCount == 0 && resObj.ResItem.RefCount.count == 1)
            {
                resObj.ResItem.RefCount.Decrease();
                DestroyResourceItem(item, destroyOjb);
            }
            return true;
        }

        /// <summary>
        /// 根据ResourceObj 卸载资源
        /// </summary>
        /// <param UIName="resObj"></param>
        /// <param UIName="destroyOjb"></param>
        /// <returns></returns>
        internal bool ReleaseResourceAsset(ResouceObj resObj,int assetHelpHash, int poolCacheCount, bool destroyOjb = false)
        {
            if (resObj == null)
            {
                return false;
            }

            ResItem item = null;
            if (!_UsededResCacheDic.TryGetValue(resObj.Crc, out item) || item == null)
            {
                Debug.LogError("不存在该资源 anme: " + resObj.CloneObj.name + " 可能多次释放");
                return false;
            }

            RemoveObjAsset(item.Crc,assetHelpHash);
            GameObject.Destroy(resObj.CloneObj);

            if (poolCacheCount == 0 && resObj.ResItem.RefCount.count == 1)
            {
                resObj.ResItem.RefCount.Decrease();
                DestroyResourceItem(item, destroyOjb);
            }
            return true;
        }



        /// <summary>
        /// 缓存加载的资源
        /// </summary>
        /// <param UIName="path"></param>
        /// <param UIName="item"></param>
        /// <param UIName="crc"></param>
        /// <param UIName="obj"></param>
        /// <param UIName="addrefcount"></param>
        void CacheResource(string path, ref ResItem item, uint crc, int addrefcount = 1)
        {
            //缓存太多,清除最早没有使用的资源
            WashOut();

            if (item == null)
            {
                Debug.LogError("没有创建ResItem  Path : " + path);
            }

            item.RefCount.Increase(addrefcount);
            ResItem oldItem = null;
            if (_UsededResCacheDic.TryGetValue(crc, out oldItem))
            {
                _UsededResCacheDic[item.Crc] = item;
            }
            else
            {
                _UsededResCacheDic.Add(item.Crc, item);
            }
        }

        /// <summary>
        /// 缓存太多, 清除最早没有使用的资源
        /// </summary>
        protected void WashOut()
        {
            // 当前内存使用大于80% 时候 清除最早没有使用的资源
            //当大于好吃个数时进行一半释放
            while (_NoRefrenceAssetMapList.Size() >= _MaxCacheCount)
            {
                for (int i = 0; i < _MaxCacheCount / 2; i++)
                {
                    ResItem item = _NoRefrenceAssetMapList.Back();
                    DestroyResourceItem(item, true);
                }
            }
        }

        /// <summary>
        /// 回收一个资源
        /// </summary>
        /// <param UIName="item"></param>
        /// <param UIName="destroy"></param>
        internal void DestroyResourceItem(ResItem item, bool destroyCache = false)
        {
            Debug.Log(item.RefCount.count +"资源卸载");
            if (item == null || item.RefCount.count > 0)
            {
                return;
            }

            if (!destroyCache)
            {
                _NoRefrenceAssetMapList.InsertToHead(item);
                return;
            }

            if (!_UsededResCacheDic.Remove(item.Crc))
            {
                return;
            }

            _NoRefrenceAssetMapList.Remove(item);

            //释放AssetBundle引用
            AssetBundleMgr.Instance.ReleaseAsset(item);

            //清空资源对应的对象池资源
            ObjManager.Instance.ClearPoolObject(item.Crc);

            if (item.Obj != null)
            {
                item.Obj = null;

#if UNITY_EDITOR

                Debug.Log("卸载完成");
                Resources.UnloadUnusedAssets();
#endif
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// 编辑器模式下加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        protected T LoadAssetByEditor<T>(string path) where T : UnityEngine.Object
        {
            T t = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
            return t;
        }

#endif

        /// <summary>
        /// 从缓存中获取resourceItem
        /// </summary>
        /// <param UIName="crc"></param>
        /// <param UIName="addRefcount"></param>
        /// <returns></returns>
        ResItem GetCacheResouceItem(uint crc, int addRefcount = 1)
        {
            ResItem item = null;
            if (_UsededResCacheDic.TryGetValue(crc, out item) && item != null)
            {
                if (item.RefCount.count == 0)
                {
                    item.RefCount.Increase(addRefcount);
                }
            }
            return item;
        }

        /// <summary>
        /// 针对ObjectManager的异步加载接口
        /// </summary>
        /// <param UIName="path"></param>
        /// <param UIName="resObj"></param>
        /// <param UIName="dealFinish"></param>
        /// <param UIName="priority"></param>
        internal void AsyncLoadResource(string path,ResLoadMode mode, ResouceObj resObj, OnAsyncFinish dealFinish,
            LoadResPriority priority)
        {
            ResItem item = GetCacheResouceItem(resObj.Crc);
            if (item != null)
            {
                resObj.ResItem = item;
                if (dealFinish != null)
                {
                    dealFinish(path, resObj);
                }
                return;
            }

            //判断是否在加载中
            AsyncLoadResParam param = null;
            if (!_LoadingAssetDic.TryGetValue(resObj.Crc, out param) || param == null)
            {
                param = _AsyncLoadResParamPool.Spawn(true);
                param.Crc = resObj.Crc;
                if (mode == ResLoadMode.Ab)
                {
                    param.LoadMode = LoadResMode.AssetBundle;
                }
                else
                {
                    param.LoadMode = LoadResMode.Resource;
                }
                param.Path = path;
                param.Priority = priority;
                _LoadingAssetDic.Add(resObj.Crc, param);
                _LoadingAssetList[(int) priority].Add(param);
                
            }

            //回调列表里面加回调
            AsyncCallBack callBack = _AsyncCallBackPool.Spawn(true);
            callBack.OnFinish = dealFinish;
            callBack.ResObj = resObj;
            param.CallBackList.Add(callBack);
        }


        /// <summary>
        /// 异步加载
        /// </summary>
        /// <returns></returns>
        IEnumerator AsyncLoadCor()
        {
            List<AsyncCallBack> callBackList = null;
            //上一次yield的时间
            long lastYieldTime = System.DateTime.Now.Ticks;
            while (true)
            {
                bool haveYield = false;
                for (int i = 0; i < (int) LoadResPriority.Res_Num; i++)
                {
                    if (_LoadingAssetList[(int) LoadResPriority.Res_Hight].Count > 0)
                    {
                        i = (int) LoadResPriority.Res_Hight;
                    }
                    else if (_LoadingAssetList[(int) LoadResPriority.Res_Middle].Count > 0)
                    {
                        i = (int) LoadResPriority.Res_Middle;
                    }

                    List<AsyncLoadResParam> loadingList = _LoadingAssetList[i];
                    if (loadingList.Count <= 0)
                    {
                        continue;
                    }

                    AsyncLoadResParam loadingItem = loadingList[0];
                    loadingList.RemoveAt(0);
                    callBackList = loadingItem.CallBackList;

                    Object obj = null;
                    ResItem item = null;
#if UNITY_EDITOR
                    if (IsEditorLoadMode)
                    {
                        if (item == null)
                        {
                            item = new ResItem();
                            item.Crc = loadingItem.Crc;
                        }

                        if (loadingItem.LoadMode == ResKit.LoadResMode.Resource)
                        {
                            ResourceRequest resRequest = null;
                            if (loadingItem.ResType == typeof(Sprite))
                            {
                                resRequest = Resources.LoadAsync<Sprite>(loadingItem.Path);
                            }
                            else
                            {
                                resRequest = Resources.LoadAsync<Object>(loadingItem.Path);
                            }

                            yield return resRequest;
                            if (resRequest.isDone)
                            {
                                obj = resRequest.asset;
                            }

                            lastYieldTime = System.DateTime.Now.Ticks;
                        }
                        else
                        {
                            if (loadingItem.ResType == typeof(Sprite))
                            {
                                obj = LoadAssetByEditor<Sprite>(loadingItem.Path);
                            }
                            else
                            {
                                obj = LoadAssetByEditor<Object>(loadingItem.Path);
                            }
                            //模拟异步加载
                            yield return new WaitForSeconds(0.2f);
                        }
                    }
#endif
                    if (obj == null)
                    {
                        if (loadingItem.LoadMode == ResKit.LoadResMode.Resource)
                        {
                            if (item == null)
                            {
                                item = new ResItem();
                                item.Crc = loadingItem.Crc;
                            }

                            ResourceRequest resRequest = null;
                            if (loadingItem.ResType == typeof(Sprite))
                            {
                                resRequest = Resources.LoadAsync<Sprite>(loadingItem.Path);
                            }
                            else
                            {
                                resRequest = Resources.LoadAsync<Object>(loadingItem.Path);
                            }

                            yield return resRequest;
                            if (resRequest.isDone)
                            {
                                obj = resRequest.asset;
                            }

                            lastYieldTime = System.DateTime.Now.Ticks;
                        }
                        else
                        {
                            item = AssetBundleMgr.Instance.LoadResourceAssetBundle(loadingItem.Crc);
                            if (item != null && item.AssetBundle != null)
                            {
                                AssetBundleRequest abRequest = null;
                                if (loadingItem.ResType == typeof(Sprite))
                                {
                                    //是图片资源
                                    abRequest = item.AssetBundle.LoadAssetAsync<Sprite>(item.AssetName);
                                }
                                else
                                {
                                    //不是图片资源
                                    abRequest = item.AssetBundle.LoadAssetAsync(item.AssetName);
                                }

                                yield return abRequest;
                                if (abRequest.isDone)
                                {
                                    obj = abRequest.asset;
                                }

                                lastYieldTime = System.DateTime.Now.Ticks;
                            }
                            else
                            {
                                Debug.LogErrorFormat("加载AssetBundle失败 : 不能从AssetBundleConfig中找到 Path :{0}", loadingItem.Path);
                            }
                        }
                    }

                    item.Crc = loadingItem.Crc;
                    item.Obj = obj;
                    item.Guid = obj.GetInstanceID();
                    CacheResource(loadingItem.Path, ref item, loadingItem.Crc, callBackList.Count);

                    for (int j = 0; j < callBackList.Count; j++)
                    {
                        AsyncCallBack callBack = callBackList[j];

                        if (callBack != null && callBack.OnFinish != null && callBack.ResObj != null)
                        {
                            ResouceObj tempResObj = callBack.ResObj;
                            tempResObj.ResItem = item;
                            callBack.OnFinish(loadingItem.Path, tempResObj, tempResObj.ParamValues);
                            callBack.OnFinish = null;
                            tempResObj = null;
                        }

                        if (callBack != null && callBack.OnObjFinish != null)
                        {
                            callBack.OnObjFinish(loadingItem.Path, obj, callBack.ParamValues);
                            callBack.OnObjFinish = null;
                        }

                        callBack.Reset();
                        _AsyncCallBackPool.Recycle(callBack);
                    }

                    obj = null;
                    callBackList.Clear();
                    _LoadingAssetDic.Remove(loadingItem.Crc);

                    loadingItem.Reset();
                    _AsyncLoadResParamPool.Recycle(loadingItem);

                    if (System.DateTime.Now.Ticks - lastYieldTime > _MaxLoadResTime)
                    {
                        yield return null;
                        lastYieldTime = System.DateTime.Now.Ticks;
                        haveYield = true;
                    }

                }

                //大于 _MaxLoadResTime 时间才等待一帧
                if (!haveYield || System.DateTime.Now.Ticks - lastYieldTime > _MaxLoadResTime)
                {
                    lastYieldTime = System.DateTime.Now.Ticks;
                    yield return null;
                }
            }
        }

    }

}
