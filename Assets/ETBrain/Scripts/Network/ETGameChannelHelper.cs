
using System.IO;
using GameFramework.Event;
using GameFramework.Network;
using UnityGameFramework.Runtime;
using Google.Protobuf;
using GameFramework;
using Msg;

namespace ETBrain
{
    public class ETGameChannelHelper : INetworkChannelHelper
    {
        private INetworkChannel m_NetworkChannel = null;

        public int PacketHeaderLength
        {
            get
            {
                return 4;
            }
        }

        public Packet DeserializePacket(IPacketHeader packetHeader, Stream source, out object customErrorData)
        {
            customErrorData = null;

            SCPacketHeader scPacketHeader = packetHeader as SCPacketHeader;
            if (scPacketHeader == null)
            {
                Log.Warning("Packet header is invalid.");
                return null;
            }

            Packet packet = null;
            if (scPacketHeader.IsValid)
            {
                if (scPacketHeader.Id == 4)
                {
                    //BinaryReader r = new BinaryReader(source);
                    //byte[] message = r.ReadBytes((int)(source.Length - source.Position));

                    //string returnStr = string.Empty;
                    //for (int i = 0; i < message.Length; i++)
                    //{
                    //    returnStr += message[i].ToString("X2");
                    //}

                    SCData scData = SCData.Create();
                    ((DataResponse)scData.GetExtensionObject()).MergeFrom(source);
                    //Log.Info("data response {0}", scData);
                    packet = scData;
                }
                else
                {
                    packet = SCHeartBeat.Create();
                    Log.Info("heart response ");
                }
            }
            else
            {
                Log.Warning("Packet header is invalid.");
            }
            ReferencePool.Release(scPacketHeader);
            return packet;
        }

        public IPacketHeader DeserializePacketHeader(Stream source, out object customErrorData)
        {
            //Log.Info("DeserializePacketHeader {0}", source.Length);

            BinaryReader r = new BinaryReader(source);
            //byte[] message = r.ReadBytes((int)(source.Length - source.Position));

            //string returnStr = string.Empty;
            //for (int i = 0; i < message.Length; i++)
            //{
            //    returnStr += message[i].ToString("X2");
            //}
            //Log.Info("DeserializePacketHeader {0}" , returnStr);

            ushort messageLen = r.ReadUInt16();
            ushort mainId = r.ReadUInt16();

            //Log.Info("{0}   {1}", mainId, messageLen);

            customErrorData = null;
            SCPacketHeader header = ReferencePool.Acquire<SCPacketHeader>();
            header.PacketLength = messageLen - 2;
            header.Id = mainId;
            return header;
        }

        public void Initialize(INetworkChannel networkChannel)
        {
            m_NetworkChannel = networkChannel;

            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.NetworkConnectedEventArgs.EventId, OnNetworkConnected);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.NetworkClosedEventArgs.EventId, OnNetworkClosed);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.NetworkMissHeartBeatEventArgs.EventId, OnNetworkMissHeartBeat);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.NetworkErrorEventArgs.EventId, OnNetworkError);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.NetworkCustomErrorEventArgs.EventId, OnNetworkCustomError);

            m_NetworkChannel.RegisterHandler(new SCHeartBeatHandler());
            m_NetworkChannel.RegisterHandler(new SCDataHandler());
        }

        public void PrepareForConnecting()
        {
            //throw new System.NotImplementedException();
        }

        public bool SendHeartBeat()
        {
            if(m_NetworkChannel != null)
            {
                m_NetworkChannel.Send(CSHeartBeat.Create());
            }
            return false;
        }

        public bool Serialize<T>(T packet, Stream destination) where T : Packet
        {
            PacketBase packetImpl = packet as PacketBase;
            if (packetImpl == null)
            {
                Log.Warning("Packet is invalid.");
                return false;
            }

            if (packetImpl.PacketType != PacketType.ClientToServer)
            {
                Log.Warning("Send packet invalid.");
                return false;
            }

            byte[] temp = packetImpl.GetExtensionObject().ToByteArray();

            //string returnStr = string.Empty;
            //for (int i = 0; i < temp.Length; i++)
            //{
            //    returnStr += temp[i].ToString("X2");
            //}
            //Log.Info("Serialize {0}", returnStr);

            ushort msglen = (ushort)(temp.Length + 2);

            BinaryWriter writer = new BinaryWriter(destination);
            writer.Write(msglen);
            writer.Write((ushort)packetImpl.Id);
            writer.Write(temp);
            writer.Flush();

            ReferencePool.Release(packet);

            return true;
        }

        public void Shutdown()
        {
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.NetworkConnectedEventArgs.EventId, OnNetworkConnected);
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.NetworkClosedEventArgs.EventId, OnNetworkClosed);
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.NetworkMissHeartBeatEventArgs.EventId, OnNetworkMissHeartBeat);
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.NetworkErrorEventArgs.EventId, OnNetworkError);
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.NetworkCustomErrorEventArgs.EventId, OnNetworkCustomError);

            m_NetworkChannel = null;
        }


        private void OnNetworkConnected(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.NetworkConnectedEventArgs ne = (UnityGameFramework.Runtime.NetworkConnectedEventArgs)e;
            if (ne.NetworkChannel != m_NetworkChannel)
            {
                return;
            }

            Log.Info("Network channel '{0}' connected.", ne.NetworkChannel.Name);
        }

        private void OnNetworkClosed(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.NetworkClosedEventArgs ne = (UnityGameFramework.Runtime.NetworkClosedEventArgs)e;
            if (ne.NetworkChannel != m_NetworkChannel)
            {
                return;
            }

            Log.Info("Network channel '{0}' closed.", ne.NetworkChannel.Name);
        }

        private void OnNetworkMissHeartBeat(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.NetworkMissHeartBeatEventArgs ne = (UnityGameFramework.Runtime.NetworkMissHeartBeatEventArgs)e;
            if (ne.NetworkChannel != m_NetworkChannel)
            {
                return;
            }

            Log.Info("Network channel '{0}' miss heart beat '{1}' times.", ne.NetworkChannel.Name, ne.MissCount.ToString());

            if (ne.MissCount < 2)
            {
                return;
            }

            ne.NetworkChannel.Close();
        }

        private void OnNetworkError(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.NetworkErrorEventArgs ne = (UnityGameFramework.Runtime.NetworkErrorEventArgs)e;
            if (ne.NetworkChannel != m_NetworkChannel)
            {
                return;
            }

            Log.Info("Network channel '{0}' error, error code is '{1}', error message is '{2}'.", ne.NetworkChannel.Name, ne.ErrorCode.ToString(), ne.ErrorMessage);

            ne.NetworkChannel.Close();
        }

        private void OnNetworkCustomError(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.NetworkCustomErrorEventArgs ne = (UnityGameFramework.Runtime.NetworkCustomErrorEventArgs)e;
            if (ne.NetworkChannel != m_NetworkChannel)
            {
                return;
            }
        }
    }
}

