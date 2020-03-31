/*
 *    描述:
 *          1. LtButton 按键静态扩展类
 *
 *    开发人: 邓平
 */
using UnityEngine.Events;

namespace LtFramework.UI
{
    public static class LtUIEvent
    {
        public static void ButtonOnClick(this LtButton button,UnityAction call, UnityAction call2 = null)
        {
            button.AddListener1P(call);
            button.AddListener2P(call2);
        }

    }
}
