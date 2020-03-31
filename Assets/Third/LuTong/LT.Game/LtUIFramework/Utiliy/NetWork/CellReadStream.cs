/*
 *      描述:
 *          1. 
 *      
 *      修改者: 邓平
 */

using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace PracticeByDeng
{
    public class CellReadStream
    {
        private const string NetPlugin = "NetDll2Unity";

        [DllImport(NetPlugin)]
        private static extern IntPtr CELLReadStream_Create(IntPtr data, int len);

        [DllImport(NetPlugin)]
        private static extern sbyte CELLReadStream_ReadInt8(IntPtr cppStreamObj);

        [DllImport(NetPlugin)]
        private static extern Int16 CELLReadStream_ReadInt16(IntPtr cppStreamObj);
        [DllImport(NetPlugin)]
        private static extern Int32 CELLReadStream_ReadInt32(IntPtr cppStreamObj);
        [DllImport(NetPlugin)]
        private static extern Int64 CELLReadStream_ReadInt64(IntPtr cppStreamObj);
        [DllImport(NetPlugin)]
        private static extern byte CELLReadStream_ReadUInt8(IntPtr cppStreamObj);

        [DllImport(NetPlugin)]
        private static extern UInt16 CELLReadStream_ReadUInt16(IntPtr cppStreamObj);
        [DllImport(NetPlugin)]
        private static extern UInt32 CELLReadStream_ReadUInt32(IntPtr cppStreamObj);
        [DllImport(NetPlugin)]
        private static extern UInt64 CELLReadStream_ReadUInt64(IntPtr cppStreamObj);
        [DllImport(NetPlugin)]
        private static extern float CELLReadStream_ReadFloat(IntPtr cppStreamObj);
        [DllImport(NetPlugin)]
        private static extern double CELLReadStream_ReadDouble(IntPtr cppStreamObj);

        [DllImport(NetPlugin)]
        private static extern UInt32 CELLReadStream_ReadOnlyUInt32(IntPtr cppStreamObj);

        [DllImport(NetPlugin)]
        private static extern bool CELLReadStream_ReadString(IntPtr cppStreamObj, StringBuilder pBuff, int len);

        [DllImport(NetPlugin)]
        private static extern bool CELLReadStream_Release(IntPtr cppStreamObj);

        //-----------------------------------------------------------

        private IntPtr cppStreamObj = IntPtr.Zero;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">消息回调传入的消息数据指针</param>
        /// <param name="len">数据字节长度</param>
        public CellReadStream(IntPtr data, int len)
        {
            cppStreamObj = CELLReadStream_Create(data, len);
        }

        public void Release()
        {
            if (cppStreamObj != IntPtr.Zero)
                CELLReadStream_Release(cppStreamObj);
            cppStreamObj = IntPtr.Zero;
        }

        public NetCmd GetNetCmd()
        {
            NetCmd cmd;
            cmd = (NetCmd)ReadUInt16();
            return cmd;
        }

        public sbyte ReadInt8()
        {
            return CELLReadStream_ReadInt8(cppStreamObj);
        }

        public Int16 ReadInt16()
        {
            return CELLReadStream_ReadInt16(cppStreamObj);
        }

        public Int32 ReadInt32()
        {
            return CELLReadStream_ReadInt32(cppStreamObj);
        }

        public Int64 ReadInt64()
        {
            return CELLReadStream_ReadInt64(cppStreamObj);
        }

        public byte ReadUInt8()
        {
            return CELLReadStream_ReadUInt8(cppStreamObj);
        }

        public UInt16 ReadUInt16()
        {
            return CELLReadStream_ReadUInt16(cppStreamObj);
        }

        public UInt32 ReadUInt32()
        {
            return CELLReadStream_ReadUInt32(cppStreamObj);
        }

        public UInt64 ReadUInt64()
        {
            return CELLReadStream_ReadUInt64(cppStreamObj);
        }

        public float ReadFloat()
        {
            return CELLReadStream_ReadFloat(cppStreamObj);
        }

        public double ReadDouble()
        {
            return CELLReadStream_ReadDouble(cppStreamObj);
        }

        public UInt32 ReadOnlyInt32()
        {
            return CELLReadStream_ReadOnlyUInt32(cppStreamObj);
        }

        public string ReadString()
        {
            int len = (int) ReadInt32();
            byte[] buffer = new byte[len];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = ReadUInt8();
            }
            return Encoding.UTF8.GetString(buffer, 0, len);

            {   
                //int len = (int) ReadOnlyInt32();
                //StringBuilder sb = new StringBuilder(len);
                //CELLReadStream_ReadString(cppStreamObj, sb, len);
                //return sb.ToString();
            }


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

    }
}
