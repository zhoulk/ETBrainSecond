
using GameFramework;
using Msg;

namespace ETBrain
{
    public class SCData : PacketBase
    {
        public override int Id
        {
            get
            {
                return 4;
            }
        }

        public override PacketType PacketType
        {
            get
            {
                return PacketType.ServerToClient;
            }
        }

        public static SCData Create()
        {
            SCData packet = ReferencePool.Acquire<SCData>();
            if(packet.m_ExtensionObject == null)
            {
                packet.m_ExtensionObject = new DataResponse();
            }
            return packet;
        }

        public override void Clear()
        {
            ((DataResponse)GetExtensionObject()).Objs.Clear();
        }
    }
}

