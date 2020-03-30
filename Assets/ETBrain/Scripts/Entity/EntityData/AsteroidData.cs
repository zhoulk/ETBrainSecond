
namespace ETBrain
{
    public class AsteroidData : TargetableObjectData
    {
        private int m_MaxHP = 0;

        private float m_Speed = 0f;

        private float m_AngularSpeed = 0f;

        public AsteroidData(int entityId, int typeId)
                : base(entityId, typeId)
        {
            m_MaxHP = 10;
            HP = 10;
            Attack = 10;
            m_Speed = 5;
            m_AngularSpeed = 15;
        }

        public int MaxHP
        {
            get
            {
                return m_MaxHP;
            }
        }

        public float Speed
        {
            get
            {
                return m_Speed;
            }
        }

        public float AngularSpeed
        {
            get
            {
                return m_AngularSpeed;
            }
        }
    }
}

