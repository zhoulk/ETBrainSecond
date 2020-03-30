﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// 加载方式。
    /// </summary>
    public enum LoadType
    {
        /// <summary>
        /// 按文本加载。
        /// </summary>
        Text = 0,

        /// <summary>
        /// 按二进制流加载。
        /// </summary>
        Bytes,

        /// <summary>
        /// 按二进制流加载。
        /// </summary>
        Stream
    }
}