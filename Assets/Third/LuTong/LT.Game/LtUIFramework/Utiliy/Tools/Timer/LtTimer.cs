/*
*    描述:
*          1.计时器
*
*    开发人: 邓平
*/
using UnityEngine;

namespace LtFramework.Util.Tools
{
    public class LtTimer
    {
        private float _Time;
        private float _Timer;

        public LtTimer(float timer = 0)
        {
            _Timer = timer;
            _Time = timer;
        }

        public float time
        {
            get
            {
                return _Time;
            }
        }

        public float timer
        {
            get { return _Timer; }
        }

        public void SetTimer(float time)
        {
            _Time = time;
            _Timer = time;
        }

        public void ResetTimer()
        {
            _Timer = _Time;
        }


        public bool Alarm(bool loop = true)
        {
            _Timer -= Time.deltaTime;
            if (_Timer <= 0)
            {
                if (loop)
                {
                    _Timer = _Time;
                }
                else
                {
                    _Timer = 0;
                }

                return true;
            }

            return false;
        }

        public bool AlarmUnSacleTime(bool loop = true)
        {
            Debug.Log("start :" +_Timer);

            _Timer -= Time.unscaledDeltaTime;
            Debug.Log("end :"+_Timer);
            if (_Timer <= 0)
            {
                if (loop)
                {
                    _Timer = _Time;
                }
                else
                {
                    _Timer = 0;
                }

                return true;
            }

            return false;
        }


        public bool AlarmFixTime(bool loop = true)
        {
            _Timer -= Time.fixedDeltaTime;
            if (_Timer <= 0)
            {
                if (loop)
                {
                    _Timer = _Time;
                }
                else
                {
                    _Timer = 0;
                }

                return true;
            }

            return false;
        }

        public bool AlarmFixTimeUnSacleTime(bool loop = true)
        {
            _Timer -= Time.fixedUnscaledDeltaTime;
            if (_Timer <= 0)
            {
                if (loop)
                {
                    _Timer = _Time;
                }
                else
                {
                    _Timer = 0;
                }

                return true;
            }

            return false;

        }


        public bool AlarmOnce()
        {
            _Timer -= Time.deltaTime;
            if (_Timer <= 0)
            {
                _Timer = float.MaxValue;
                return true;
            }

            return false;
        }

    }

}
