
using GameFramework;
using Msg;

namespace ETBrain
{
    public class CSHeartBeat : PacketBase
    {
        public override int Id
        {
            get
            {
                return 1;
            }
        }

        public override PacketType PacketType
        {
            get
            {
                return PacketType.ClientToServer;
            }
        }

        public static CSHeartBeat Create()
        {
            CSHeartBeat packet = ReferencePool.Acquire<CSHeartBeat>();
            if(packet.m_ExtensionObject == null)
            {
                packet.m_ExtensionObject = new HeartRequest();
            }
            return packet;
        }

        public override void Clear()
        {
            
        }
    }
}

