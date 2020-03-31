/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/10/08
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;

namespace LT
{
    /// <summary>
    /// Unity 资源加载工具类
    /// </summary>
    public static class ResourceUtils
    {
        public static Object Load(string path)
        {
            return Resources.Load(path);
        }

        public static T Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        public static T[] LoadAll<T>(string path) where T : Object
        {
            return Resources.LoadAll<T>(path);
        }

        public static Object[] LoadAll(string path)
        {
            return Resources.LoadAll(path);
        }

        public static ResourceRequest LoadAsync<T>(string path) where T : Object
        {
            return Resources.LoadAsync<T>(path);
        }

        public static ResourceRequest LoadAsync(string path)
        {
            return Resources.LoadAsync(path);
        }

        public static ResourceRequest LoadAsync(string path, System.Type type)
        {
            return Resources.LoadAsync(path, type);
        }

        public static void UnloadAsset(Object assetToUnload)
        {
            Resources.UnloadAsset(assetToUnload);
        }

        public static AsyncOperation UnloadUnusedAssets()
        {
            return Resources.UnloadUnusedAssets();
        }
    }
}