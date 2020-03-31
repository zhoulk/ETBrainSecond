/*
*    描述:
*          1. 静态扩展类
*
*    开发人: 邓平
*/
using UnityEngine;

namespace LtFramework.Util
{

    public static partial class ComponentEx
    {
        public static T MyComponent<T>(this Component component) where T : Component
        {
            T com = component.GetComponent<T>();
            if (com != null)
            {
                return com;
            }

            Debug.LogError("该物体 " + component.name + " 上没有" + typeof(T) + "组件");
            return null;
        }
    }
}
