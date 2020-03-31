/*
 *      描述:
 *          1. 
 *      
 *      修改者: 邓平
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PracticeByDeng
{
    public class CellSendStream
    {
        //数据缓冲区
        private List<byte> _byteList = null;

        public CellSendStream(int nSize = 128)
        {
            _byteList = new List<byte>(nSize);
        }

        public void Write(byte[] data)
        {
            _byteList.AddRange(data);
        }

        public void WriteInt8(sbyte n)
        {
            _byteList.Add((byte) n);
        }

        public void WriteInt16(Int16 n)
        {
            Write(BitConverter.GetBytes(n));
        }

        public void WriteInt32(Int32 n)
        {
            Write(BitConverter.GetBytes(n));
        }

        public void WriteInt64(Int64 n)
        {
            Write(BitConverter.GetBytes(n));
        }

        public void SetNetCmd(NetCmd cmd)
        {
            WriteUInt16((UInt16) cmd);
        }
        public byte[] Array
        {
            get { return _byteList.ToArray(); }
        }

        public void Finish()
        {
            if (_byteList.Count > UInt16.MaxValue)
            {
                //Error
                Debug.LogError("数据大于  Uint16最大字节 数据应该分包");
            }
            //写入消息头
            UInt16 len = (UInt16)_byteList.Count;
            //Uint16   2个字节
            len += 2; 
            _byteList.InsertRange(0,BitConverter.GetBytes(len));
        }

        public void Release()
        {

        }

        public void WriteUInt8(byte n)
        {
            _byteList.Add(n);
        }

        public void WriteUInt16(UInt16 n)
        {
            Write(BitConverter.GetBytes(n));
        }

        public void WriteUInt32(UInt32 n)
        {
            Write(BitConverter.GetBytes(n));
        }

        public void WriteUInt64(UInt64 n)
        {
            Write(BitConverter.GetBytes(n));
        }

        public void WriteFloat(float n)
        {
            Write(BitConverter.GetBytes(n));
        }

        public void WriteDouble(double n)
        {
            Write(BitConverter.GetBytes(n));
        }

        /// <summary>
        /// UTF8
        /// </summary>
        /// <param name="s"></param>
        public void WriteString(string s)
        {
            byte[] buff = Encoding.UTF8.GetBytes(s);
            WriteUInt32((UInt32)buff.Length + 1/*最后多希尔一个结束符*/);
            Write(buff);
            //字符串的结束符
            WriteUInt8(0);
        }

        public void WriteBytes(byte[] data)
        {
            WriteUInt32((UInt32)data.Length);
            Write(data);
        }

        public void WriteInt32s(Int32[] data)
        {
            WriteUInt32((UInt32)data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                WriteInt32(data[i]);
            }
        }

    }
}
