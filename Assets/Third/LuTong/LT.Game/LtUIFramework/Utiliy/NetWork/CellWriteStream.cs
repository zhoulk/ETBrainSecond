/*
 *      描述:
 *          1. 
 *      
 *      修改者: 邓平
 */

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PracticeByDeng
{
    public class CellWriteStream
    {
        private const string NetPlugin = "NetDll2Unity";

        [DllImport(NetPlugin)]
        private static extern IntPtr CELLWriteStream_Create(int nSize);

        [DllImport(NetPlugin)]
        private static extern bool CELLWriteStream_WriteInt8(IntPtr cppStreamObj, sbyte n);
        [DllImport(NetPlugin)]
        private static extern bool CELLWriteStream_WriteInt16(IntPtr cppStreamObj, Int16 n);
        [DllImport(NetPlugin)]
        private static extern bool CELLWriteStream_WriteInt32(IntPtr cppStreamObj, Int32 n);
        [DllImport(NetPlugin)]
        private static extern bool CELLWriteStream_WriteInt64(IntPtr cppStreamObj, Int64 n);
        [DllImport(NetPlugin)]
        private static extern bool CELLWriteStream_WriteUint8(IntPtr cppStreamObj, byte n);
        [DllImport(NetPlugin)]
        private static extern bool CELLWriteStream_WriteUint16(IntPtr cppStreamObj, UInt16 n);
        [DllImport(NetPlugin)]
        private static extern bool CELLWriteStream_WriteUint32(IntPtr cppStreamObj, UInt32 n);
        [DllImport(NetPlugin)]
        private static extern bool CELLWriteStream_WriteUint64(IntPtr cppStreamObj, UInt64 n);
        [DllImport(NetPlugin)]
        private static extern bool CELLWriteStream_WriteFloat(IntPtr cppStreamObj, float n);
        [DllImport(NetPlugin)]
        private static extern bool CELLWriteStream_WriteDouble(IntPtr cppStreamObj, double n);

        [DllImport(NetPlugin)]
        private static extern bool CELLWriteStream_WriteString(IntPtr cppStreamObj, string pBuff, int len);

        [DllImport(NetPlugin)]
        private static extern bool CELLWriteStream_Release(IntPtr cppStreamObj);

        //---------------------------------------------------------------------

        private IntPtr cppStreamObj = IntPtr.Zero;


        public CellWriteStream(int nSize = 128)
        {
            cppStreamObj = CELLWriteStream_Create(nSize);
        }
        public void Release()
        {
            if (cppStreamObj != IntPtr.Zero)
                CELLWriteStream_Release(cppStreamObj);
            cppStreamObj = IntPtr.Zero;
        }

        public IntPtr getCppStreamObj
        {
            get { return cppStreamObj; }
        }

        public IntPtr cppObj
        {
            get { return cppStreamObj; }
        }

        public void SetNetCmd(NetCmd cmd)
        {
            WriteUInt16((UInt16)cmd);
        }

        public void Finish()
        {

        }

        public bool WriteInt8(sbyte n)
        {
            return CELLWriteStream_WriteInt8(cppStreamObj,n);
        }

        public bool WriteInt16(Int16 n)
        {
            return CELLWriteStream_WriteInt16(cppStreamObj, n);
        }

        public bool WriteInt32(Int32 n)
        {
            return CELLWriteStream_WriteInt32(cppStreamObj, n);
        }

        public bool WriteInt64(Int64 n)
        {
            return CELLWriteStream_WriteInt64(cppStreamObj, n);
        }

        public bool WriteUInt8(byte n)
        {
            return CELLWriteStream_WriteUint8(cppStreamObj, n);
        }

        public bool WriteUInt16(UInt16 n)
        {
            return CELLWriteStream_WriteUint16(cppStreamObj, n);
        }

        public bool WriteUInt32(UInt32 n)
        {
            return CELLWriteStream_WriteUint32(cppStreamObj, n);
        }

        public bool WriteUInt64(UInt64 n)
        {
            return CELLWriteStream_WriteUint64(cppStreamObj, n);
        }

        public bool WriteFloat(float n)
        {
            return CELLWriteStream_WriteFloat(cppStreamObj, n);
        }

        public bool WriteDouble(double n)
        {
            return CELLWriteStream_WriteDouble(cppStreamObj, n);
        }

        public void Write(byte[] b)
        {
            int len = b.Length;
            for (int i = 0; i < len; i++)
            {
                WriteUInt8(b[i]);
            }
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
