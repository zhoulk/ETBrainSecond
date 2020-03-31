/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/07/03
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using UnityEngine;

namespace LT.Sdk
{
    /// <summary>
    /// Unity调用Android辅助器
    /// </summary>
    public class Unity2AndroidHelper
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public Unity2AndroidHelper()
        {
            try
            {
#if UNITY_EDITOR
                Debug.Log("LTSdk 需在真机上运行生效.");
#else
                //启动sdk
                MainObject = new AndroidJavaObject("com.lutongnet.Unity2Android");
                MainObject.Call("Main");

                //获取桥接文件
                BridgeClass = new AndroidJavaClass("org.unity.UnityAPIBridge");

                Debug.Log("LTSdk 初始化成功.");
#endif
            }
            catch (Exception e)
            {
                Debug.LogError($"LTSdk 初始化失败.{e} ");
            }
        }

        /// <summary>
        /// 桥接类
        /// </summary>
        public AndroidJavaClass BridgeClass
        {
            get;
            private set;
        }

        /// <summary>
        /// 入口对象
        /// </summary>
        public AndroidJavaObject MainObject
        {
            get;
            private set;
        }
    }
}