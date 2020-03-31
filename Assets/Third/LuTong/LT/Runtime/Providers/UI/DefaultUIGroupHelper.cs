/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/08/08
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;
using UnityEngine.UI;

namespace LT.UI
{
    /// <summary>
    /// 默认UIGroup辅助器
    /// </summary>
    public class DefaultUIGroupHelper : MonoBehaviour, IUIGroupHelper
    {
        private int depth;
        private Canvas canvas;

        private void Awake()
        {
            canvas = gameObject.GetOrAddComponent<Canvas>();
            gameObject.GetOrAddComponent<GraphicRaycaster>();
        }

        private void Start()
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;
            canvas.sortingOrder = depth;

            RectTransform transform = GetComponent<RectTransform>();
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.anchoredPosition = Vector2.zero;
            transform.sizeDelta = Vector2.zero;
        }

        /// <summary>
        /// 设置界面组深度。
        /// </summary>
        /// <param name="depth">界面组深度。</param>
        public virtual void SetDepth(int depth)
        {
            this.depth = depth;
            canvas.overrideSorting = true;
            canvas.sortingOrder = depth;
        }
    }
}