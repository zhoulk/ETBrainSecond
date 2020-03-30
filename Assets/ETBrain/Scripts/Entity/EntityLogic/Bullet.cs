
using UnityEngine;

namespace ETBrain
{
    public class Bullet : Entity
    {
        private BulletData m_BulletData;

        //显示位置
        public FixVector3 m_fixv3RenderPosition = new FixVector3(Fix64.Zero, Fix64.Zero, Fix64.Zero);

        //最后的位置
        public FixVector3 m_fixv3LastPosition = new FixVector3(Fix64.Zero, Fix64.Zero, Fix64.Zero);

        protected internal override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_BulletData = (BulletData)userData;

            m_fixv3RenderPosition = m_BulletData.FPosition;
            RecordLastPos();

            GameData.g_listBullet.Add(this);
        }

        protected internal override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            GameData.g_listBullet.Remove(this);
        }

        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            //CachedTransform.Translate(Vector3.forward * m_BulletData.Speed * elapseSeconds, Space.World);
        }

        public void UpdateLogic()
        {
            m_fixv3RenderPosition += new FixVector3(Fix64.Zero, Fix64.Zero, Fix64.One * GameData.g_fixFrameLen * 5);
        }

        public void UpdateRenderPosition(float interpolation)
        {
            //Log.Info("Asteroid  UpdateRenderPosition {0} {1} {2}", interpolation, m_fixv3LastPosition, m_fixv3RenderPosition);

            //只有会移动的对象才需要采用插值算法补间动画,不会移动的对象直接设置位置即可
            if (interpolation != 0)
            {
                CachedTransform.position = Vector3.Lerp(m_fixv3LastPosition.ToVector3(), m_fixv3RenderPosition.ToVector3(), interpolation);
            }
            else
            {
                CachedTransform.position = m_fixv3RenderPosition.ToVector3();
            }
        }

        public virtual void RecordLastPos()
        {
            //m_fixv3LastPosition = m_fixv3LogicPosition;
            m_fixv3LastPosition = m_fixv3RenderPosition;
        }

        public BulletData BulletData
        {
            get
            {
                return m_BulletData;
            }
        }
    }
}

