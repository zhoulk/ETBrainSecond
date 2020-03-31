/*
 *      描述:
 *          1. 
 *      
 *      修改者: 邓平
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PracticeByDeng
{
    public enum NetCmd
    {
        LOGIN,
        LOGIN_RESULT,
        LOGOUT,
        LOGOUT_RESULT,
        NEW_USER_JOIN,
        C2S_HEART,
        S2C_HEART,
        CERROR
    };

    public class CellClientNode : CellTcpClient
    {

        public string Ip = "127.0.0.1";
        public short Port = 4567;
        private bool isConnet = false;

        void Start()
        {
            Create();
            isConnet = Connect(Ip, Port);
            if (isConnet)
            {
                Debug.Log("连接成功");
            }
            else
            {
                Debug.LogError("连接失败");
            }

        }

        void Update()
        {
            if (isConnet)
            {
                OnRun();

                SendMsg_2();
            }
        }

        void SendMsg()
        {
            CellWriteStream s = new CellWriteStream(256);
            s.SetNetCmd(NetCmd.LOGOUT);

            s.WriteInt8(1);
            s.WriteInt16(2);
            s.WriteInt32(3);
            s.WriteFloat(4.3f);
            s.WriteDouble(5.6f);

            string str = "测试字符串___1 client";
            s.WriteString(str);
            string a = "测试字符串___2 ahah";
            s.WriteString(a);
            int[] b = { 1, 2, 3, 4, 5 };
            s.WriteInt32s(b);
            s.Finish();

            SendData(s);
            s.Release();
        }

        void SendMsg_2()
        {
            CellSendStream s = new CellSendStream(256);
            s.SetNetCmd(NetCmd.LOGOUT);

            s.WriteInt8(1);
            s.WriteInt16(2);
            s.WriteInt32(3);
            s.WriteFloat(4.3f);
            s.WriteDouble(5.6f);

            string str = "测试字符串___1 client";
            s.WriteString(str);
            string a = "测试字符串___2 ahah";
            s.WriteString(a);
            int[] b = { 1, 2, 3, 4, 5 };
            s.WriteInt32s(b);
            s.Finish();

            SendData(s);
            s.Release();
        }

        public override void OnNetMsgBytes(IntPtr data, int len)
        {
            base.OnNetMsgBytes(data, len);
            CellReadStream r = new CellReadStream(data, len);
            r.ReadUInt16();
            r.GetNetCmd();
            Debug.Log(r.ReadInt8());
            Debug.Log(r.ReadInt16());
            Debug.Log(r.ReadInt32());
            Debug.Log(r.ReadFloat());
            Debug.Log(r.ReadDouble());
            Debug.Log(r.ReadString());
            Debug.Log(r.ReadString());
            Int32[] arr = r.ReadInt32s();
            for (int i = 0; i < arr.Length; i++)
            {
                Debug.Log(arr[i]);
            }

            r.Release();
        }

        public override void OnNetMsgBytes(byte[] data)
        {
            //base.OnNetMsgBytes(data);
            //CellRecvStream r = new CellRecvStream(data);
            //r.GetNetCmd();
            //Debug.Log(r.ReadInt8());
            //Debug.Log(r.ReadInt16());
            //Debug.Log(r.ReadInt32());
            //Debug.Log(r.ReadFloat());
            //Debug.Log(r.ReadDouble());
            //Debug.Log(r.ReadString());
            //Debug.Log(r.ReadString());
            //Int32[] arr = r.ReadInt32s();
            //for (int i = 0; i < arr.Length; i++)
            //{
            //    Debug.Log(arr[i]);
            //}
        }



        private void OnDestroy()
        {
            Close();
            isConnet = false;
        }

	}
}
