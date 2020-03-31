/*
 *    描述:
 *          1.
 *
 *    开发人: 邓平
 */

using UnityEngine;

namespace LtFramework.ResKit
{
    public static class ObjMgr
    {
        /// <summary>
        /// 获得对象
        /// </summary>
        /// <param name="path">对象路径</param>
        /// <param name="setCreateNode">设置创建节点</param>
        /// <returns></returns>
        public static GameObject Spawn(string path, bool setCreateNode = false)
        {
            return ObjManager.Spawn(path, setCreateNode);
        }

        public static void SpawnAsync(string path, OnAsyncObjFinish onFinish, LoadResPriority priority,
            bool setCreateNode = false, params object[] paramValues)
        {
            ObjManager.SpawnAsync(path, onFinish, priority, setCreateNode, paramValues);
        }

        /// <summary>
        /// 预加载GameObject
        /// </summary>
        /// <param UIName="path">路径</param>
        /// <param UIName="count">预加载个数</param>
        /// <param UIName="clear">跳场景是否清除</param>
        public static void PreSpawn(string path, int count = 1, bool clearChangeScene = false)
        {
            ObjManager.PreSpawn(path, count, clearChangeScene);
        }

        /// <summary>
        /// 取消异步加载
        /// </summary>
        /// <param UIName="guid"></param>
        public static void CancleAsyncLoad(long guid)
        {
            ObjManager.CancleAsyncLoad(guid);
        }

        /// <summary>
        /// 是否正在异步加载
        /// </summary>
        /// <param UIName="guid"></param>
        /// <returns></returns>
        public static bool IsAsyncLoading(long guid)
        {
            return ObjManager.IsAsyncLoading(guid);
        }

        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="destoryCache">情况缓存</param>
        /// <param name="recycleParent">是否放到回收节点</param>
        /// <param UIName="clearChangeScene">跳场景是否清除</param>
        /// <param name="maxCacheCount">设置缓存最大值, 等于0不缓存  小于0缓存无上限</param>
        public static void Release(GameObject obj, bool destoryCache = true, bool recycleParent = true,
            bool clearChangeScene = true, int maxCacheCount = -1)
        {
            obj.transform.localScale = Vector3.one;
            ObjManager.Release(obj, destoryCache, recycleParent, clearChangeScene, maxCacheCount);
        }


        /// <summary>
        /// 清空 Recycle对象
        /// </summary>
        /// <param name="clearChangeScene">是否要清除 切换场景不销毁的资源</param>
        public static void ClearRecycleObj(bool clearChangeScene = false)
        {
            ObjManager.ClearRecycle(clearChangeScene);
        }

        /// <summary>
        /// 判断对象是否是ObjMgr创建创建
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsObjMgrSpawn(GameObject obj)
        {
            return ObjMgr.IsObjMgrSpawn(obj);
        }
    }
}
