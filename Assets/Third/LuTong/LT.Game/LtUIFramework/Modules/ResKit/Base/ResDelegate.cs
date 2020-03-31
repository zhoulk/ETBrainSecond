/*
 *    描述:
 *          1.
 *
 *    开发人: 邓平
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LtFramework.ResKit
{
    /// <summary>
    /// 资源加载完成回调
    /// </summary>
    /// <param UIName="path"></param>
    /// <param UIName="obj"></param>
    /// <param UIName="param1"></param>
    /// <param UIName="param2"></param>
    /// <param UIName="param3"></param>
    public delegate void OnAsyncObjFinish(string path, Object obj,params object[] paramValues);

    /// <summary>
    /// 实例化资源加载对象完成回调
    /// </summary>
    /// <param UIName="path"></param>
    /// <param UIName="resObj"></param>
    /// <param UIName="param1"></param>
    /// <param UIName="param2"></param>
    /// <param UIName="param3"></param>
    public delegate void OnAsyncFinish(string path, ResouceObj resObj,params object[] paramValues);
}
