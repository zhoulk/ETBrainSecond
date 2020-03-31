/*
 *    描述:
 *          1.资源助手
 *
 *    开发人: 邓平
 */

using System;
using UnityEngine;

namespace LtFramework.ResKit
{
    public class AssetHelper:IDisposable
    {
        ~AssetHelper()
        {
            //Dispose();
            ResManager.Instance.ClearAssetCache(GetHashCode());
        }

        public void Dispose()
        {
            ResManager.Instance.ClearAssetCache(GetHashCode());
        }

        #region ResMgrAPI

        /// <summary>
        /// 同步资源加载
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源路径</param>
        /// <returns>资源对象</returns>
        public T LoadRes<T>(string path) where T : UnityEngine.Object
        {
            return ResManager.Instance.LoadResource<T>(path, GetHashCode());
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源路径</param>
        /// <param name="onFinish">完成回调</param>
        /// <param name="priority">资源加载优先级</param>
        /// <param name="paramValues">参数列表</param>
        public void AsyncLoadRes<T>(string path, OnAsyncObjFinish onFinish,
            LoadResPriority priority = LoadResPriority.Res_Slow, params object[] paramValues)
            where T : UnityEngine.Object
        {
            ResManager.Instance.AsyncLoadResourceAsset<T>(path, onFinish, GetHashCode(), priority, paramValues);
        }

        /// <summary>
        /// 释放资源
        ///     释放时要置空资源引用
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="destroyRes">是否清空缓存</param>
        public void ReleaseRes(string path, bool destroyRes = true)
        {
            ResManager.Instance.ReleaseResourceAsset(path,GetHashCode(), destroyRes);
        }

        /// <summary>
        /// 释放资源
        ///     释放时要置空资源引用
        /// </summary>
        /// <param name="obj">资源对象</param>
        /// <param name="destroyRes">是否清空缓存</param>
        public void ReleaseRes(UnityEngine.Object obj, bool destroyRes = true)
        {
            ResManager.Instance.ReleaseResourceAsset(obj,GetHashCode(), destroyRes);
        }

        /// <summary>
        /// 资源预加载
        /// </summary>
        /// <param name="path"></param>
        public void PreloadRes<T>(string path) where T : UnityEngine.Object
        {
            ResManager.Instance.PreloadResourceAsset<T>(path,GetHashCode());
        }

        /// <summary>
        /// 清空所有加载的Res资源
        /// </summary>
        public void ReleaseAllRes()
        {
            ResManager.Instance.ClearCacheAsset(GetHashCode());
        }

        #endregion

        #region ObjMgrAPI


        /// <summary>
        /// 获得对象
        /// </summary>
        /// <param name="path">对象路径</param>
        /// <param name="setCreateNode">设置创建节点</param>
        /// <returns></returns>
        public GameObject Spawn(string path, bool setCreateNode = false)
        {
            return ObjManager.Instance.InstantiateObjectAsset(path, GetHashCode(), true, setCreateNode);
        }

        /// <summary>
        /// 异步获得对象
        /// </summary>
        /// <param name="path">对象路径</param>
        /// <param name="onFinish">获得回调</param>
        /// <param name="priority">加载优先级</param>
        /// <param name="setCreateNode">设置创建节点</param>
        /// <param name="paramValues">回调参数</param>
        public long SpawnAsync(string path, OnAsyncObjFinish onFinish,
            LoadResPriority priority = LoadResPriority.Res_Middle,
            bool setCreateNode = false, params object[] paramValues)
        {
            return ObjManager.Instance.InstantiateObjectAsyncAsset(path, GetHashCode(), onFinish, priority,
                setCreateNode, paramValues);
        }

        /// <summary>
        /// 预加载GameObject
        /// </summary>
        /// <param UIName="path">路径</param>
        /// <param UIName="count">预加载个数</param>
        /// <param UIName="clearChangeScene">跳场景是否清除</param>
        public void PreSpawn(string path, int count = 1, bool clearChangeScene = false)
        {
            ObjManager.Instance.PreLoadGameObjectAsset(path, GetHashCode(), count, clearChangeScene);
        }


        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="destoryCache">情况缓存</param>
        /// <param name="recycleParent">是否放到回收节点</param>
        /// <param UIName="clearChangeScene">跳场景是否清除</param>
        /// <param name="maxCacheCount">设置缓存最大值, 等于0不缓存  小于0缓存无上限</param>

        public void Release(GameObject obj, bool destoryCache = true, bool recycleParent = true,
            bool clearChangeScene = true, int maxCacheCount = 0)
        {
            ObjManager.Instance.ReleaseObjectAsset(obj,GetHashCode(), destoryCache, recycleParent, clearChangeScene, maxCacheCount);
        }

        /// <summary>
        /// 情况缓存
        /// </summary>
        public void ClearRecycleObj(bool clearChangeScene = false)
        {
            ObjManager.Instance.ClearCacheAsset(GetHashCode(),clearChangeScene);
        }

        /// <summary>
        /// 取消异步加载
        /// </summary>
        /// <param UIName="guid"></param>
        public void CancleAsyncLoad(long guid)
        {
            ObjManager.Instance.CancleLoadAsset(GetHashCode(),guid);
        }

        /// <summary>
        /// 是否正在异步加载
        /// </summary>
        /// <param UIName="guid"></param>
        /// <returns></returns>
        public bool IsAsyncLoading(long guid)
        {
            return ObjManager.IsAsyncLoading(guid);
        }

        #endregion
    }
}
