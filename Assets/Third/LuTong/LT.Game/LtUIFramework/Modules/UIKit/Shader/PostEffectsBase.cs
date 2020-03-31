/*
 *    描述:
 *          1. 屏幕效果基类
 *
 *    开发人: 邓平
 */
using UnityEngine;

namespace LtFramework.UI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class PostEffectsBase : MonoBehaviour
    {

        protected void Start()
        {
            CheckResources();
        }

        #region 检测平台

        protected void CheckResources()
        {
            bool isSupported = CheckSupport();

            if (isSupported == false)
            {
                NotSupported();
            }
        }

        protected bool CheckSupport()
        {
            if (SystemInfo.supportsImageEffects == false)
            {
                Debug.LogWarning("该平台不支持 Image Effect");
                return false;
            }

            return true;
        }

        protected void NotSupported()
        {
            enabled = false;
        }

        #endregion



        protected Material CheckShaderAndCreateMaterial(Shader shader, Material material)
        {
            if (shader == null)
            {
                return null;
            }

            if (shader.isSupported && material && material.shader == shader)
                return material;

            if (!shader.isSupported)
            {
                return null;
            }
            else
            {
                material = new Material(shader);
                material.hideFlags = HideFlags.DontSave;
                if (material)
                    return material;
                else
                    return null;
            }
        }
    }
}
