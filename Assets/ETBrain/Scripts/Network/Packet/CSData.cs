using GameFramework;
using Msg;

namespace ETBrain
{
    public class CSData : PacketBase
    {
        public override int Id
        {
            get
            {
                return 3;
            }
        }

        public override PacketType PacketType
        {
            get
            {
                return PacketType.ClientToServer;
            }
        }

        public static CSData Create()
        {
            CSData packet = ReferencePool.Acquire<CSData>();
            if (packet.m_ExtensionObject == null)
            {
                packet.m_ExtensionObject = new DataRequest();
            }
            return packet;
        }

        public override void Clear()
        {
            ((DataRequest)GetExtensionObject()).Objs.Clear();
        }
    }
}
