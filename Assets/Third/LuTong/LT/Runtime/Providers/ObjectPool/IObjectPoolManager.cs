/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/08/08
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;

namespace LT.ObjectPool
{
    /// <summary>
    /// 对象池管理器。
    /// </summary>
    public interface IObjectPoolManager
    {
        /// <summary>
        /// 获取对象池数量。
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// 检查是否存在对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>是否存在对象池。</returns>
        bool HasObjectPool<T>() where T : ObjectBase;

        /// <summary>
        /// 检查是否存在对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <returns>是否存在对象池。</returns>
        bool HasObjectPool(Type objectType);

        /// <summary>
        /// 检查是否存在对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <returns>是否存在对象池。</returns>
        bool HasObjectPool<T>(string name) where T : ObjectBase;

        /// <summary>
        /// 检查是否存在对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <returns>是否存在对象池。</returns>
        bool HasObjectPool(Type objectType, string name);

        /// <summary>
        /// 检查是否存在对象池。
        /// </summary>
        /// <param name="fullName">对象池完整名称。</param>
        /// <returns>是否存在对象池。</returns>
        bool HasObjectPool(string fullName);

        /// <summary>
        /// 检查是否存在对象池。
        /// </summary>
        /// <param name="condition">要检查的条件。</param>
        /// <returns>是否存在对象池。</returns>
        bool HasObjectPool(Predicate<ObjectPoolBase> condition);

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>要获取的对象池。</returns>
        IObjectPool<T> GetObjectPool<T>() where T : ObjectBase;

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <returns>要获取的对象池。</returns>
        ObjectPoolBase GetObjectPool(Type objectType);

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <returns>要获取的对象池。</returns>
        IObjectPool<T> GetObjectPool<T>(string name) where T : ObjectBase;

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <returns>要获取的对象池。</returns>
        ObjectPoolBase GetObjectPool(Type objectType, string name);

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <param name="fullName">对象池完整名称。</param>
        /// <returns>要获取的对象池。</returns>
        ObjectPoolBase GetObjectPool(string fullName);

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <param name="condition">要检查的条件。</param>
        /// <returns>要获取的对象池。</returns>
        ObjectPoolBase GetObjectPool(Predicate<ObjectPoolBase> condition);

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <param name="condition">要检查的条件。</param>
        /// <returns>要获取的对象池。</returns>
        ObjectPoolBase[] GetObjectPools(Predicate<ObjectPoolBase> condition);

        /// <summary>
        /// 获取所有对象池。
        /// </summary>
        /// <returns>所有对象池。</returns>
        ObjectPoolBase[] GetAllObjectPools();

        /// <summary>
        /// 获取所有对象池。
        /// </summary>
        /// <param name="sort">是否根据对象池的优先级排序。</param>
        /// <returns>所有对象池。</returns>
        ObjectPoolBase[] GetAllObjectPools(bool sort);

        /// <summary>
        /// 创建仅单次获取对象的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>要创建的对象池。</returns>
        IObjectPool<T> CreateSingle<T>() where T : ObjectBase;

        /// <summary>
        /// 创建仅单次获取对象的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <returns>要创建的对象池。</returns>
        ObjectPoolBase CreateSingle(Type objectType);

        /// <summary>
        /// 创建仅单次获取对象的对象池。
        /// </summary>
        /// <typeparam name="T">对象池类型</typeparam>
        /// <param name="name">对象池名称</param>
        /// <param name="capacity">对象池容量</param>
        /// <param name="autoReleaseInterval">对象池释放间隔秒数</param>
        /// <param name="expireTime">池内对象过期秒数</param>
        /// <param name="priority">对象池优先级,值越小优先级越高</param>
        /// <returns>对象池</returns>
        IObjectPool<T> CreateSingle<T>(string name, int capacity = int.MaxValue, float autoReleaseInterval = float.MaxValue, float expireTime = float.MaxValue, int priority = 0) where T : ObjectBase;

        /// <summary>
        ///  创建仅单次获取对象的对象池。
        /// </summary>
        /// <param name="objectType">对象池类型</param>
        /// <param name="name">对象池名称</param>
        /// <param name="capacity">对象池容量</param>
        /// <param name="autoReleaseInterval">对象池释放间隔秒数</param>
        /// <param name="expireTime">池内对象过期秒数</param>
        /// <param name="priority">对象池优先级,值越小优先级越高</param>
        /// <returns>对象池</returns>
        ObjectPoolBase CreateSingle(Type objectType, string name, int capacity = int.MaxValue, float autoReleaseInterval = float.MaxValue, float expireTime = float.MaxValue, int priority = 0);

        /// <summary>
        /// 创建允许重复获取对象的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>要创建的对象池。</returns>
        IObjectPool<T> CreateRepeat<T>() where T : ObjectBase;

        /// <summary>
        /// 创建允许重复获取对象的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <returns>要创建的对象池。</returns>
        ObjectPoolBase CreateRepeat(Type objectType);

        /// <summary>
        /// 创建允许重复获取对象的对象池。
        /// </summary>
        /// <typeparam name="T">对象池类型</typeparam>
        /// <param name="name">对象池名称</param>
        /// <param name="capacity">对象池容量</param>
        /// <param name="autoReleaseInterval">对象池释放间隔秒数</param>
        /// <param name="expireTime">池内对象过期秒数</param>
        /// <param name="priority">对象池优先级,值越小优先级越高</param>
        /// <returns>对象池</returns>
        IObjectPool<T> CreateRepeat<T>(string name, int capacity = int.MaxValue, float autoReleaseInterval = float.MaxValue, float expireTime = float.MaxValue, int priority = 0) where T : ObjectBase;

        /// <summary>
        /// 创建允许重复获取对象的对象池。
        /// </summary>
        /// <param name="objectType">对象池类型</param>
        /// <param name="name">对象池名称</param>
        /// <param name="capacity">对象池容量</param>
        /// <param name="autoReleaseInterval">对象池释放间隔秒数</param>
        /// <param name="expireTime">池内对象过期秒数</param>
        /// <param name="priority">对象池优先级,值越小优先级越高</param>
        /// <returns>对象池</returns>
        ObjectPoolBase CreateRepeat(Type objectType, string name, int capacity = int.MaxValue, float autoReleaseInterval = float.MaxValue, float expireTime = float.MaxValue, int priority = 0);

        /// <summary>
        /// 销毁对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>是否销毁对象池成功。</returns>
        bool DestroyObjectPool<T>() where T : ObjectBase;

        /// <summary>
        /// 销毁对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <returns>是否销毁对象池成功。</returns>
        bool DestroyObjectPool(Type objectType);

        /// <summary>
        /// 销毁对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">要销毁的对象池名称。</param>
        /// <returns>是否销毁对象池成功。</returns>
        bool DestroyObjectPool<T>(string name) where T : ObjectBase;

        /// <summary>
        /// 销毁对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">要销毁的对象池名称。</param>
        /// <returns>是否销毁对象池成功。</returns>
        bool DestroyObjectPool(Type objectType, string name);

        /// <summary>
        /// 销毁对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="objectPool">要销毁的对象池。</param>
        /// <returns>是否销毁对象池成功。</returns>
        bool DestroyObjectPool<T>(IObjectPool<T> objectPool) where T : ObjectBase;

        /// <summary>
        /// 销毁对象池。
        /// </summary>
        /// <param name="objectPool">要销毁的对象池。</param>
        /// <returns>是否销毁对象池成功。</returns>
        bool DestroyObjectPool(ObjectPoolBase objectPool);

        /// <summary>
        /// 释放对象池中的可释放对象。
        /// </summary>
        void Release();

        /// <summary>
        /// 释放对象池中的所有未使用对象。
        /// </summary>
        void ReleaseAllUnused();
    }
}