using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Network
{
    /// <summary>
    /// 网络消息包头接口。
    /// </summary>
    public interface IPacketHeader
    {
        /// <summary>
        /// 获取网络消息包长度。
        /// </summary>
        int PacketLength
        {
            get;
        }
    }
}

