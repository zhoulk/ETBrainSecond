
using UnityEngine;
using UnityGameFramework.Runtime;

namespace ETBrain
{
    public partial class BuiltinDataComponent : GameFrameworkComponent
    {
        [SerializeField]
        private ChannelType m_channel = ChannelType.Unknown;

        [SerializeField]
        private Enviroment m_env = Enviroment.Develop;

        public ChannelType Channel
        {
            get
            {
                return m_channel;
            }
        }

        public Enviroment Env
        {
            get
            {
                return m_env;
            }
        }
    }
}
