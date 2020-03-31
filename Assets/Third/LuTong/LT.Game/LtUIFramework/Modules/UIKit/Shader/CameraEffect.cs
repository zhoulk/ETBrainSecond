/*
 *    描述:
 *          1. 相机效果
 *
 *    开发人: 邓平
 */
using UnityEngine;
using System.Collections;

namespace LtFramework.UI
{

    public delegate void cameraEffectHide();


    public class CameraEffect : PostEffectsBase
    {
        public static CameraEffect Instance;

        private void Awake()
        {
            Instance = this;
        }

        public Shader cameraShader;
        private Material cameraMaterial;

        public Material material
        {
            get
            {
                cameraMaterial = CheckShaderAndCreateMaterial(cameraShader, cameraMaterial);
                return cameraMaterial;
            }
        }

        [Range(0.0f, 1.0f)] public float brightness = 1.0f;

        private bool isOk = false;

        public void SetOKTure()
        {
            isOk = true;
        }

        public void SetOKFalse()
        {
            isOk = false;
        }





        void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            if (isOk) return;

            if (material != null)
            {
                material.SetFloat("_Brightness", brightness);

                Graphics.Blit(src, dest, material);
            }
            else
            {
                Graphics.Blit(src, dest);
            }
        }

    }

}
