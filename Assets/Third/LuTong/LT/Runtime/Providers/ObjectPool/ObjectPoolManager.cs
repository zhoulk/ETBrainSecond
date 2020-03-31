/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/08/08
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using LT.MonoDriver;
using System;
using System.Collections.Generic;

namespace LT.ObjectPool
{
    /// <summary>
    /// 对象池管理器。
    /// </summary>
    internal sealed partial class ObjectPoolManager : IObjectPoolManager, IUpdate, IOnDestroy
    {
        private const int DefaultCapacity = int.MaxValue;
        private const float DefaultExpireTime = float.MaxValue;
        private const float DefaultReleaseInterval = float.MaxValue;
        private const int DefaultPriority = 0;

        private readonly Dictionary<string, ObjectPoolBase> objectPools;

        /// <summary>
        /// 初始化对象池管理器的新实例。
        /// </summary>
        public ObjectPoolManager()
        {
            objectPools = new Dictionary<string, ObjectPoolBase>();
        }

        /// <inheritdoc />
        public int Count
        {
            get
            {
                return objectPools.Count;
            }
        }

        /// <inheritdoc />
        public void Update()
        {
            foreach (KeyValuePair<string, ObjectPoolBase> objectPool in objectPools)
            {
                objectPool.Value.Update();
            }
        }

        /// <inheritdoc />
        public void OnDestroy()
        {
            foreach (KeyValuePair<string, ObjectPoolBase> objectPool in objectPools)
            {
                objectPool.Value.ReleaseAll();
            }

            objectPools.Clear();
        }

        /// <summary>
        /// 检查是否存在对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>是否存在对象池。</returns>
        public bool HasObjectPool<T>() where T : ObjectBase
        {
            return HasObjectPool(Utility.Text.GetFullName<T>(string.Empty));
        }

        /// <summary>
        /// 检查是否存在对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <returns>是否存在对象池。</returns>
        public bool HasObjectPool(Type objectType)
        {
            Guard.Verify<ArgumentException>(objectType == null, "Object type is invalid.");
            Guard.Verify<ArgumentException>(!typeof(ObjectBase).IsAssignableFrom(objectType), string.Format("Object type '{0}' is invalid.", objectType.FullName));

            return HasObjectPool(Utility.Text.GetFullName(objectType, string.Empty));
        }

        /// <summary>
        /// 检查是否存在对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <returns>是否存在对象池。</returns>
        public bool HasObjectPool<T>(string name) where T : ObjectBase
        {
            return HasObjectPool(Utility.Text.GetFullName<T>(name));
        }

        /// <summary>
        /// 检查是否存在对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <returns>是否存在对象池。</returns>
        public bool HasObjectPool(Type objectType, string name)
        {
            Guard.Verify<ArgumentException>(objectType == null, "Object type is invalid.");
            Guard.Verify<ArgumentException>(!typeof(ObjectBase).IsAssignableFrom(objectType), string.Format("Object type '{0}' is invalid.", objectType.FullName));

            return HasObjectPool(Utility.Text.GetFullName(objectType, name));
        }

        /// <summary>
        /// 检查是否存在对象池。
        /// </summary>
        /// <param name="fullName">对象池完整名称。</param>
        /// <returns>是否存在对象池。</returns>
        public bool HasObjectPool(string fullName)
        {
            Guard.NotEmptyOrNull(fullName, "Full name is invalid.");

            return objectPools.ContainsKey(fullName);
        }

        /// <summary>
        /// 检查是否存在对象池。
        /// </summary>
        /// <param name="condition">要检查的条件。</param>
        /// <returns>是否存在对象池。</returns>
        public bool HasObjectPool(Predicate<ObjectPoolBase> condition)
        {
            Guard.Verify<ArgumentException>(condition == null, "Condition is invalid.");

            foreach (KeyValuePair<string, ObjectPoolBase> objectPool in objectPools)
            {
                if (condition(objectPool.Value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>要获取的对象池。</returns>
        public IObjectPool<T> GetObjectPool<T>() where T : ObjectBase
        {
            return (IObjectPool<T>)GetObjectPool(Utility.Text.GetFullName<T>(string.Empty));
        }

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <returns>要获取的对象池。</returns>
        public ObjectPoolBase GetObjectPool(Type objectType)
        {
            Guard.Verify<ArgumentException>(objectType == null, "Object type is invalid.");
            Guard.Verify<ArgumentException>(!typeof(ObjectBase).IsAssignableFrom(objectType), string.Format("Object type '{0}' is invalid.", objectType.FullName));

            return GetObjectPool(Utility.Text.GetFullName(objectType, string.Empty));
        }

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <returns>要获取的对象池。</returns>
        public IObjectPool<T> GetObjectPool<T>(string name) where T : ObjectBase
        {
            return (IObjectPool<T>)GetObjectPool(Utility.Text.GetFullName<T>(name));
        }

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <returns>要获取的对象池。</returns>
        public ObjectPoolBase GetObjectPool(Type objectType, string name)
        {
            Guard.Verify<ArgumentException>(objectType == null, "Object type is invalid.");
            Guard.Verify<ArgumentException>(!typeof(ObjectBase).IsAssignableFrom(objectType), string.Format("Object type '{0}' is invalid.", objectType.FullName));

            return GetObjectPool(Utility.Text.GetFullName(objectType, name));
        }

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <param name="fullName">对象池完整名称。</param>
        /// <returns>要获取的对象池。</returns>
        public ObjectPoolBase GetObjectPool(string fullName)
        {
            Guard.NotEmptyOrNull(fullName, "Full name is invaild.");

            ObjectPoolBase objectPool = null;
            if (objectPools.TryGetValue(fullName, out objectPool))
            {
                return objectPool;
            }

            return null;
        }

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <param name="condition">要检查的条件。</param>
        /// <returns>要获取的对象池。</returns>
        public ObjectPoolBase GetObjectPool(Predicate<ObjectPoolBase> condition)
        {
            Guard.Verify<ArgumentException>(condition == null, "Condition is invaild.");

            foreach (KeyValuePair<string, ObjectPoolBase> objectPool in objectPools)
            {
                if (condition(objectPool.Value))
                {
                    return objectPool.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <param name="condition">要检查的条件。</param>
        /// <returns>要获取的对象池。</returns>
        public ObjectPoolBase[] GetObjectPools(Predicate<ObjectPoolBase> condition)
        {
            Guard.Verify<ArgumentException>(condition == null, "Condition is invaild.");

            List<ObjectPoolBase> results = new List<ObjectPoolBase>();
            foreach (KeyValuePair<string, ObjectPoolBase> objectPool in objectPools)
            {
                if (condition(objectPool.Value))
                {
                    results.Add(objectPool.Value);
                }
            }

            return results.ToArray();
        }

        /// <summary>
        /// 获取所有对象池。
        /// </summary>
        /// <returns>所有对象池。</returns>
        public ObjectPoolBase[] GetAllObjectPools()
        {
            return GetAllObjectPools(false);
        }

        /// <summary>
        /// 获取所有对象池。
        /// </summary>
        /// <param name="sort">是否根据对象池的优先级排序。</param>
        /// <returns>所有对象池。</returns>
        public ObjectPoolBase[] GetAllObjectPools(bool sort)
        {
            if (sort)
            {
                List<ObjectPoolBase> results = new List<ObjectPoolBase>();
                foreach (KeyValuePair<string, ObjectPoolBase> objectPool in objectPools)
                {
                    results.Add(objectPool.Value);
                }

                results.Sort(ObjectPoolComparer);
                return results.ToArray();
            }
            else
            {
                int index = 0;
                ObjectPoolBase[] results = new ObjectPoolBase[objectPools.Count];
                foreach (KeyValuePair<string, ObjectPoolBase> objectPool in objectPools)
                {
                    results[index++] = objectPool.Value;
                }

                return results;
            }
        }

        /// <summary>
        /// 创建仅单次获取对象的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>要创建的对象池。</returns>
        public IObjectPool<T> CreateSingle<T>() where T : ObjectBase
        {
            return InnerCreateObjectPool<T>(string.Empty, false, DefaultCapacity, DefaultReleaseInterval, DefaultExpireTime, DefaultPriority);
        }

        /// <summary>
        /// 创建仅单次获取对象的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <returns>要创建的对象池。</returns>
        public ObjectPoolBase CreateSingle(Type objectType)
        {
            return InnerCreateObjectPool(objectType, string.Empty, false, DefaultCapacity, DefaultReleaseInterval, DefaultExpireTime, DefaultPriority);
        }

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
        public IObjectPool<T> CreateSingle<T>(string name, int capacity, float autoReleaseInterval, float expireTime, int priority) where T : ObjectBase
        {
            return InnerCreateObjectPool<T>(name, false, capacity, autoReleaseInterval, expireTime, priority);
        }

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
        public ObjectPoolBase CreateSingle(Type objectType, string name, int capacity, float autoReleaseInterval, float expireTime, int priority)
        {
            return InnerCreateObjectPool(objectType, name, false, capacity, autoReleaseInterval, expireTime, priority);
        }

        /// <summary>
        /// 创建允许重复获取对象的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>要创建的对象池。</returns>
        public IObjectPool<T> CreateRepeat<T>() where T : ObjectBase
        {
            return InnerCreateObjectPool<T>(string.Empty, true, DefaultCapacity, DefaultReleaseInterval, DefaultExpireTime, DefaultPriority);
        }

        /// <summary>
        /// 创建允许重复获取对象的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <returns>要创建的对象池。</returns>
        public ObjectPoolBase CreateRepeat(Type objectType)
        {
            return InnerCreateObjectPool(objectType, string.Empty, true, DefaultCapacity, DefaultReleaseInterval, DefaultExpireTime, DefaultPriority);
        }

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
        public IObjectPool<T> CreateRepeat<T>(string name, int capacity, float autoReleaseInterval, float expireTime, int priority) where T : ObjectBase
        {
            return InnerCreateObjectPool<T>(name, true, capacity, autoReleaseInterval, expireTime, priority);
        }

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
        public ObjectPoolBase CreateRepeat(Type objectType, string name, int capacity, float autoReleaseInterval, float expireTime, int priority)
        {
            return InnerCreateObjectPool(objectType, name, true, capacity, autoReleaseInterval, expireTime, priority);
        }

        /// <summary>
        /// 销毁对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>是否销毁对象池成功。</returns>
        public bool DestroyObjectPool<T>() where T : ObjectBase
        {
            return InnerDestroyObjectPool(Utility.Text.GetFullName<T>(string.Empty));
        }

        /// <summary>
        /// 销毁对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <returns>是否销毁对象池成功。</returns>
        public bool DestroyObjectPool(Type objectType)
        {
            Guard.Verify<ArgumentException>(objectType == null, "Object type is invalid.");
            Guard.Verify<ArgumentException>(!typeof(ObjectBase).IsAssignableFrom(objectType), string.Format("Object type '{0}' is invalid.", objectType.FullName));

            return InnerDestroyObjectPool(Utility.Text.GetFullName(objectType, string.Empty));
        }

        /// <summary>
        /// 销毁对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">要销毁的对象池名称。</param>
        /// <returns>是否销毁对象池成功。</returns>
        public bool DestroyObjectPool<T>(string name) where T : ObjectBase
        {
            return InnerDestroyObjectPool(Utility.Text.GetFullName<T>(name));
        }

        /// <summary>
        /// 销毁对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">要销毁的对象池名称。</param>
        /// <returns>是否销毁对象池成功。</returns>
        public bool DestroyObjectPool(Type objectType, string name)
        {
            Guard.Verify<ArgumentException>(objectType == null, "Object type is invalid.");
            Guard.Verify<ArgumentException>(!typeof(ObjectBase).IsAssignableFrom(objectType), string.Format("Object type '{0}' is invalid.", objectType.FullName));

            return InnerDestroyObjectPool(Utility.Text.GetFullName(objectType, name));
        }

        /// <summary>
        /// 销毁对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="objectPool">要销毁的对象池。</param>
        /// <returns>是否销毁对象池成功。</returns>
        public bool DestroyObjectPool<T>(IObjectPool<T> objectPool) where T : ObjectBase
        {
            Guard.Verify<ArgumentException>(objectPool == null, "Object type is invalid.");

            return InnerDestroyObjectPool(Utility.Text.GetFullName<T>(objectPool.Name));
        }

        /// <summary>
        /// 销毁对象池。
        /// </summary>
        /// <param name="objectPool">要销毁的对象池。</param>
        /// <returns>是否销毁对象池成功。</returns>
        public bool DestroyObjectPool(ObjectPoolBase objectPool)
        {
            Guard.Verify<ArgumentException>(objectPool == null, "Object type is invalid.");

            return InnerDestroyObjectPool(Utility.Text.GetFullName(objectPool.ObjectType, objectPool.Name));
        }

        /// <summary>
        /// 释放对象池中的可释放对象。
        /// </summary>
        public void Release()
        {
            ObjectPoolBase[] objectPools = GetAllObjectPools(true);
            foreach (ObjectPoolBase objectPool in objectPools)
            {
                objectPool.Release();
            }
        }

        /// <summary>
        /// 释放对象池中的所有未使用对象。
        /// </summary>
        public void ReleaseAllUnused()
        {
            ObjectPoolBase[] objectPools = GetAllObjectPools(true);
            foreach (ObjectPoolBase objectPool in objectPools)
            {
                objectPool.ReleaseAllUnused();
            }
        }

        private IObjectPool<T> InnerCreateObjectPool<T>(string name, bool allowMultiSpawn, int capacity, float autoReleaseInterval, float expireTime, int priority) where T : ObjectBase
        {
            Guard.Verify<LogicException>(HasObjectPool<T>(name), string.Format("Already exist object pool '{0}'.", Utility.Text.GetFullName<T>(name)));

            ObjectPool<T> objectPool = new ObjectPool<T>(name, allowMultiSpawn, capacity, autoReleaseInterval, expireTime, priority);
            objectPools.Add(Utility.Text.GetFullName<T>(name), objectPool);
            return objectPool;
        }

        private ObjectPoolBase InnerCreateObjectPool(Type objectType, string name, bool allowMultiSpawn, int capacity, float autoReleaseInterval, float expireTime, int priority)
        {
            Guard.Verify<ArgumentException>(objectType == null, "Object type is invalid.");
            Guard.Verify<ArgumentException>(!typeof(ObjectBase).IsAssignableFrom(objectType), string.Format("Object type '{0}' is invalid.", objectType.FullName));
            Guard.Verify<ArgumentException>(HasObjectPool(objectType, name), string.Format("Already exist object pool '{0}'.", Utility.Text.GetFullName(objectType, name)));

            Type objectPoolType = typeof(ObjectPool<>).MakeGenericType(objectType);
            ObjectPoolBase objectPool = (ObjectPoolBase)Activator.CreateInstance(objectPoolType, name, allowMultiSpawn, capacity, autoReleaseInterval, expireTime, priority);
            objectPools.Add(Utility.Text.GetFullName(objectType, name), objectPool);
            return objectPool;
        }

        private bool InnerDestroyObjectPool(string fullName)
        {
            ObjectPoolBase objectPool = null;
            if (objectPools.TryGetValue(fullName, out objectPool))
            {
                objectPool.ReleaseAll();
                return objectPools.Remove(fullName);
            }

            return false;
        }

        private int ObjectPoolComparer(ObjectPoolBase a, ObjectPoolBase b)
        {
            return a.Priority.CompareTo(b.Priority);
        }
    }
}