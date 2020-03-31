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
    /// 默认UI窗体辅助器
    /// </summary>
    public class DefaultUIFormHelper : IUIFormHelper
    {
        private readonly Transform m_UIGroupRoot;

        /// <summary>
        /// 构建默认的窗体辅助器
        /// </summary>
        /// <param name="uiGroupRoot"></param>
        public DefaultUIFormHelper(Transform uiGroupRoot)
        {
            m_UIGroupRoot = uiGroupRoot;
        }

        /// <inheritdoc />
        public object InstantiateUIForm(object uiFormAsset)
        {
            return Object.Instantiate(uiFormAsset as Object);
        }

        /// <inheritdoc />
        public IUIForm CreateUIForm(object uiFormInstance, IUIGroup uiGroup, object userData)
        {
            GameObject gameObject = uiFormInstance as GameObject;

            if (gameObject == null)
            {
                LTLog.Error("UI form instance is invalid.");
                return null;
            }

            Transform transform = gameObject.transform;
            transform.SetParent(((MonoBehaviour)uiGroup.Helper).transform);
            transform.localScale = Vector3.one;

            return gameObject.GetComponent<IUIForm>();
        }

        /// <summary>
        /// 创建界面组辅助器
        /// </summary>
        /// <param name="uiGroupName">界面组名</param>
        /// <returns>界面组</returns>
        public IUIGroupHelper CreateUIGroupHelper(string uiGroupName)
        {
            GameObject instance = new GameObject($"UI Group - {uiGroupName}");
            instance.GetOrAddComponent<GraphicRaycaster>();
            instance.GetOrAddComponent<Canvas>();
            var uiGroupHelper = instance.AddComponent<DefaultUIGroupHelper>();

            instance.layer = UnityEngine.LayerMask.NameToLayer("UI");
            instance.transform.SetParent(m_UIGroupRoot);
            instance.transform.localScale = Vector3.one;
            instance.transform.localPosition = Vector3.zero;

            return uiGroupHelper;
        }

        /// <inheritdoc />
        public void ReleaseUIForm(object uiFormInstance)
        {
            Object.Destroy(uiFormInstance as Object);
            Resources.UnloadUnusedAssets();
        }
    }
}