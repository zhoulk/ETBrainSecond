/*
 *    描述:
 *          1.Mono单例
 *
 *    开发人: 邓平
 */
namespace LtFramework.Util
{
    using System;

    public class QMonoSingletonPath : MonoSingletonPath
    {
        public QMonoSingletonPath(string pathInHierarchy) : base(pathInHierarchy)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class MonoSingletonPath : Attribute
    {
		private string mPathInHierarchy;

        public MonoSingletonPath(string pathInHierarchy)
        {
            mPathInHierarchy = pathInHierarchy;
        }

        public string PathInHierarchy
        {
            get { return mPathInHierarchy; }
        }
    }
}
