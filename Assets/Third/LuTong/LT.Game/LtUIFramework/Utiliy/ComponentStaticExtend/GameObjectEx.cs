/*
*    描述:
*          1. 静态扩展类
*
*    开发人: 邓平
*/
using UnityEngine;

namespace LtFramework.Util
{

    public static class GameObjectEx
    {
        /// <summary>
        /// 获取子节点（对象）脚本
        /// </summary>
        /// <typeparam UIName="T">泛型</typeparam>
        /// <param UIName="goParent">父对象</param>
        /// <param UIName="childName">子对象名称</param>
        /// <returns></returns>
        public static T GetChildComponet<T>(this GameObject parentObj, string childName) where T : Component
        {
            Transform searchTranformNode = null; //查找特定子节点

            searchTranformNode = parentObj.transform.FindChildNode(childName);
            if (searchTranformNode != null)
            {
                T t = searchTranformNode.gameObject.GetComponent<T>();
                if (t != null)
                {
                    return t;
                }
                else
                {
                    Debug.LogError(childName + "物体上 不存在 [" + typeof(T).Name + "] 组件");
                    return null;
                }

            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 给子节点添加脚本
        /// </summary>
        /// <typeparam UIName="T"></typeparam>
        /// <param UIName="goParent">父对象</param>
        /// <param UIName="childName">子对象名称</param>
        /// <returns></returns>
        public static T AddChildNodeCompnent<T>(this GameObject goParent, string childName) where T : Component
        {
            Transform searchTranform = null; //查找特定节点结果

            //查找特定子节点
            searchTranform = goParent.transform.FindChildNode(childName);
            //如果查找成功，则考虑如果已经有相同的脚本了，则先删除，否则直接添加。
            if (searchTranform != null)
            {
                //如果已经有相同的脚本了，则先删除
                T[] componentScriptsArray = searchTranform.GetComponents<T>();
                for (int i = 0; i < componentScriptsArray.Length; i++)
                {
                    if (componentScriptsArray[i] != null)
                    {
                        GameObject.Destroy(componentScriptsArray[i]);
                    }
                }

                return searchTranform.gameObject.AddComponent<T>();
            }
            else
            {
                return null;
            }

        }


    }
}
