
using UnityEngine;
using UnityGameFramework.Runtime;

namespace ETBrain
{
    public class TargetableObject : Entity
    {
        private TargetableObjectData m_TargetableObjectData = null;

        //最后的位置
        public FixVector3 m_fixv3LastPosition = new FixVector3(Fix64.Zero, Fix64.Zero, Fix64.Zero);

        //逻辑位置
        public FixVector3 m_fixv3LogicPosition = new FixVector3(Fix64.Zero, Fix64.Zero, Fix64.Zero);

        //显示位置
        public FixVector3 m_fixv3RenderPosition = new FixVector3(Fix64.Zero, Fix64.Zero, Fix64.Zero);

        //旋转值
        public FixVector3 m_fixv3LogicRotation;

        //缩放值
        public FixVector3 m_fixv3LogicScale;

        public TargetableObjectData TargetableObjectData
        {
            get
            {
                return m_TargetableObjectData;
            }
        }

        public void ApplyDamage(Entity attacker, int damageHP)
        {
            //float fromHPRatio = m_TargetableObjectData.HPRatio;
            m_TargetableObjectData.HP -= damageHP;
            //float toHPRatio = m_TargetableObjectData.HPRatio;
            //if (fromHPRatio > toHPRatio)
            //{
            //    GameEntry.HPBar.ShowHPBar(this, fromHPRatio, toHPRatio);
            //}

            if (m_TargetableObjectData.HP <= 0)
            {
                OnDead(attacker);
            }
        }

        protected internal override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_TargetableObjectData = userData as TargetableObjectData;
            if (m_TargetableObjectData == null)
            {
                Log.Error("Targetable object data is invalid.");
                return;
            }
        }

        protected virtual void OnDead(Entity attacker)
        {
            GameEntry.Entity.HideEntity(this);
        }

        private void OnTriggerEnter(UnityEngine.Collider other)
        {
            Entity entity = other.gameObject.GetComponent<Entity>();
            if (entity == null)
            {
                return;
            }

            if (entity is TargetableObject && entity.Id >= Id)
            {
                // 碰撞事件由 Id 小的一方处理，避免重复处理
                return;
            }

            AIUtility.PerformCollision(this, entity);
        }

        public virtual void UpdateLogic()
        {

        }

        public virtual void UpdateRenderPosition(float interpolation)
        {

        }

        //- 记录最后的位置
        // 
        // @return none.
        public virtual void RecordLastPos()
        {
            //m_fixv3LastPosition = m_fixv3LogicPosition;
            m_fixv3LastPosition = m_fixv3RenderPosition;
        }
    }
}

