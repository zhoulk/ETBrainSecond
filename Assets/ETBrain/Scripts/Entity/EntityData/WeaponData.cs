
using UnityGameFramework.Runtime;

namespace ETBrain
{
    public class WeaponData : EntityData
    {
        private int m_OwnerId = 0;

        private Fix64 m_AttackInterval = Fix64.Zero;

        public WeaponData(int entityId, int typeId, int owenId)
            : base(entityId, typeId)
        {
            m_OwnerId = owenId;
            m_AttackInterval = Fix64.FromRaw(203);
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

        /// <summary>
        /// 攻击间隔。
        /// </summary>
        public Fix64 AttackInterval
        {
            get
            {
                return m_AttackInterval;
            }
        }
    }
}

