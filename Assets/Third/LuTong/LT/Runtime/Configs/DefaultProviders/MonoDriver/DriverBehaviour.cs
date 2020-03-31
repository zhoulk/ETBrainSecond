/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/4
 * 模块描述：驱动脚本
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;
using System.Collections;

namespace LT.MonoDriver
{
    /// <summary>
    /// 驱动脚本
    /// </summary>
    [ExecutionOrder(-9999)]
    internal sealed class DriverBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 驱动器
        /// </summary>
        private MonoDriver driver;

        /// <summary>
        /// 等待帧结束
        /// </summary>
        private WaitForEndOfFrame waitForEndOfFrame;

        /// <summary>
        /// Awake
        /// </summary>
        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
            waitForEndOfFrame = new WaitForEndOfFrame();
        }

        /// <summary>
        /// 设定驱动器
        /// </summary>
        /// <param name="driver">驱动器</param>
        public void SetDriver(MonoDriver driver)
        {
            this.driver = driver;
        }

        /// <summary>
        /// 每帧更新时
        /// </summary>
        public void Update()
        {
            //StartCoroutine(EndOfFrame());

            if (driver != null)
            {
                driver.Update();
            }
        }

        /// <summary>
        /// 在每帧更新时之后
        /// </summary>
        public void LateUpdate()
        {
            if (driver != null)
            {
                driver.LateUpdate();
            }
        }

        /// <summary>
        /// 当释放时
        /// </summary>
        public void OnDestroy()
        {
            if (driver != null)
            {
                driver.OnDestroy();
            }
        }

        /// <summary>
        /// 固定刷新
        /// </summary>
        public void FixedUpdate()
        {
            if (driver != null)
            {
                driver.FixedUpdate();
            }
        }

        /// <summary>
        /// 当绘制GUI时
        /// </summary>
        public void OnGUI()
        {
            if (driver != null)
            {
                driver.OnGUI();
            }
        }

        /// <summary>
        /// 当应用获得/失去焦点
        /// </summary>
        /// <param name="focus"></param>
        private void OnApplicationFocus(bool focus)
        {
            if (driver != null)
            {
                driver.OnApplicationFocus(focus);
            }
        }

        /// <summary>
        /// 当应用暂停
        /// </summary>
        /// <param name="pause"></param>
        private void OnApplicationPause(bool pause)
        {
            if (driver != null)
            {
                driver.OnApplicationPause(pause);
            }
        }

        /// <summary>
        /// 当应用退出
        /// </summary>
        private void OnApplicationQuit()
        {
            if (driver != null)
            {
                driver.OnApplicationQuit();
            }
        }

        /// <summary>
        /// 帧结束
        /// </summary>
        /// <returns></returns>
        IEnumerator EndOfFrame()
        {
            yield return waitForEndOfFrame;

            if (driver != null)
            {
                driver.EndOfFrame();
            }
        }
    }
}