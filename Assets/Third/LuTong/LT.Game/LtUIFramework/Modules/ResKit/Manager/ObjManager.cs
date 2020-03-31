/*
 *    描述:
 *          1. Object资源管理类
 *
 *    开发人: 邓平
 */
using LtFramework.Util;
using LtFramework.Util.Pools;
using LtFramework.Util.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LtFramework.ResKit
{
    public class ObjManager : ISingleton
    {
        #region 缓存

        //对象池释放节点
        private Transform _RecycleNode;

        //对象创建节点
        private Transform _CreateNode;

        //对象池
        private readonly Dictionary<uint, List<ResouceObj>> _ObjectPoolDic = new Dictionary<uint, List<ResouceObj>>();

        //暂存ResObj的Dic
        protected readonly Dictionary<int, ResouceObj> _ResourceObjDic = new Dictionary<int, ResouceObj>();

        //ResouceObj 类对象池
        //protected readonly ClassObjectPool<ResouceObj> _ResourceObjClassPool;

        //根据异步的guid存储ResourceObj,来办的是否正在异步加载
        protected readonly Dictionary<long, ResouceObj> _AsyncResObjs = new Dictionary<long, ResouceObj>();


        #endregion

        #region 单例

        internal static ObjManager Instance => SingletonProperty<ObjManager>.Instance;

        public void Dispose()
        {
            SingletonProperty<ObjManager>.Dispose();
        }

        private ObjManager()
        {
            //_ResourceObjClassPool = GetOrCreateClassPool<ResouceObj>(200);
            MonoRes monoRes = MonoRes.Instance;

            GameObject recycleNode = new GameObject("RecycleNode");
            recycleNode.transform.SetParent(monoRes.transform, true);
            recycleNode.gameObject.SetActive(false);
            GameObject createNode = new GameObject("CreateNode");
            createNode.transform.SetParent(monoRes.transform, true);
            _RecycleNode = recycleNode.transform;
            _CreateNode = createNode.transform;
            
        }

        public void OnSingletonInit()
        {

        }



        #endregion

        #region API

        /// <summary>
        /// 获得对象
        /// </summary>
        /// <param name="path">对象路径</param>
        /// <param name="setCreateNode">设置创建节点</param>
        /// <returns></returns>
        internal static GameObject Spawn(string path, bool setCreateNode = false)
        {
            return Instance.InstantiateObject(path,true, setCreateNode);
        }

        /// <summary>
        /// 异步获得对象
        /// </summary>
        /// <param name="path">对象路径</param>
        /// <param name="onFinish">获得回调</param>
        /// <param name="priority">加载优先级</param>
        /// <param name="setCreateNode">设置创建节点</param>
        /// <param name="paramValues">回调参数</param>
        internal static long SpawnAsync(string path, OnAsyncObjFinish onFinish, LoadResPriority priority = LoadResPriority.Res_Middle,
            bool setCreateNode = false, params object[] paramValues)
        {
            return Instance.InstantiateObjectAsync(path, onFinish, priority, setCreateNode, paramValues);
        }

        /// <summary>
        /// 预加载GameObject
        /// </summary>
        /// <param UIName="path">路径</param>
        /// <param UIName="count">预加载个数</param>
        /// <param UIName="clearChangeScene">跳场景是否清除</param>
        internal static void PreSpawn(string path, int count = 1, bool clearChangeScene = false)
        {
            Instance.PreLoadGameObject(path,count,clearChangeScene);
        }


        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="destoryCache">情况缓存</param>
        /// <param name="recycleParent">是否放到回收节点</param>
        /// <param UIName="clearChangeScene">跳场景是否清除</param>
        /// <param name="maxCacheCount">设置缓存最大值, 等于0不缓存  小于0缓存无上限</param>

        internal static void Release(GameObject obj, bool destoryCache = true, bool recycleParent = true,
            bool clearChangeScene = true, int maxCacheCount = 0)
        {
            Instance.ReleaseObject(obj, destoryCache, recycleParent, clearChangeScene, maxCacheCount);
        }

        /// <summary>
        /// 情况缓存
        /// </summary>
        internal static void ClearRecycle(bool clearChangeScene)
        {
            Instance.ClearCache(clearChangeScene);
        }

        /// <summary>
        /// 该对象是否是对象池创建
        /// </summary>
        /// <param name="obj"></param>
        internal static void IsObjMgrSpawn(GameObject obj)
        {
            Instance.IsObjectManagerCreat(obj);
        }

        /// <summary>
        /// 取消异步加载
        /// </summary>
        /// <param UIName="guid"></param>
        internal static void CancleAsyncLoad(long guid)
        {
            Instance.CancleLoad(guid);
        }

        /// <summary>
        /// 是否正在异步加载
        /// </summary>
        /// <param UIName="guid"></param>
        /// <returns></returns>
        internal static bool IsAsyncLoading(long guid)
        {
            return Instance.IsAsyncLoad(guid);
        }

        #endregion

        #region 程序集方法


        /// <summary>
        /// 清空对象池
        /// </summary>
        private void ClearCache(bool clearChangeScene)
        {
            uint[] clearKeys = _ObjectPoolDic.Keys.ToArray();
            List<uint> tempList = new List<uint>();
            foreach (uint key in clearKeys)
            {
                List<ResouceObj> st = _ObjectPoolDic[key];
                for (int i = st.Count - 1; i >= 0; i--)
                {
                    ResouceObj resObj = st[i];
                    if (!System.Object.ReferenceEquals(resObj.CloneObj, null))
                    {
                        if (!clearChangeScene && resObj.ClearByChangeScene)
                        {
                            continue;
                        }
                        _ResourceObjDic.Remove(resObj.CloneObj.GetInstanceID());
                        ResManager.Instance.ReleaseResource(resObj, i, true);
                        //_ResourceObjClassPool.Recycle(resObj);
                        st.Remove(resObj);
                    }
                }

                if (st.Count <= 0)
                {
                    tempList.Add(key);
                }
            }

            for (int i = 0; i < tempList.Count; i++)
            {
                uint temp = tempList[i];
                if (_ObjectPoolDic.ContainsKey(temp))
                {
                    _ObjectPoolDic.Remove(temp);
                }
            }
            
            tempList.Clear();
        }


        /// <summary>
        /// 清空对象池
        /// </summary>
        internal void ClearCacheAsset(int assetHelpHash,bool clearChangeScene)
        {
            uint[] clearKeys = _ObjectPoolDic.Keys.ToArray();
            List<uint> tempList = new List<uint>();
            foreach (uint key in clearKeys)
            {
                List<ResouceObj> st = _ObjectPoolDic[key];
                for (int i = st.Count - 1; i >= 0; i--)
                {
                    ResouceObj resObj = st[i];
                    if (!System.Object.ReferenceEquals(resObj.CloneObj, null))
                    {
                        if (!clearChangeScene && resObj.ClearByChangeScene)
                        {
                            continue;
                        }
                        _ResourceObjDic.Remove(resObj.CloneObj.GetInstanceID());
                        ResManager.Instance.ReleaseResourceAsset(resObj,assetHelpHash, i, true);
                        //_ResourceObjClassPool.Recycle(resObj);
                        st.Remove(resObj);
                    }
                }

                if (st.Count <= 0)
                {
                    tempList.Add(key);
                }
            }

            for (int i = 0; i < tempList.Count; i++)
            {
                uint temp = tempList[i];
                if (_ObjectPoolDic.ContainsKey(temp))
                {
                    _ObjectPoolDic.Remove(temp);
                }
            }
            tempList.Clear();
        }


        /// <summary>
        /// 清除某个资源在对象池中的所有对象
        /// </summary>
        /// <param UIName="crc"></param>
        internal void ClearPoolObject(uint crc)
        {
            List<ResouceObj> st = null;
            if (!_ObjectPoolDic.TryGetValue(crc, out st) || st == null)
            {
                return;
            }

            for (int i = st.Count - 1; i >= 0; i--)
            {
                ResouceObj resObj = st[i];
                if (resObj.ClearByChangeScene)
                {
                    st.Remove(resObj);
                    int tempID = resObj.CloneObj.GetInstanceID();
                    GameObject.Destroy(resObj.CloneObj);
                    _ResourceObjDic.Remove(tempID);
                    //_ResourceObjClassPool.Recycle(resObj);
                }

            }

            if (st.Count <= 0)
            {
                _ObjectPoolDic.Remove(crc);
            }

        }

        /// <summary>
        /// 从对象池中取对象
        /// </summary>
        /// <param UIName="crc"></param>
        /// <returns></returns>
        private ResouceObj GetObjectFromPool(uint crc)
        {
            List<ResouceObj> st = null;
            if (_ObjectPoolDic.TryGetValue(crc, out st) && st != null && st.Count > 0)
            {
                ResouceObj resObj = st[0];
                st.RemoveAt(0);
                GameObject obj = resObj.CloneObj;



                if (!System.Object.ReferenceEquals(obj, null))
                {
                    resObj.AlreadyRelease = false;
                    //编辑器下改名
#if UNITY_EDITOR
                    if (!obj)
                    {
                        string error = string.Format("该对象InstanceID[{0}]已经被销毁且没有通过对象池销毁 请检测", resObj.ResItem.Guid);
                        throw new Exception(error);
                    }

                    if (obj.name.EndsWith("(Recycle)"))
                    {
                        obj.name = obj.name.Replace("(Recycle)", "");
                    }
#endif
                }

                return resObj;
            }

            return null;
        }

        /// <summary>
        /// 取消异步加载
        /// </summary>
        /// <param UIName="guid"></param>
        private void CancleLoad(long guid)
        {
            ResouceObj resObj = null;
            if (_AsyncResObjs.TryGetValue(guid, out resObj) && ResManager.Instance.CancleLoad(resObj))
            {
                _AsyncResObjs.Remove(guid);
                //_ResourceObjClassPool.Recycle(resObj);
            }
        }

        internal void CancleLoadAsset(int assetHelpHash,long guid)
        {
            ResouceObj resObj = null;
            if (_AsyncResObjs.TryGetValue(guid, out resObj) && ResManager.Instance.CancleLoad(resObj))
            {
                ResManager.Instance.RemoveObjAsset(resObj.Crc,assetHelpHash);
                _AsyncResObjs.Remove(guid);
                //_ResourceObjClassPool.Recycle(resObj);
            }
        }

        /// <summary>
        /// 是否正在异步加载
        /// </summary>
        /// <param UIName="guid"></param>
        /// <returns></returns>
        private bool IsAsyncLoad(long guid)
        {
            return _AsyncResObjs[guid] != null;
        }

        /// <summary>
        /// 该对象是否是对象池创建的
        /// </summary>
        /// <returns></returns>
        private bool IsObjectManagerCreat(GameObject obj)
        {
            ResouceObj resObj = _ResourceObjDic[obj.GetInstanceID()];
            return resObj != null;
        }

        /// <summary>
        /// 预加载GameObject
        /// </summary>
        /// <param UIName="path">路径</param>
        /// <param UIName="count">预加载个数</param>
        /// <param UIName="clear">跳场景是否清除</param>
        private void PreLoadGameObject(string path, int count = 1, bool clearChangeScene = false)
        {
            List<GameObject> tempGameObjectList = new List<GameObject>();
            for (int i = 0; i < count; i++)
            {
                GameObject obj = InstantiateObject(path, clearChangeScene, true);
                tempGameObjectList.Add(obj);
            }

            for (int i = 0; i < count; i++)
            {
                GameObject obj = tempGameObjectList[i];
                ReleaseObject(obj, false, true, clearChangeScene, count);
                obj = null;
            }

            tempGameObjectList.Clear();
        }

        /// <summary>
        /// 预加载GameObject
        /// </summary>
        /// <param UIName="path">路径</param>
        /// <param UIName="count">预加载个数</param>
        /// <param UIName="clear">跳场景是否清除</param>
        internal void PreLoadGameObjectAsset(string path, int assetHelpHash, int count = 1,
            bool clearChangeScene = false)
        {
            List<GameObject> tempGameObjectList = new List<GameObject>();
            for (int i = 0; i < count; i++)
            {
                GameObject obj = InstantiateObjectAsset(path, assetHelpHash, false, clearChangeScene);
                tempGameObjectList.Add(obj);
            }

            for (int i = 0; i < count; i++)
            {
                GameObject obj = tempGameObjectList[i];
                ReleaseObjectAsset(obj, assetHelpHash, false, true, clearChangeScene, count);
                obj = null;
            }

            tempGameObjectList.Clear();
        }

        private GameObject InstantiateObject(string path, uint crc, ResLoadMode mode, bool clearChangeScene,
            bool setCreateNode)
        {
            ResouceObj resouceObj = null;
            try
            {
                resouceObj = GetObjectFromPool(crc);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Debug.LogError("对象路径 Path :" + path);
            }

            if (resouceObj == null)
            {
                resouceObj = new ResouceObj();// _ResourceObjClassPool.Spawn(true);
                resouceObj.Crc = crc;
                resouceObj.ClearByChangeScene = clearChangeScene;
                //ResouceManager提供加载方法
                resouceObj = ResManager.Instance.LoadRessource(path, mode, resouceObj);
                if (resouceObj.ResItem.Obj != null)
                {
                    resouceObj.CloneObj = GameObject.Instantiate(resouceObj.ResItem.Obj) as GameObject;
                }
            }

            if (setCreateNode)
            {
                resouceObj.CloneObj.transform.SetParent(_CreateNode, false);
            }

            resouceObj.Guid = resouceObj.CloneObj.GetInstanceID();
            if (!_ResourceObjDic.ContainsKey(resouceObj.Guid))
            {
                _ResourceObjDic.Add(resouceObj.Guid, resouceObj);
            }

            //引用计数 加一
            resouceObj.ResItem.RefCount.Increase();
            return resouceObj.CloneObj;
        }

        /// <summary>
        /// 同步加载加载对象
        /// </summary>
        /// <param name="path">对象路径</param>
        /// <param name="clearChangeScene">跳场景是否清除</param>
        /// <param name="setCreateNode">是否设置创建节点</param>
        /// <returns></returns>
        private GameObject InstantiateObject(string path, bool clearChangeScene, bool setCreateNode)
        {
            ResLoadMode mode = ResManager.Instance.LoadMode(ref path);
            uint crc = Crc32.GetCrc32(path);
            return InstantiateObject(path, crc, mode, clearChangeScene, setCreateNode);
        }

        internal GameObject InstantiateObjectAsset(string path, int assetHelpHash, bool clearChangeScene,
            bool setCreateNode)
        {
            ResLoadMode mode = ResManager.Instance.LoadMode(ref path);
            uint crc = Crc32.GetCrc32(path);
            ResManager.Instance.AddObjAsset(crc, assetHelpHash);
            return InstantiateObject(path, crc, mode, clearChangeScene, setCreateNode);
        }


        /// <summary>
        /// 异步对象加载
        /// </summary>
        /// <param UIName="path"></param>
        /// <param UIName="fealFinish"></param>
        /// <param UIName="priority"></param>
        /// <param UIName="setSceneObject"></param>
        /// <param UIName="param1"></param>
        /// <param UIName="param2"></param>
        /// <param UIName="param3"></param>
        /// <param UIName="clearChangeScene"></param>
        private long InstantiateObjectAsync(string path,uint crc,ResLoadMode mode, OnAsyncObjFinish onFinish, LoadResPriority priority,
            bool setCreateNode = false, params object[] paramValues)
        {
            ResouceObj resObj = GetObjectFromPool(crc);
            if (resObj != null)
            {
                if (setCreateNode)
                {
                    resObj.CloneObj.transform.SetParent(_CreateNode, false);
                }

                if (onFinish != null)
                {
                    resObj.ResItem.RefCount.Increase();
                    onFinish(path, resObj.CloneObj, paramValues);
                }

                return resObj.Guid;
            }

            long guid = ResManager.CreateGuid();

            resObj = new ResouceObj();// _ResourceObjClassPool.Spawn(true);
            resObj.Crc = crc;
            resObj.SetParent = setCreateNode;
            resObj.OnFinish = onFinish;
            if (paramValues != null && paramValues.Length > 0)
            {
                foreach (object value in paramValues)
                {
                    resObj.ParamList.Add(value);
                }
            }
            //调用ResourceManagerd的异步加载接口
            ResManager.Instance.AsyncLoadResource(path, mode, resObj, OnLoadResourceObjFinish, priority);
            return guid;
        }


        /// <summary>
        /// 异步对象加载
        /// </summary>
        /// <param UIName="path"></param>
        /// <param UIName="fealFinish"></param>
        /// <param UIName="priority"></param>
        /// <param UIName="setSceneObject"></param>
        /// <param UIName="param1"></param>
        /// <param UIName="param2"></param>
        /// <param UIName="param3"></param>
        /// <param UIName="clearChangeScene"></param>
        private long InstantiateObjectAsync(string path, OnAsyncObjFinish onFinish, LoadResPriority priority,
            bool setCreateNode = false, params object[] paramValues)
        {
            if (string.IsNullOrEmpty(path))
            {
                return 0;
            }
            ResLoadMode mode = ResManager.Instance.LoadMode(ref path);
            uint crc = Crc32.GetCrc32(path);

            return InstantiateObjectAsync(path, crc, mode, onFinish, priority, setCreateNode, paramValues);
        }

        /// <summary>
        /// 异步对象加载
        /// </summary>
        /// <param UIName="path"></param>
        /// <param UIName="fealFinish"></param>
        /// <param UIName="priority"></param>
        /// <param UIName="setSceneObject"></param>
        /// <param UIName="param1"></param>
        /// <param UIName="param2"></param>
        /// <param UIName="param3"></param>
        /// <param UIName="clearChangeScene"></param>
        internal long InstantiateObjectAsyncAsset(string path,int assetHelpHash, OnAsyncObjFinish onFinish, LoadResPriority priority,
            bool setCreateNode = false, params object[] paramValues)
        {
            if (string.IsNullOrEmpty(path))
            {
                return 0;
            }
            ResLoadMode mode = ResManager.Instance.LoadMode(ref path);
            uint crc = Crc32.GetCrc32(path);
            ResManager.Instance.AddObjAsset(crc, assetHelpHash);
            return InstantiateObjectAsync(path, crc, mode, onFinish, priority, setCreateNode, paramValues);
        }



        /// <summary>
        /// 资源加载完成回调
        /// </summary>
        /// <param UIName="path"></param>
        /// <param UIName="resObj"></param>
        /// <param UIName="param1"></param>
        /// <param UIName="param2"></param>
        /// <param UIName="param3"></param>
        private void OnLoadResourceObjFinish(string path, ResouceObj resObj, params object[] paramValues)
        {
            if (resObj == null)
            {
                return;
            }

            if (resObj.ResItem.Obj == null)
            {
#if UNITY_EDITOR
                Debug.LogError("异步资源加载的资源为空 Path :" + path);
#endif
            }
            else
            {
                resObj.CloneObj = GameObject.Instantiate(resObj.ResItem.Obj) as GameObject;
            }

            //加载完成就重正在加载的异步中移除
            if (_AsyncResObjs.ContainsKey(resObj.Guid))
            {
                _AsyncResObjs.Remove(resObj.Guid);
            }


            if (resObj.CloneObj != null && resObj.SetParent)
            {
                resObj.CloneObj.transform.SetParent(_CreateNode, false);
            }

            if (resObj.OnFinish != null)
            {
                int tempID = resObj.CloneObj.GetInstanceID();
                if (!_ResourceObjDic.ContainsKey(tempID))
                {
                    _ResourceObjDic.Add(tempID, resObj);
                }
                resObj.ResItem.RefCount.Increase();
                resObj.OnFinish(path, resObj.CloneObj, resObj.ParamValues);
            }
        }

        /// <summary>
        /// 获得对象Crc
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private uint GetReleseCrc(GameObject obj)
        {
            if (obj == null)
            {
                return 0;
            }

            ResouceObj resObj = null;

            int tempID = obj.GetInstanceID();
            if (!_ResourceObjDic.TryGetValue(tempID, out resObj))
            {
                Debug.Log("这个对象不是ObjectManager创建");
                return 0;
            }
            if (resObj == null)
            {
                Debug.LogError("缓存的ResourceObj为空");
                return 0;
            }
            else
            {
                return resObj.Crc;
            }
        }

        /// <summary>
        /// 回收资源
        /// </summary>
        /// <param UIName="obj"></param>
        /// <param UIName="maxCacheCount"></param>
        /// <param UIName="destoryCache"></param>
        /// <param UIName="recycleParent"></param>
        private void ReleaseObject(ResouceObj resObj, int InstanceID, bool destoryCache, bool recycleParent,
            bool clearChangeScene, int maxCacheCount)
        {

            List<ResouceObj> st = null;

            //引用计数减一
            resObj.ResItem.RefCount.Decrease();
            resObj.ClearByChangeScene = clearChangeScene;

            if (maxCacheCount == 0)
            {
                _ObjectPoolDic.TryGetValue(resObj.Crc, out st);
                _ResourceObjDic.Remove(InstanceID);
                int poolCount = 0;
                if (st != null)
                {
                    poolCount = st.Count;
                }

                ResManager.Instance.ReleaseResource(resObj, poolCount, destoryCache);
                //_ResourceObjClassPool.Recycle(resObj);
            }
            else
            {
                //回收到对象池
                if (!_ObjectPoolDic.TryGetValue(resObj.Crc, out st) || st == null)
                {
                    st = new List<ResouceObj>();
                    _ObjectPoolDic.Add(resObj.Crc, st);
                }

                if (resObj.CloneObj)
                {
                    if (recycleParent)
                    {
                        resObj.CloneObj.transform.SetParent(_RecycleNode);
                    }
                    else
                    {
                        resObj.CloneObj.SetActive(false);
                    }
                }

                if (maxCacheCount < 0 || st.Count < maxCacheCount)
                {
                    st.Add(resObj);
                    resObj.AlreadyRelease = true;
                }
                else
                {
                    //达到最大缓存个数
                    _ResourceObjDic.Remove(InstanceID);
                    ResManager.Instance.ReleaseResource(resObj, st.Count, destoryCache);
                    //_ResourceObjClassPool.Recycle(resObj);
                }
            }
        }


        /// <summary>
        /// 回收资源
        /// </summary>
        /// <param UIName="obj"></param>
        /// <param UIName="maxCacheCount"></param>
        /// <param UIName="destoryCache"></param>
        /// <param UIName="recycleParent"></param>
        private void ReleaseObject(GameObject obj, bool destoryCache, bool recycleParent,
            bool clearChangeScene, int maxCacheCount)
        {
            if (obj == null)
            {
                return;
            }

            ResouceObj resObj = null;

            int tempID = obj.GetInstanceID();
            if (!_ResourceObjDic.TryGetValue(tempID, out resObj))
            {
                Debug.Log("这个对象不是ObjectManager创建");
                return;
            }

            if (resObj == null)
            {
                Debug.LogError("缓存的ResourceObj为空");
            }

            if (resObj.AlreadyRelease)
            {
                Debug.LogError("该对象"+resObj.CloneObj.name+"已经被释放到对象池,检查是否清空引用 " + resObj.CloneObj.GetInstanceID());
                return;
            }
#if UNITY_EDITOR
            obj.name += "(Recycle)";
#endif
            ReleaseObject(resObj, tempID, destoryCache, recycleParent, clearChangeScene, maxCacheCount);
        }

        /// <summary>
        /// 回收资源
        /// </summary>
        /// <param UIName="obj"></param>
        /// <param UIName="maxCacheCount"></param>
        /// <param UIName="destoryCache"></param>
        /// <param UIName="recycleParent"></param>
        internal void ReleaseObjectAsset(GameObject obj,int assetHelpHash, bool destoryCache, bool recycleParent,
            bool clearChangeScene, int maxCacheCount)
        {
            if (obj == null)
            {
                return;
            }

            ResouceObj resObj = null;

            int tempID = obj.GetInstanceID();
            if (!_ResourceObjDic.TryGetValue(tempID, out resObj))
            {
                Debug.Log("这个对象不是ObjectManager创建");
                return;
            }

            if (resObj == null)
            {
                Debug.LogError("缓存的ResourceObj为空");
            }

            if (resObj.AlreadyRelease)
            {
                Debug.LogError("该对象已经被释放到对象池,检查是否清空引用");
                return;
            }
#if UNITY_EDITOR
            obj.name += "(Recycle)";
#endif
            ResManager.Instance.RemoveObjAsset(resObj.Crc, assetHelpHash);
            ReleaseObject(resObj, tempID, destoryCache, recycleParent, clearChangeScene, maxCacheCount);
        }




        #region 类对象池的使用


        protected Dictionary<Type, object> _ClassPoolDic = new Dictionary<Type, object>();

        /// <summary>
        /// 创建类对象池,创建完成后外面可以保持ClassObjectPool<T>,然后调用Spawn和Recycle来创建对象和回收对象
        /// </summary>
        /// <typeparam UIName="T"></typeparam>
        /// <param UIName="maxcount"></param>
        /// <returns></returns>
        internal ClassObjectPool<T> GetOrCreateClassPool<T>(int maxcount) where T : class, new()
        {
            Type type = typeof(T);
            object outOjb = null;
            if (!_ClassPoolDic.TryGetValue(type, out outOjb) || outOjb == null)
            {
                ClassObjectPool<T> newPool = new ClassObjectPool<T>(maxcount);
                _ClassPoolDic.Add(type, newPool);
                return newPool;
            }

            return outOjb as ClassObjectPool<T>;
        }

        /// <summary>
        /// 从对象池中取T对象
        /// </summary>
        /// <typeparam UIName="T"></typeparam>
        /// <param UIName="maxcount"></param>
        /// <returns></returns>
        internal T NewClassObjectFromPool<T>(int maxcount) where T : class, new()
        {
            ClassObjectPool<T> pool = GetOrCreateClassPool<T>(maxcount);
            if (pool == null)
            {
                return null;
            }
            else
            {
                return pool.Spawn(true);
            }

        }

        #endregion

        #endregion


    }
}
