using GameFramework.Network;
using Google.Protobuf;

namespace ETBrain
{
    public abstract class PacketBase : Packet
    {
        protected IMessage m_ExtensionObject;

        public PacketBase()
        {
            m_ExtensionObject = null;
        }

        public abstract PacketType PacketType
        {
            get;
        }

        public IMessage GetExtensionObject()
        {
            return m_ExtensionObject;
        }
    }
}

