/*
 *    描述:
 *          1.Mono单例
 *
 *    开发人: 邓平
 */
using UnityEngine;
namespace LtFramework.Util
{
    public static class MonoSingletonProperty<T> where T : MonoBehaviour, ISingleton
	{
		private static T _Instance = null;

		public static T Instance
		{
			get
			{
				if (null == _Instance)
				{
					_Instance = MonoSingletonCreator.CreateMonoSingleton<T>();
				}

				return _Instance;
			}
		}

		public static void Dispose()
		{
			if (MonoSingletonCreator.IsUnitTestMode)
			{
				Object.DestroyImmediate(_Instance.gameObject);
			}
			else
			{
				Object.Destroy(_Instance.gameObject);
			}

			_Instance = null;
		}
	}
}