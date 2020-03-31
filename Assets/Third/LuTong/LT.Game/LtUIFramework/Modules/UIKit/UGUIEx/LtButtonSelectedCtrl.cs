/*
 *    描述:
 *          1. LtButton按钮操作回调类
 *
 *    开发人: 邓平
 */
using UnityGameFramework.Runtime;

namespace LtFramework.UI
{
    public class LtButtonSelectedCtrl : MonoBehaviourEx, ILtButtonCtrl
    {
        void Awake()
        {
        }


        public void OnSelected()
        {
            gameObject.SetActive(true);
        }

        public void OnDiselected()
        {
            gameObject.SetActive(false);
        }

        public void OnClick()
        {
        }


    }
}
