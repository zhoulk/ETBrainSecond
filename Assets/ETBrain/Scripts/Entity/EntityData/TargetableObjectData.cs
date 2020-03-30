
namespace ETBrain
{
    public class TargetableObjectData : EntityData
    {
        private int m_HP = 0;

        private int m_Attack = 0;

        public TargetableObjectData(int entityId, int typeId)
             : base(entityId, typeId)
        {

        }

        /// <summary>
        /// 当前生命。
        /// </summary>
        public int HP
        {
            get
            {
                return m_HP;
            }
            set
            {
                m_HP = value;
            }
        }

        public int Attack
        {
            get
            {
                return m_Attack;
            }
            set
            {
                m_Attack = value;
            }
        }
    }
}

