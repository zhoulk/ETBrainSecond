/*
 *  Title 
/*
 *      描述:
 *          1. 
 *      
 *      修改者: 邓平
 */

using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace PracticeByDeng
{
    public enum AddressFamily
    {
        //AF_UNSPEC = 0, // unspecified
        //AF_UNIX = 1, // local to host (pipes, portals)
        AF_INET = 2, // internetwork: UDP, TCP, etc.
        //AF_IMPLINK = 3, // arpanet imp addresses
        //AF_PUP = 4, // pup protocols: e.g. BSP
        //AF_CHAOS = 5, // mit CHAOS protocols
        //AF_NS = 6, // XEROX NS protocols
        //AF_IPX = AF_NS, // IPX protocols: IPX, SPX, etc.
        //AF_ISO = 7, // ISO protocols
        //AF_OSI = AF_ISO, // OSI is ISO
        //AF_ECMA = 8, // european computer manufacturers
        //AF_DATAKIT = 9, // datakit protocols
        //AF_CCITT = 10, // CCITT protocols, X.25 etc
        //AF_SNA = 11, // IBM SNA
        //AF_DECnet = 12, // DECnet
        //AF_DLI = 13, // Direct data link interface
        //AF_LAT = 14, // LAT
        //AF_HYLINK = 15, // NSC Hyperchannel
        //AF_APPLETALK = 16, // AppleTalk
        //AF_NETBIOS = 17, // NetBios-style addresses
        //AF_VOICEVIEW = 18, // VoiceView
        //AF_FIREFOX = 19, // Protocols from Firefox
        //AF_UNKNOWN1 = 20, // Somebody is using this!
        //AF_BAN = 21, // Banyan
        //AF_ATM = 22, // Native ATM Services
        //AF_INET6 = 23, // Internetwork Version 6
        //AF_CLUSTER = 24, // Microsoft Wolfpack
        //AF_12844 = 25, // IEEE 1284.4 WG AF
        //AF_IRDA = 26, // IrDA
        //AF_NETDES = 28, // Network Designers OSI & gateway
    }

    public class CellTcpClient : MonoBehaviour
    {
        public delegate void OnNetMsgCallBack(IntPtr csOjb, IntPtr data, int len);

        [MonoPInvokeCallback(typeof(OnNetMsgCallBack))]
        private static void OnNetMsgCallBack1(IntPtr csOjb, IntPtr data, int len)
        {
            //将c++ 传入的数据转换成c# 字节数组
            GCHandle handle = GCHandle.FromIntPtr(csOjb);
            CellTcpClient obj = handle.Target as CellTcpClient;
            if (obj)
            {
                byte[] buff = new byte[len];
                Marshal.Copy(data,buff,0,len);

                obj.OnNetMsgBytes(data,len);
                obj.OnNetMsgBytes(buff);
            }
        }

        private const string NetPlugin = "NetDll2Unity";
        [DllImport(NetPlugin)]
        private static extern IntPtr CELLClient_Create(IntPtr csObj, OnNetMsgCallBack cb,int addressFamily, int sendSize, int recvSize);

        [DllImport(NetPlugin)]
        private static extern bool CELLClient_Connect(IntPtr cppClientObj, string ip, short port);

        [DllImport(NetPlugin)]
        private static extern bool CELLClient_OnRun(IntPtr cppClientObj);

        [DllImport(NetPlugin)]
        private static extern void CELLClient_Close(IntPtr cppClientObj);

        [DllImport(NetPlugin)]
        private static extern int CELLClient_Send(IntPtr cppClientObj, byte[] data, int len);

        [DllImport(NetPlugin)]
        private static extern int CELLClient_SendWriteStream(IntPtr cppClientObj, IntPtr cppStreamObj);

        /*----------------------------------------------------------------------------------------*/

        private GCHandle _handleThis; 
        //this 对象的指针 在c++消息回调中传回
        private IntPtr _csThisObj = IntPtr.Zero;
        // C++ NativeTcpClient 对象指针
        private IntPtr _cppClientObj = IntPtr.Zero;

        public void Create(int sendSize = 10240,
            int recvSize = 10240, AddressFamily addressFamily = AddressFamily.AF_INET)
        {
            _handleThis = GCHandle.Alloc(this);
            _csThisObj = GCHandle.ToIntPtr(_handleThis);
            _cppClientObj = CELLClient_Create(_csThisObj, OnNetMsgCallBack1, (int) addressFamily, sendSize,
                recvSize);
        }

        public bool Connect(string ip, short port)
        {
            if (_cppClientObj == IntPtr.Zero) return false;

            return CELLClient_Connect(_cppClientObj, ip, port);
        }

        public bool OnRun()
        {
            if (_cppClientObj == IntPtr.Zero) return false;

            return CELLClient_OnRun(_cppClientObj);
        }

        public void Close()
        {
            if (_cppClientObj == IntPtr.Zero) return;

            CELLClient_Close(_cppClientObj);
            _cppClientObj = IntPtr.Zero;
            _handleThis.Free();
        }

        public int SendData( byte[] data)
        {
            if (_cppClientObj == IntPtr.Zero) return 0;

            return CELLClient_Send(_cppClientObj,data, data.Length);
        }

        public int SendData(CellSendStream ss)
        {
            return SendData(ss.Array);
        }

        public int SendData(CellWriteStream ws)
        {
            if (_cppClientObj == IntPtr.Zero&& ws != null) return 0;
            return CELLClient_SendWriteStream(_cppClientObj, ws.cppObj);
        }


        public virtual void OnNetMsgBytes(IntPtr data,int len)
        {

        }

        public virtual void OnNetMsgBytes(byte[] data)
        {
            Debug.Log("OnNetMsgBytes: len  =  " + data.Length);
        }

    }
}
