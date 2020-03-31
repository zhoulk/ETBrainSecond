/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/04/29
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;
using LT.MonoDriver;

namespace LT.Fps
{
    /// <summary>
    /// 游戏帧率信息显示
    /// </summary>
	internal class Fps : IUpdate, IOnGUI
    {
        private float m_Fps;
        private Color m_Color;
        private float m_LastSinceNow;
        private int m_BetweenInternal;

        /// <summary>
        /// 构建Fps信息
        /// </summary>
        public Fps()
        {
            m_Color = new Color(0, 1, 0);
            m_LastSinceNow = Time.realtimeSinceStartup;
        }

        /// <summary>
        /// 获取当前Fps值
        /// </summary>
        public int Value
        {
            get { return (int)m_Fps; }
        }

        /// <summary>
        /// MonoBehavior Update
        /// </summary>
		public void Update()
        {
            m_BetweenInternal++;

            float sinceNow = Time.realtimeSinceStartup;
            if (sinceNow >= (m_LastSinceNow + 1f))
            {
                m_Fps = Mathf.RoundToInt(m_BetweenInternal / (sinceNow - m_LastSinceNow));
                m_LastSinceNow = sinceNow;
                m_BetweenInternal = 0;
            }
        }

        /// <summary>
        /// MonoBehavior OnGUI
        /// </summary>
        public void OnGUI()
        {
            GUI.color = m_Color;
            GUI.Box(new Rect(10, 10, 160, 24), $"Version:{UnityEngine.Application.version}  FPS:{Value}");
        }
    }
}