/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：kcp信道
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LT;

namespace LT.Net
{
    /// <summary>
    /// kcp信道
    /// </summary>
    public class KChannel : IDisposable
    {
        /// <summary>
        /// 是否被销毁
        /// </summary>
        protected bool disposed;

        /// <summary>
        /// Udp客户端
        /// </summary>
        private UdpClient udpClient;

        /// <summary>
        /// 远程端点
        /// </summary>
        private IPEndPoint remoteEndPoint;

        /// <summary>
        /// Kcp
        /// </summary>
        private KCP kcp;

        /// <summary>
        /// 上一次刷新时间
        /// </summary>
        private uint lastRecvTime;

        /// <summary>
        /// 会话Id
        /// </summary>
        private uint conv;

        /// <summary>
        /// 创建时间
        /// </summary>
        private uint createTime;

        /// <summary>
        /// 是否连接
        /// </summary>
        private bool isConnected;

        /// <summary>
        /// 消息回调
        /// </summary>
        public Action<KChannel, byte[]> OnReadCallback;

        /// <summary>
        /// 时间戳回掉
        /// </summary>
        public Action<KChannel, List<uint>> OnTimeStampCallback;

        /// <summary>
        /// 错误码回调
        /// </summary>
        public Action<KChannel, ErrorCode> OnErrorCallback;

        /// <summary>
        /// 远程端口
        /// </summary>
        public IPEndPoint RemoteEndPoint => remoteEndPoint;

        /// <summary>
        /// 频道名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// KCP信道
        /// </summary>
        /// <param name="conv">会话id</param>
        /// <param name="udpClient">下层协议</param>
        /// <param name="remoteEndPoint">远程端点</param>
        public KChannel(uint conv, UdpClient udpClient, IPEndPoint remoteEndPoint)
        {
            this.conv = conv;
            this.udpClient = udpClient;
            this.remoteEndPoint = remoteEndPoint;
            this.createTime = (uint)SystemTime.ClientNow();

            kcp = new KCP(conv, KcpOutput);
            kcp.NoDelay(1, 10, 1, 1);
            kcp.WndSize(256, 256);
            kcp.SetMtu(470);
        }

        private void KcpOutput(byte[] bytes, int size)
        {
            Debug.Log($"{Name} UdpSend:{remoteEndPoint}:{size}");
            udpClient.Send(bytes, size, remoteEndPoint);
        }

        private void KcpSend(byte[] buffer)
        {
            if (this.disposed)
            {
                return;
            }

            kcp.Send(buffer);
        }

        public void Send(byte[] buffer)
        {
            if (kcp != null)
            {
                // 检查等待发送的消息，如果超出两倍窗口大小，应该断开连接
                if (kcp.WaitSnd() > 256 * 2)
                {
                    this.OnError(ErrorCode.ERR_KcpWaitSendSizeTooLarge);
                    return;
                }
            }

            KcpSend(buffer);
        }

        public void Update()
        {
            if (disposed) return;

            uint timeNow = (uint)SystemTime.ClientNow();

            try
            {
                kcp.Update(timeNow);
            }
            catch (Exception)
            {
                //网络无法访问
                this.OnError(ErrorCode.ERR_SocketError);
                return;
            }

            if (!isConnected)
            {
                // 没连接上则报错
                if (timeNow - this.createTime > 3 * 1000)
                {
                    this.OnError(ErrorCode.ERR_KcpCantConnect);
                    this.Dispose();
                    return;
                }
            }
            else
            {
                //因是异步接收，lastRecvTime有可能比timeNow刷新慢。
                if (timeNow < this.lastRecvTime)
                    return;

                //  超时断开连接
                if ((timeNow - this.lastRecvTime) > 10 * 1000)
                {
                    Debug.LogError($"{Name} timeNow:{timeNow}  lastRecvTime:{lastRecvTime}  ts:{(timeNow - this.lastRecvTime)}");

                    this.OnError(ErrorCode.ERR_KcpChannelTimeout);
                    this.Dispose();
                    return;
                }
            }
        }

        /// <summary>
        /// 处理接收到的数据
        /// </summary>
        /// <param name="cache"> 接收到的数据</param>
        public void HandleRecv(byte[] cache)
        {
            if (this.disposed)
            {
                return;
            }

            kcp.Input(cache);

            while (true)
            {
                try
                {
                    if (this.disposed)
                    {
                        return;
                    }

                    int size = kcp.PeekSize();

                    if (size < 0)
                    {
                        return;
                    }

                    if (size == 0)
                    {
                        //发生错误，应该断开
                        this.OnError((ErrorCode)SocketError.NetworkReset);
                        this.Dispose();
                        return;
                    }

                    if (!isConnected)
                    {
                        this.isConnected = true;
                    }

                    var buffer = new byte[size];
                    kcp.Recv(buffer);

                    this.lastRecvTime = (uint)SystemTime.ClientNow();

                    this.OnRead(buffer);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        protected void OnError(ErrorCode e)
        {
            this.OnErrorCallback?.Invoke(this, e);
        }

        protected void OnRead(byte[] buffer)
        {
            this.OnReadCallback?.Invoke(this, buffer);
        }

        public uint Conv => conv;
        public bool Connected => isConnected;

        public virtual void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;
            kcp = null;
        }
    }
}