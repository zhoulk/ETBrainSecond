/*
 *      描述:
 *          1. 
 *      
 *      修改者: 邓平
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace PracticeByDeng
{
    public class CellRecvStream
    {
        //数据缓冲区
        private byte[] _buffer = null;

        #region C++解析

        public CellRecvStream(IntPtr data ,int len)
        {
            _buffer = new byte[len];
            Marshal.Copy(data,_buffer,0,len);
        }

        #endregion


        #region C#解析
        //
        private int _nReadPos = 0;



        public CellRecvStream(byte[] data)
        {
            _buffer = data;
            _nReadPos = 0;
            ReadUInt16();
        }


        private void Pop(int n)
        {
            _nReadPos += n;
        }

        private bool canRead(int n)
        {
            return _buffer.Length - _nReadPos >= n;
        }

        public NetCmd GetNetCmd()
        {
            NetCmd cmd;
            cmd = (NetCmd)ReadUInt16();
            return cmd;
        }

        public sbyte ReadInt8()
        {
            if (canRead(1))
            {
                sbyte n = 0;
                n = (sbyte) _buffer[_nReadPos];
                Pop(1);
                return n;
            }

            Debug.LogError("读取 Int8 数据出错");
            return 0;
        }

        public Int16 ReadInt16()
        {
            if (canRead(2))
            {
                Int16 n = 0;
                n = BitConverter.ToInt16(_buffer, _nReadPos);
                Pop(2);
                return n;
            }

            Debug.LogError("读取 Int16 数据出错");
            return 0;
        }

        public Int32 ReadInt32()
        {
            if (canRead(4))
            {
                Int32 n = 0;
                n = BitConverter.ToInt32(_buffer, _nReadPos);
                Pop(4);
                return n;
            }

            Debug.LogError("读取 Int32 数据出错");
            return 0;
        }

        public Int64 ReadInt64()
        {
            if (canRead(8))
            {
                Int64 n = 0;
                n = BitConverter.ToInt64(_buffer, _nReadPos);
                Pop(8);
                return n;
            }

            Debug.LogError("读取 Int64 数据出错");
            return 0;
        }

        public sbyte ReadUInt8()
        {
            if (canRead(1))
            {
                sbyte n = 0;
                n = (sbyte) _buffer[_nReadPos];
                Pop(1);
                return n;
            }

            Debug.LogError("读取 UInt8 数据出错");
            return 0;
        }

        public UInt16 ReadUInt16()
        {
            if (canRead(2))
            {
                UInt16 n = 0;
                n = BitConverter.ToUInt16(_buffer, _nReadPos);
                Pop(2);
                return n;
            }

            Debug.LogError("读取 UInt16 数据出错");
            return 0;
        }

        public UInt32 ReadUInt32()
        {
            if (canRead(4))
            {
                UInt32 n = 0;
                n = BitConverter.ToUInt32(_buffer, _nReadPos);
                Pop(4);
                return n;
            }

            Debug.LogError("读取 UInt32 数据出错");
            return 0;
        }

        public UInt64 ReadUInt64()
        {
            if (canRead(8))
            {
                UInt64 n = 0;
                n = BitConverter.ToUInt64(_buffer, _nReadPos);
                Pop(8);
                return n;
            }

            Debug.LogError("读取 UInt64 数据出错");
            return 0;
        }

        public float ReadFloat()
        {
            if (canRead(4))
            {
                float n = 0;
                n = BitConverter.ToSingle(_buffer, _nReadPos);
                Pop(4);
                return n;
            }

            Debug.LogError("读取 float 数据出错");
            return 0;
        }

        public double ReadDouble()
        {
            if (canRead(8))
            {
                double n = 0;
                n = BitConverter.ToDouble(_buffer, _nReadPos);
                Pop(8);
                return n;
            }

            Debug.LogError("读取 double 数据出错");
            return 0;
        }

        public string ReadString()
        {
            string s = string.Empty;
            int len = ReadInt32();

            if (canRead(len) && len > 0)
            {
                s = Encoding.UTF8.GetString(_buffer, _nReadPos, len);
                Pop(len);
                return s;
            }

            Debug.LogError("读取 string 数据出错");
            return null;
        }

        public Int32[] ReadInt32s()
        {
            int len = ReadInt32();
            Int32[] data = null;
            if (len < 0)
            {
                Debug.LogError("读取 Int32[] 数据出错");
                return null;
            }

            data = new int[len];
            for (int i = 0; i < len; i++)
            {
                data[i] = ReadInt32();
            }

            return data;
        }

        #endregion
    }
}
