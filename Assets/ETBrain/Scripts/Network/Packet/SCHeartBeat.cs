
using GameFramework;
using Msg;

namespace ETBrain
{
    public class SCHeartBeat : PacketBase
    {
        public override int Id
        {
            get
            {
                return 2;
            }
        }

        public override PacketType PacketType
        {
            get
            {
                return PacketType.ServerToClient;
            }
        }

        public static SCHeartBeat Create()
        {
            SCHeartBeat packet = ReferencePool.Acquire<SCHeartBeat>();
            if(packet.m_ExtensionObject == null)
            {
                packet.m_ExtensionObject = new HeartResponse();
            }
            return packet;
        }

        public override void Clear()
        {
            
        }
    }
}

