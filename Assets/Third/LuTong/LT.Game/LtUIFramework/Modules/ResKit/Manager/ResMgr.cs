/*
 *    描述:
 *          1.
 *
 *    开发人: 邓平
 */

namespace LtFramework.ResKit
{
    public static class ResMgr 
    {

        #region API

        /// <summary>
        /// 同步资源加载
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源路径</param>
        /// <returns>资源对象</returns>
        public static T LoadRes<T>(string path) where T : UnityEngine.Object
        {
            return ResManager.LoadRes<T>(path);
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源路径</param>
        /// <param name="onFinish">完成回调</param>
        /// <param name="priority">资源加载优先级</param>
        /// <param name="paramValues">参数列表</param>
        public static void AsyncLoadRes<T>(string path, OnAsyncObjFinish onFinish,
            LoadResPriority priority = LoadResPriority.Res_Slow, params object[] paramValues)
            where T : UnityEngine.Object
        {
             ResManager.AsyncLoadRes<T>(path,onFinish,priority,paramValues);
        }

        /// <summary>
        /// 释放资源
        ///     释放时要置空资源引用
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="destroyRes">是否清空缓存</param>
        public static void ReleaseRes(string path, bool destroyRes = true)
        {
            ResManager.ReleaseRes(path, destroyRes);
        }

        /// <summary>
        /// 释放资源
        ///     释放时要置空资源引用
        /// </summary>
        /// <param name="obj">资源对象</param>
        /// <param name="destroyRes">是否清空缓存</param>
        public static void ReleaseRes(UnityEngine.Object obj, bool destroyRes = true)
        {
            ResManager.ReleaseRes(obj, destroyRes);
        }

        /// <summary>
        /// 资源预加载
        /// </summary>
        /// <param name="path"></param>
        public static void PreloadRes<T>(string path) where T : UnityEngine.Object
        {
            ResManager.PreloadRes<T>(path);
        }

        /// <summary>
        /// 清空 跳场景清掉 的缓存
        /// </summary>
        public static void Clear()
        {
            ResManager.Clear();
        }
        #endregion

    }
}
