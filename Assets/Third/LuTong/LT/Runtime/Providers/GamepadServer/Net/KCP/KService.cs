/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：Kcp服务端
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace LT.Net
{
    public class KService : IDisposable
    {
        private UdpClient udpClient;
        private Dictionary<uint, KChannel> channels = new Dictionary<uint, KChannel>();
        private UnityEngine.Object syncRoot = new UnityEngine.Object();

        public Action<KChannel, byte[]> OnReadCallback;
        public Action<KChannel, ErrorCode> OnErrorCallback;

        public KService(IPEndPoint localEndPoint)
        {
            udpClient = new UdpClient(localEndPoint);
            udpClient.BeginReceive(OnRecv, null);
        }

        public void Update()
        {
            foreach (var c in channels.Values)
            {
                c.Update();
            }
        }

        public void BoardcastSend(byte[] buf)
        {
            foreach (var c in channels.Values)
            {
                c.Send(buf);
            }
        }

        private void OnRecv(IAsyncResult ar)
        {
            try
            {
                IPEndPoint remoteEndPoint = null;
                byte[] cache = udpClient.EndReceive(ar, ref remoteEndPoint);

                Debug.Log("udp recv:" + cache.Length);

                // 长度小于KCP.IKCP_OVERHEAD，不是kcp消息
                if (cache.Length < KCP.IKCP_OVERHEAD)
                {
                    udpClient.BeginReceive(OnRecv, null);
                    return;
                }

                uint conv = 0;
                KCP.ikcp_decode32u(cache, 0, ref conv);

                lock (syncRoot)
                {
                    if (!channels.TryGetValue(conv, out KChannel channel))
                    {
                        //Debug.LogError("recv:" + remoteEndPoint);
                        channel = new KChannel(conv, udpClient, remoteEndPoint)
                        {
                            Name = "Server",
                            OnReadCallback = this.OnReadCallback,
                            OnErrorCallback = this.OnErrorCallback
                        };
                        channels.Add(conv, channel);
                    }

                    channel.HandleRecv(cache);
                }

                udpClient.BeginReceive(OnRecv, null);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);

                if (udpClient != null)
                {
                    //异常引发可能是远端强制关闭,服务端需继续拉起接收
                    udpClient.BeginReceive(OnRecv, null);
                }
            }
        }

        public void RemoveChannel(uint conv)
        {
            lock (syncRoot)
            {
                if (channels.TryGetValue(conv, out KChannel channel))
                {
                    channels.Remove(conv);
                    Debug.Log($"Service remove conv:{conv}  Count:{channels.Count}");
                }
            }
        }

        public void Dispose()
        {
            foreach (var c in channels.Values)
            {
                c.Dispose();
            }

            udpClient.Dispose();
            udpClient = null;
        }
    }
}