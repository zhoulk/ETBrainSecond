
namespace ETBrain
{
    public class BulletData : EntityData
    {
        private int m_OwnerId = 0;

        private int m_Attack = 0;

        private float m_Speed = 0f;

        public BulletData(int entityId, int typeId, int ownId)
            : base(entityId, typeId) {
            m_OwnerId = ownId;
            m_Attack = 10;
            m_Speed = 10;
        }

        /// <summary>
        /// 拥有者编号。
        /// </summary>
        public int OwnerId
        {
            get
            {
                return m_OwnerId;
            }
        }

        public int Attack
        {
            get
            {
                return m_Attack;
            }
        }

        public float Speed
        {
            get
            {
                return m_Speed;
            }
        }
    }
}

