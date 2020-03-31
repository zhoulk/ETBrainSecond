/*
 *    描述:
 *          1. UI操作更新脚本
 *
 *    开发人: 邓平
 */
using System.Collections.Generic;
using UnityEngine;

namespace LtFramework.UI
{
    public class InputEventSystem
    {
        private readonly List<IBaseUIForm> _CanCtrlUIForm = new List<IBaseUIForm>();

        object _Mutex = new object();

        /// <summary>
        /// 添加控制UI
        /// </summary>
        /// <param UIName="uiFrom"></param>
        internal void AddCtrlUIForm(IBaseUIForm uiFrom)
        {
            if (!_CanCtrlUIForm.Contains(uiFrom))
            {
                lock (_Mutex)
                {
                    _CanCtrlUIForm.Add(uiFrom);
                }
            }
        }

        /// <summary>
        /// 移除控制UI
        /// </summary>
        /// <param UIName="uiFrom"></param>
        internal void RemoveCtrlUIForm(IBaseUIForm uiFrom)
        {
            if (_CanCtrlUIForm.Contains(uiFrom))
            {
                Debug.LogWarning("移除:" + uiFrom.name);
                lock (_Mutex)
                {
                    _CanCtrlUIForm.Remove(uiFrom);
                }
            }

        }

        /// <summary>
        /// 获得当前可操作UI
        /// </summary>
        /// <returns></returns>
        internal List<IBaseUIForm> CtrlUIForm()
        {
            return _CanCtrlUIForm;
        }

        internal void Update()
        {
            lock (_Mutex)
            {
                if (_CanCtrlUIForm.Count > 0)
                {
                    for (int i = 0; i < _CanCtrlUIForm.Count; i++)
                    {
                        if (_CanCtrlUIForm[i] != null)
                        {
                            _CanCtrlUIForm[i].InputUpdate();
                        }
                        else
                        {
                            _CanCtrlUIForm.RemoveAt(i);
                        }
                    }

                    for (int i = _CanCtrlUIForm.Count - 1; i >= 0; i--)
                    {

                    }
                }
            }

        }


        internal void OnGUI()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.F1))
            {
                for (int i = 0; i < _CanCtrlUIForm.Count; i++)
                {
                    Debug.LogWarning(_CanCtrlUIForm[i].name + "=======UI面板=======  " + i);
                }
            }
#endif

        }

    }
}
