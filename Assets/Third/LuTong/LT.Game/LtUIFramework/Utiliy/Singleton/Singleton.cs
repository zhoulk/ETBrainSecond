/*
 *    描述:
 *          1.单例类
 *               子类要实现私有构造方法
 *
 *    开发人: 邓平
 */
namespace LtFramework.Util
{
	public abstract class Singleton<T> : ISingleton where T : Singleton<T>
	{
		protected static T _Instance;

		static object mLock = new object();

		protected Singleton()
		{
		}

		public static T Instance
		{
			get
			{
				lock (mLock)
				{
					if (_Instance == null)
					{
						_Instance = SingletonCreator.CreateSingleton<T>();
					}
				}

				return _Instance;
			}
		}

		public virtual void Dispose()
		{
			_Instance = null;
		}

		public virtual void OnSingletonInit()
		{
		}
	}
}