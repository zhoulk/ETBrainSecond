
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace ETBrain
{
    public class Aircraft : TargetableObject
    {
        private AircraftData m_AircraftData = null;

        private Rect m_PlayerMoveBoundary = new Rect(-6, -4, 12, 12);
        private Vector3 m_TargetPosition = Vector3.zero;

        protected List<Weapon> m_Weapons = new List<Weapon>();

        public bool isFire = false;
        public bool r_isFire = false;

        protected internal override void OnShow(object userData)
        {
            base.OnShow(userData);

            GameData.g_listAircreaft.Add(this);

            m_AircraftData = userData as AircraftData;
            if (m_AircraftData == null)
            {
                Log.Error("Aircraft data is invalid.");
                return;
            }

            List<WeaponData> weaponDatas = m_AircraftData.GetAllWeaponDatas();
            for (int i = 0; i < weaponDatas.Count; i++)
            {
                GameEntry.Entity.ShowWeapon(weaponDatas[i]);
            }
        }

        protected override void OnDead(Entity attacker)
        {
            base.OnDead(attacker);

            GameData.g_listAircreaft.Remove(this);
        }

        protected internal override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            GameData.g_listAircreaft.Remove(this);
        }

        protected internal override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);

            if (childEntity is Weapon)
            {
                m_Weapons.Add((Weapon)childEntity);
                return;
            }
        }

        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            isFire = false;
            if (Input.GetMouseButton(0))
            {
                isFire = true;

                Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                m_TargetPosition = new Vector3(point.x, 0f, point.z);

                //for (int i = 0; i < m_Weapons.Count; i++)
                //{
                //    m_Weapons[i].TryAttack();
                //}
            }

            Vector3 direction = m_TargetPosition - CachedTransform.localPosition;
            if (direction.sqrMagnitude <= Vector3.kEpsilon)
            {
                return;
            }

            Vector3 speed = Vector3.ClampMagnitude(direction.normalized * 20 * elapseSeconds, direction.magnitude);
            Vector3 pos = new Vector3
            (
                Mathf.Clamp(CachedTransform.localPosition.x + speed.x, m_PlayerMoveBoundary.xMin, m_PlayerMoveBoundary.xMax),
                0f,
                Mathf.Clamp(CachedTransform.localPosition.z + speed.z, m_PlayerMoveBoundary.yMin, m_PlayerMoveBoundary.yMax)
            );
            pos = m_TargetPosition;
            //CachedTransform.localPosition = new Vector3
            //(
            //    Mathf.Clamp(CachedTransform.localPosition.x + speed.x, m_PlayerMoveBoundary.xMin, m_PlayerMoveBoundary.xMax),
            //    0f,
            //    Mathf.Clamp(CachedTransform.localPosition.z + speed.z, m_PlayerMoveBoundary.yMin, m_PlayerMoveBoundary.yMax)
            //);
            //CachedTransform.localPosition = pos;
            m_fixv3LogicPosition = new FixVector3((Fix64)pos.x, (Fix64)pos.y, (Fix64)pos.z);
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
            //Log.Info("arcraft logic");

            if (r_isFire)
            {
                //Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //m_TargetPosition = new Vector3(point.x, 0f, point.z);

                for (int i = 0; i < m_Weapons.Count; i++)
                {
                    m_Weapons[i].TryAttack();
                }
            }
        }

        public override void UpdateRenderPosition(float interpolation)
        {
            base.UpdateRenderPosition(interpolation);

            //只有会移动的对象才需要采用插值算法补间动画,不会移动的对象直接设置位置即可
            if (interpolation != 0)
            {
                CachedTransform.localPosition = Vector3.Lerp(m_fixv3LastPosition.ToVector3(), m_fixv3RenderPosition.ToVector3(), interpolation);
            }
            else
            {
                CachedTransform.localPosition = m_fixv3RenderPosition.ToVector3();
            }
        }
    }
}

