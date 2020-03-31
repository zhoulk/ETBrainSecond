/*
 *    描述:
 *          1.
 *
 *    开发人: 邓平
 */

using System;
using System.Collections.Generic;

namespace LtFramework.ResKit
{

    /// <summary>
    /// 异步加载参数
    /// </summary>
    public class AsyncLoadResParam
    {
        //回调列表
        public List<AsyncCallBack> CallBackList = new List<AsyncCallBack>();
        public uint Crc;
        public string Path;
        public Type ResType;
        public LoadResMode LoadMode;

        public LoadResPriority Priority = LoadResPriority.Res_Slow;

        public void Reset()
        {
            CallBackList.Clear();
            Crc = 0;
            Path = string.Empty;
            ResType = typeof(UnityEngine.Object);
            Priority = LoadResPriority.Res_Slow;
            LoadMode = LoadResMode.Resource;
        }
    }


    public class AsyncCallBack
    {
        //加载完成的回调(针对ObjectManager)
        public OnAsyncFinish OnFinish = null;

        //ObjectManager对应的中间类
        public ResouceObj ResObj = null;

        //------------------------------------------------//
        //完成回调
        public OnAsyncObjFinish OnObjFinish = null;

        //回调参数
        public readonly List<object> Params = new List<object>();

        public object[] ParamValues
        {
            get
            {
                return Params.ToArray();
            }
        }

        public void Reset()
        {
            OnObjFinish = null;
            OnFinish = null;
            Params.Clear();
            ResObj = null;
        }
    }
}
