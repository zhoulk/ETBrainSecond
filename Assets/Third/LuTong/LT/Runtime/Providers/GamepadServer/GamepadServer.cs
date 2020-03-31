/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/12/11
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using LT.Net;
using LT;
using LT.MonoDriver;
using LT.EventDispatcher;

namespace LT.GamepadServer
{
    /// <summary>
    /// 手柄Server
    /// </summary>
    internal class GamepadServer : IGamepadServer, IUpdate, IOnDestroy
    {
        private KService kServer;
        private UdpClient udpClient;
        private IPackager packager;
        private IApplication application;
        private IMonoDriver monoDriver;

        private List<KChannel> removeChannels;

        /// <summary>
        /// 构造函数
        /// </summary>
        public GamepadServer(IApplication application, IMonoDriver monoDriver)
        {
            this.monoDriver = monoDriver;
            this.application = application;
            this.removeChannels = new List<KChannel>();

            InitKcpServer();
            InitUdpServer();
        }

        /// <summary>
        /// 初始化UdpServer
        /// </summary>
        private void InitUdpServer()
        {
            string msg = $"{NetworkHelper.LocalAddress()}-3900-Unity-Unity {UnityEngine.Application.productName}";

            this.udpClient = new UdpClient();
            this.monoDriver.StartCoroutine(OnBroadcastMessage(msg, new IPEndPoint(IPAddress.Broadcast, 7777)));
        }

        /// <summary>
        /// 广播消息
        /// </summary>
        IEnumerator OnBroadcastMessage(string content, IPEndPoint remoteEndPoint)
        {
            while (true)
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(content);
                udpClient.Send(bytes, bytes.Length, remoteEndPoint);
                yield return new WaitForSeconds(2f);
            }
        }

        private void InitKcpServer()
        {
            this.packager = new CShaperPackager();
            this.kServer = new KService(new IPEndPoint(IPAddress.Any, 3900))
            {
                OnReadCallback = this.OnServiceReadCallback,
                OnErrorCallback = this.OnServiceErrorCallback
            };
        }

        private void OnServiceReadCallback(KChannel channel, byte[] buf)
        {
            Debug.Log("kcp recv:" + buf.Length);
            var msg = packager.Decode(buf) as MessageKeyboard;

            //数据回传客户端
            channel.Send(buf);

            App.Make<IEventDispatcher>().Dispatch(new GamepadEventArgs(msg));
        }

        private void OnServiceErrorCallback(KChannel channel, ErrorCode error)
        {
            Debug.Log($"Service channel:{channel.Conv}  error:{error}");
            this.removeChannels.Add(channel);
        }

        /// <summary>
        /// 广播数据到所有已连接手柄
        /// </summary>
        /// <param name="json">数据</param>
        public void BroadcastToGamepad(byte[] bytes)
        {
            kServer.BoardcastSend(bytes);
        }

        /// <inheritdoc />
        public void Update()
        {
            if (kServer != null)
            {
                kServer.Update();

                foreach (var c in removeChannels)
                {
                    kServer.RemoveChannel(c.Conv);
                }

                removeChannels.Clear();
            }
        }

        /// <summary>
        /// Monobehvior OnDestroy
        /// </summary>
        public void OnDestroy()
        {
            kServer?.Dispose();
            udpClient?.Close();
        }
    }
}