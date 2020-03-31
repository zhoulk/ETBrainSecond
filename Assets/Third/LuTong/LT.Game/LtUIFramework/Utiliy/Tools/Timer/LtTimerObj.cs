/*
*    描述:
*          1.计时器
*
*    开发人: 邓平
*/
using System;
using UnityEngine;

namespace LtFramework.Util.Tools
{
    public class LtTimerObj : MonoBehaviour
    {
        private float _Time;
        private float _Timer = 1;
        private Action _CallBack;
        private bool _Loop;

        public void Init(Action callBack, float timer, bool loop = false)
        {
            _Timer = timer;
            _Time = timer;
            this._CallBack = callBack;
            this._Loop = loop;
        }

        public void Alarm()
        {
            _Timer -= Time.deltaTime;
            if (_Timer <= 0)
            {
                if (_CallBack != null)
                {
                    _CallBack.Invoke();
                }

                if (_CallBack != null)
                {
                    _CallBack();
                    if (_Loop)
                    {
                        _Timer = _Time;
                    }
                    else
                    {
                        Destroy(this.gameObject);
                    }
                }
            }
        }

        void Update()
        {
            Alarm();

        }

    }
}
