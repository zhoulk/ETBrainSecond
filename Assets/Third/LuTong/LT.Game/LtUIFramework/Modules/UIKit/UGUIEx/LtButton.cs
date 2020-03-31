/*
 *    描述:
 *          1. UGUI扩展类
 *
 *    开发人: 邓平
 */
using UnityEngine.Events;

namespace LtFramework.UI
{
    public class LtButton : ButtonEx
    {
        public void OnClick(UnityAction call, UnityAction call2 = null)
        {
            AddListener1P(call);
            AddListener2P(call2);
        }
    }

}
