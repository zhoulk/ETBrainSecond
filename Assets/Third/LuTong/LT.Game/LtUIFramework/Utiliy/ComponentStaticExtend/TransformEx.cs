/*
*    描述:
*          1. 静态扩展类
*
*    开发人: 邓平
*/
using LtFramework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace LtFramework.Util
{
    /// <summary>
    /// 查找扩展方法
    /// </summary>
    public static partial class TransformEx
    {
        /// <summary>
        /// 查找子节点对象
        /// 内部使用“递归算法”
        /// </summary>
        /// <param UIName="goParent">父对象</param>
        /// <param UIName="chiildName">查找的子对象名称</param>
        /// <returns></returns>
        public static Transform FindChildNode(this Transform transform, string chiildName)
        {
            Transform searchTrans = null; //查找结果
            searchTrans = transform.Find(chiildName);
            if (searchTrans == null)
            {
                foreach (Transform trans in transform)
                {
                    searchTrans = FindChildNode(trans, chiildName);
                    if (searchTrans != null)
                    {
                        return searchTrans;
                    }
                }
            }
            return searchTrans;
        }

        /// <summary>
        /// 获取子节点（对象）脚本
        /// </summary>
        /// <typeparam UIName="T">泛型</typeparam>
        /// <param UIName="transform">父对象</param>
        /// <param UIName="childName">子对象名称</param>
        /// <returns></returns>
        public static T GetChildComponet<T>(this Transform transform, string childName) where T : Component
        {
            return transform.gameObject.GetChildComponet<T>(childName);
        }

        /// <summary>
        /// 给子节点添加父对象
        /// </summary>
        /// <param UIName="parents">父对象的方位</param>
        /// <param UIName="child">子对象的方法</param>
        public static void AddChild(this Transform parents, Transform child)
        {
            parents.SetParent(parents);
            parents.Identity();
        }
    }

    /// <summary>
    /// 赋值扩展方法
    /// </summary>
    public static partial class TransformEx
    {
        public static void SetLocalPosX(this Transform transform, float x)
        {

            var localPosition = transform.localPosition;
            localPosition.x = x;
            transform.localPosition = localPosition;
        }

        public static void SetLocalPosY(this Transform transform, float y)
        {

            var localPosition = transform.localPosition;
            localPosition.y = y;
            transform.localPosition = localPosition;
        }

        public static void SetLocalPosZ(this Transform transform, float z)
        {

            var localPosition = transform.localPosition;
            localPosition.z = z;
            transform.localPosition = localPosition;
        }

        public static void SetLocalPosXY(this Transform transform, float x, float y)
        {

            var localPosition = transform.localPosition;
            localPosition.x = x;
            localPosition.y = y;
            transform.localPosition = localPosition;
        }

        public static void SetLocalPosXZ(this Transform transform, float x, float z)
        {

            var localPosition = transform.localPosition;
            localPosition.x = x;
            localPosition.z = z;
            transform.localPosition = localPosition;
        }

        public static void SetLocalPosYZ(this Transform transform, float y, float z)
        {
            var localPosition = transform.localPosition;
            localPosition.y = y;
            localPosition.z = z;
            transform.localPosition = localPosition;
        }

        public static void SetLocalPosXYZ(this Transform transform, float x, float y, float z)
        {
            var localPosition = transform.localPosition;
            localPosition.x = x;
            localPosition.y = y;
            localPosition.z = z;
            transform.localPosition = localPosition;
        }

        public static void Identity(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
        }

        public static void Identity(this MonoBehaviour mono)
        {
            mono.transform.Identity();
        }

    }

    /// <summary>
    /// 获得组件扩展方法
    /// </summary>
    public static partial class TransformEx
    {
        public static RectTransform RectTransform(this Transform transform)
        {
            RectTransform rect = transform.GetComponent<RectTransform>();
            if (rect)
            {
                return rect;
            }
            Debug.LogError("该物体 " + transform.name + " 上没有RectTransform组件");
            return null;
        }

        public static LtButton LtButton(this Transform transform)
        {
            LtButton ltButton = transform.GetComponent<LtButton>();
            if (ltButton)
            {
                return ltButton;
            }

            Debug.LogError("该物体 " + transform.name + " 上没有LtButton组件");
            return null;
        }

        public static Image Image(this Transform transform)
        {
            Image image = transform.GetComponent<Image>();
            if (image)
            {
                return image;
            }

            Debug.LogError("该物体 " + transform.name + " 上没有Image组件");
            return null;
        }

        public static Animator Animator(this Transform transform)
        {
            Animator animator = transform.GetComponent<Animator>();
            if (animator)
            {
                return animator;
            }
            Debug.LogError("该物体 " + transform.name + " 上没有Animator组件");
            return null;
        }

        public static Slider Slider(this Transform transform)
        {
            Slider slider = transform.GetComponent<Slider>();
            if (slider)
            {
                return slider;
            }

            Debug.LogError("该物体 " + transform.name + " 上没有Slider组件");
            return null;
        }

        public static Text Text(this Transform transform)
        {
            Text text = transform.GetComponent<Text>();
            if (text)
            {
                return text;
            }

            Debug.LogError("该物体 " + transform.name + " 上没有Text组件");
            return null;
        }



    }
}
