
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ETBrain
{
    [Serializable]
    public class AircraftData : TargetableObjectData
    {
        [SerializeField]
        private int m_MaxHP = 0;

        [SerializeField]
        private int m_Defense = 0;

        [SerializeField]
        private int m_DeadEffectId = 0;

        [SerializeField]
        private int m_DeadSoundId = 0;

        private List<WeaponData> m_WeaponDatas = new List<WeaponData>();

        public AircraftData(int entityId, int typeId)
            : base(entityId, typeId)
        {
            m_MaxHP = 100;
            HP = 10000;
            Attack = 10;

            AttachWeaponData(new WeaponData(GameEntry.Entity.GenerateSerialId(), 10000, Id));
        }

        /// <summary>
        /// 最大生命。
        /// </summary>
        public int MaxHP
        {
            get
            {
                return m_MaxHP;
            }
        }

        /// <summary>
        /// 防御。
        /// </summary>
        public int Defense
        {
            get
            {
                return m_Defense;
            }
        }

        public int DeadEffectId
        {
            get
            {
                return m_DeadEffectId;
            }
        }

        public int DeadSoundId
        {
            get
            {
                return m_DeadSoundId;
            }
        }

        public List<WeaponData> GetAllWeaponDatas()
        {
            return m_WeaponDatas;
        }

        public void AttachWeaponData(WeaponData weaponData)
        {
            if (weaponData == null)
            {
                return;
            }

            if (m_WeaponDatas.Contains(weaponData))
            {
                return;
            }

            m_WeaponDatas.Add(weaponData);
        }
    }
}
