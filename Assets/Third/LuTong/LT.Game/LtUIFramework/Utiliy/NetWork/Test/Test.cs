/*
 *    描述:
 *          1.
 *
 *    开发人: 邓平
 */

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace DPNamespace
{
    public class Test : MonoBehaviour
    {
        private const string NetPlugin = "NetDll2Unity";

        #region Test untiy调用Dll

        [DllImport(NetPlugin)]
        private static extern int Add(int a, int b);

        private void TestAdd()
        {
            Debug.Log(Add(9, 9));
        }

        #endregion

        #region Test Dll调用Unity
        public delegate void TestDllCallUnity(IntPtr csOjb);

        [DllImport(NetPlugin)]
        private static extern int TestCallDll(IntPtr csObj, TestDllCallUnity callBack);

        private void TestCallDll()
        {
            var handleThis = GCHandle.Alloc(this);
            var csThisObj = GCHandle.ToIntPtr(handleThis);
            TestCallDll(csThisObj, DllCallBack);
        }

        private static void DllCallBack(IntPtr csObj)
        {
            Debug.Log("Dll Call Back");
        }


        #endregion



        void Start()
        {
            TestCallDll();
        }
    }
}
