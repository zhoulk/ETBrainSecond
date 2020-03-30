using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace ETBrain
{
    public class Weapon : Entity
    {
        private WeaponData m_WeaponData = null;

        private Fix64 m_NextAttackTime = Fix64.Zero;

        private const string AttachPoint = "Weapon Point";

        protected internal override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_WeaponData = userData as WeaponData;
            if (m_WeaponData == null)
            {
                Log.Error("Weapon data is invalid.");
                return;
            }

            GameEntry.Entity.AttachEntity(Entity, m_WeaponData.OwnerId, AttachPoint);
        }

        protected internal override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);

            Name = Utility.Text.Format("Weapon of {0}", parentEntity.Name);
            CachedTransform.localPosition = Vector3.zero;
        }

        public void TryAttack()
        {
            if (m_WeaponData.AttackInterval > m_NextAttackTime)
            {
                m_NextAttackTime += GameData.g_fixFrameLen;
                return;
            }

            GameEntry.Entity.ShowBullet(new BulletData(GameEntry.Entity.GenerateSerialId(), 10000, m_WeaponData.OwnerId)
            {
                //Position = CachedTransform.position,
                FPosition = new FixVector3(
                    Fix64.One*CachedTransform.position.x,
                    Fix64.One * CachedTransform.position.y,
                    Fix64.One * CachedTransform.position.z),
            });
            GameEntry.Sound.PlaySound("weapon_player");
        }
    }
}

