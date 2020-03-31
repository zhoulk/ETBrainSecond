/*
*    描述:
*          1. 引用计数
*
*    开发人: 邓平
*/

using System;
using System.Diagnostics;

namespace LtFramework.Util.Tools
{
    public interface IRefCounter
    {
        int count { get; }
        void Increase(int num = 1);
        void Decrease(int num = 1);
        
    }

    public class RefCounter : IRefCounter
    {
        public int count { get; private set; }

        private Action OnIncrease;
        private Action OnDecrease;
        private Action OnZeroRef;
        

        public RefCounter(Action onZeroRefCallBack = null)
        {
            OnZeroRef = onZeroRefCallBack;
        }

        public void Reset()
        {
            count = 0;
        }
        public void Increase(int num = 1)
        {
            count += num;
        }

        public void Decrease(int num = 1)
        {
            count -= num;
            if (count == 0)
            {
                if (OnZeroRef != null)
                {
                    OnZeroRef();
                }
                OnZeroRef?.Invoke();
            }
            if (count < 0)
            {
                UnityEngine.Debug.LogError("引用计数小于零");
            }
        }
    }

    public class ObjRefCounter : IRefCounter
    {
        private object _obj;
        private Action<object> OnZeroRef;

        public ObjRefCounter(object obj = null, Action<object> onZeroRefCallBack = null)
        {
            _obj = obj;
            OnZeroRef = onZeroRefCallBack;
        }

        public int count { get; private set; }
        public void Increase(int num = 1)
        {
            count += 1;
        }

        public void Decrease(int num = 1)
        {
            count -= 1;
            if (count == 0)
            {
                OnZeroRef?.Invoke(_obj);
            }

            if (count < 0)
            {
                UnityEngine.Debug.LogError("引用计数小于零 ");
            }
        }

    }
}
