/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/08/09
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using LT.DataTable;

namespace LT.UI
{
    /// <summary>
    /// UI 扩展
    /// </summary>
    public static class ExtensionUI
    {
        #region animation methods extension
        /// <summary>
        /// 淡入淡出
        /// </summary>
        /// <param name="canvasGroup">扩展CanvasGroup组件</param>
        /// <param name="alpha">目标alpha值</param>
        /// <param name="duration">持续时间</param>
        /// <returns>迭代器</returns>
        public static IEnumerator Fade2Alpha(this CanvasGroup canvasGroup, float alpha, float duration, Action completedCallback = null)
        {
            float time = 0f;
            float originalAlpha = canvasGroup.alpha;
            while (time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
                yield return new WaitForEndOfFrame();
            }

            canvasGroup.alpha = alpha;
            completedCallback?.Invoke();
        }

        /// <summary>
        /// 平滑过渡
        /// </summary>
        /// <param name="slider">扩展Slider组件</param>
        /// <param name="value">目标值</param>
        /// <param name="duration">持续时间</param>
        /// <returns>迭代器</returns>
        public static IEnumerator Smooth(this Slider slider, float value, float duration)
        {
            float time = 0f;
            float originalValue = slider.value;

            while (time < duration)
            {
                time += Time.deltaTime;
                slider.value = Mathf.Lerp(originalValue, value, time / duration);
                yield return new WaitForEndOfFrame();
            }

            slider.value = value;
        }
        #endregion

        #region datatable methods extension

        /// <summary>
        /// 基于资源id获取窗体信息
        /// 通过id值操作UI，需先注册闭包处理
        /// </summary>
        private static Func<int, UIFormInfo> GetUIFormInfo = null;

        /// <summary>
        /// 扩展UI模块对数据表的支持
        /// </summary>
        /// <param name="uiManager">ui管理器</param>
        /// <param name="closure">扩展闭包</param>
        public static void Extend(this IUIManager uiManager, Func<int, UIFormInfo> closure)
        {
            GetUIFormInfo = closure;
        }

        public static bool HasUIForm<T>(this IUIManager uiManager, int uiFormId, string uiGroupName = null) where T : IDataRow
        {
            Guard.Verify<LogicException>(GetUIFormInfo == null, "Must transfer the Extend() method before GetUIFormInfo() method.");

            UIFormInfo uiFormInfo = GetUIFormInfo(uiFormId);

            if (string.IsNullOrEmpty(uiGroupName))
            {
                return uiManager.HasUIForm(uiFormInfo.AssetName);
            }

            IUIGroup uiGroup = uiManager.GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                return false;
            }

            return uiGroup.HasUIForm(uiFormInfo.AssetName);
        }

        public static IUIForm GetUIForm(this IUIManager uiManager, int uiFormId, string uiGroupName = null)
        {
            Guard.Verify<LogicException>(GetUIFormInfo == null, "Must transfer the Extend() method before GetUIFormInfo() method.");

            UIFormInfo uiFormInfo = GetUIFormInfo(uiFormId);
            if (string.IsNullOrEmpty(uiGroupName))
            {
                return uiManager.GetUIForm(uiFormInfo.AssetName);
            }

            IUIGroup uiGroup = uiManager.GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                return null;
            }

            return (UIForm)uiGroup.GetUIForm(uiFormInfo.AssetName);
        }

        public static void CloseUIForm(this IUIManager uiManager, int uiFormId)
        {
            var uiForm = GetUIForm(uiManager, uiFormId);
            uiManager.CloseUIForm(uiForm);
        }

        public static void OpenUIForm(this IUIManager uiManager, int uiFormId, object userData = null)
        {
            Guard.Verify<LogicException>(GetUIFormInfo == null, "Must transfer the Extend() method before GetUIFormInfo() method.");

            UIFormInfo uiFormInfo = GetUIFormInfo(uiFormId);

            if (uiManager.HasUIForm(uiFormInfo.AssetName))
            {
                return;
            }

            uiManager.OpenUIForm(uiFormInfo.AssetName, uiFormInfo.UIGroupName, uiFormInfo.UIGroupDepth, uiFormInfo.PauseCoveredUIForm, userData);
        }

        #endregion
    }
}