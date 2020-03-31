/*
 *    描述:
 *          1. UI生命周期函数回调基类接口
 *
 *    开发人: 邓平
 */
using System.Collections.Generic;

namespace LtFramework.UI
{
    public interface ILtDisplayStateUI
    {
        /// <summary>
        /// UI显示状态
        /// </summary>
        DisplayState displayState { get; set; }

        /// <summary>
        /// 动画索引
        /// </summary>
        List<int> AnimationIndexs { get; set; }

    }
}
