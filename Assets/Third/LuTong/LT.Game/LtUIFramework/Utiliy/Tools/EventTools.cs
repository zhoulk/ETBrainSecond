/*
*    描述:
*          1. 常用方法函数封装
*
*    开发人: 邓平
*/
using System;
using System.IO;
using UnityEngine;

namespace LtFramework.Util.Tools
{
    public class EventTools
    {

        #region 子节点操作


        /// <summary>
        /// 根据名字查找子节点 （名字唯一）
        /// </summary>
        /// <param UIName="goParent">父节点</param>
        /// <param UIName="ChildName">查找 子节点名字</param>
        /// <returns>null 没找到子节点</returns>
        public static Transform FindTheChildNode(GameObject goParent, string ChildName)
        {
            Transform searchTrans = null;
            searchTrans = goParent.transform.Find(ChildName);
            if (searchTrans == null)
            {
                foreach (Transform trans in goParent.transform)
                {
                    searchTrans = FindTheChildNode(trans.gameObject, ChildName);
                    if (searchTrans != null)
                    {
                        return searchTrans;
                    }

                }
            }

            return searchTrans;
        }

        /// <summary>
        /// 根据子节点名字 得到子节点的 T 组件
        /// </summary>
        /// <param UIName="goParent">父节点</param>
        /// <param UIName="childName">子节点 名字</param>
        /// <typeparam UIName="T"> 获得组件 </typeparam>
        /// <returns> null 没找到相应的子节点 </returns>
        public static T GetTheChildNodeCompont<T>(GameObject goParent, string childName) where T : Component
        {
            Transform searchTranformNode = null;
            searchTranformNode = FindTheChildNode(goParent, childName);
            if (searchTranformNode != null)
            {
                return searchTranformNode.gameObject.GetComponent<T>();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据 子节点名字 给子节点添加 T 组件
        /// </summary>
        /// <param UIName="goParent"> 父对象 </param>
        /// <param UIName="childName"> 子节点 </param>
        /// <typeparam UIName="T"> 添加的组件 </typeparam>
        /// <returns> null 没找到相应的子节点 </returns>
        public static T AddChildNodeCompnet<T>(GameObject goParent, string childName) where T : Component
        {
            Transform searchTranformNode = null;
            searchTranformNode = FindTheChildNode(goParent, childName);
            if (searchTranformNode != null)
            {
                //如果有相同脚本 先删 否则直接添加
                T[] componentScriptsArray = searchTranformNode.GetComponents<T>();
                for (int i = 0; i < componentScriptsArray.Length; i++)
                {
                    if (componentScriptsArray[i] != null)
                    {
                        UnityEngine.Object.Destroy(componentScriptsArray[i]);
                    }

                }

                return searchTranformNode.gameObject.AddComponent<T>();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 将 child 添加到 parents 节点下 并 Reset Transform
        /// </summary>
        /// <param UIName="parents"> 父节点 </param>
        /// <param UIName="child"> 子物体 </param>
        public static void AddChildNodeToParentNode(Transform parents, Transform child)
        {
            child.SetParent(parents, false);
            StePositionZero(child);
        }

        /// <summary>
        /// Reset Transform
        /// </summary>
        /// <param UIName="transform"> </param>
        public static void StePositionZero(Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localEulerAngles = Vector3.zero;
        }


        #endregion


        #region Gun转向

        /// <summary>
        /// 朝向目标位置 目标位置改变   Update中调用
        /// </summary>
        /// <param UIName="tran"></param>
        /// <param UIName="ptarget"></param>
        /// <param UIName="speed"></param>
        public static void LookAtUpdate(Transform tran, Vector3 ptarget, float speed)
        {
            Vector3 target = (ptarget - tran.position).normalized;

            float myeulerAng = tran.localEulerAngles.z;
            Vector3 myVec = EulerAngToVerctor3(myeulerAng);
            float ang = Vector3.Angle(myVec, target);

            Vector3 v = Vector3.Cross(myVec, target);
            if (v.z <= 0)
            {
                tran.localEulerAngles = new Vector3(tran.localEulerAngles.x, tran.localEulerAngles.y,
                    Mathf.Lerp(tran.localEulerAngles.z, tran.localEulerAngles.z - ang, speed * Time.fixedDeltaTime));
            }
            else
            {
                tran.localEulerAngles = new Vector3(tran.localEulerAngles.x, tran.localEulerAngles.y,
                    Mathf.Lerp(tran.localEulerAngles.z, tran.localEulerAngles.z + ang, speed * Time.deltaTime));
            }
        }

        private static Vector3 EulerAngToVerctor3(float eulerAng)
        {
            Vector3 vec = new Vector3(0, 1, 0);
            return V3RotateAround(vec, Vector3.forward, eulerAng);
        }

        /// <summary>
        /// 计算一个Vector3绕旋转中心旋转指定角度后所得到的向量。
        /// </summary>
        /// <param UIName="source">旋转前的源Vector3</param>
        /// <param UIName="axis">旋转轴</param>
        /// <param UIName="angle">旋转角度</param>
        /// <returns>旋转后得到的新Vector3</returns>
        private static Vector3 V3RotateAround(Vector3 source, Vector3 axis, float angle)
        {
            Quaternion q = Quaternion.AngleAxis(angle, axis); // 旋转系数
            return q * source; // 返回目标点
        }

        #endregion

        #region 设置 位置偏移


        public static void SetPositionByOffset(GameObject source, GameObject target, Vector2 offset)
        {
            SetPositionByOffset(source, target.transform, offset);
        }

        public static void SetPositionByOffset(GameObject source, Transform target, Vector2 offset)
        {

            if (System.Math.Abs(target.transform.localEulerAngles.y) < 0.1f)
            {
                offset = new Vector2(-offset.x, offset.y);
            }

            source.transform.localPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y,
                target.position.z);
        }


        #endregion



        public static void SetPositionAndAngleToGameObject(GameObject source, Transform target)
        {
            source.transform.localPosition = target.position;
            source.transform.localScale = target.localScale;
            source.transform.localEulerAngles = target.localEulerAngles;
        }

        public static void SetPositionToGameObject(GameObject source, Transform target)
        {
            source.transform.localPosition = target.position;
            source.transform.localScale = target.localScale;
            source.transform.localEulerAngles = Vector3.zero;
        }

        public static int GetRangeMinValue(int value1, int value2)
        {
            return Mathf.Max(Mathf.Min(value1, value2), 0);
        }

        #region 文件读取


        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param UIName="path">路径</param>
        /// <param UIName="name">文件名</param>
        /// <param UIName="info">文件信息</param>
        public static void CreateFile(string path, string name, string info)
        {
            //文件流信息
            StreamWriter sw;
            FileInfo t = new FileInfo(path + "//" + name);

            //如果此文件不存在则创建
            sw = t.CreateText();

            //以行的形式写入信息
            sw.WriteLine(info);
            //关闭流
            sw.Close();
            //销毁流
            sw.Dispose();
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param UIName="path">文件路径</param>
        /// <param UIName="name">文件名</param>
        /// <returns>文件内容</returns>
        public static string LoadFile(string path, string name)
        {
            //使用流的形式读取
            StreamReader sr = null;
            try
            {
                sr = File.OpenText(path + "//" + name);
            }
            catch (Exception)
            {

                Debug.LogError("没有找到相应的文件：" + path + "/" + name);
                //路径与名称未找到文件则直接返回空
                return null;
            }

            string arrlist;
            arrlist = sr.ReadToEnd();
            //关闭流
            sr.Close();
            //销毁流
            sr.Dispose();
            //将数组链表容器返回
            return arrlist;
        }

        #endregion

    }
}
