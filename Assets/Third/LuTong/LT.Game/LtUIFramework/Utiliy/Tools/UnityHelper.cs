/*
*    描述:
*          1. 测试方法写到这个类中
*             方法测试没有问题后移到相应的类下
*
*    开发人: 邓平
*/
using System;
using UnityEngine;

namespace LtFramework.Util.Tools
{
    public class UnityHelper
    {

        #region 弃用方法

        [Obsolete("该方法已经过时,请使用 transform.FindChildNode( chiildName )")]
        public static Transform FindTheChildNode(GameObject goParent, string chiildName)
        {
            return goParent.transform.FindChildNode(chiildName);
        }

        [Obsolete("该方法已经过时,请使用 gameobject.GetChildComponet( chiildName )")]
        public static T GetTheChildNodeComponet<T>(GameObject goParent, string childName) where T : Component
        {
            return goParent.GetChildComponet<T>(childName);
        }
        #endregion
    }
}