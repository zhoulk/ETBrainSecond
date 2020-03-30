
using UnityEngine;
using UnityGameFramework.Runtime;

namespace ETBrain
{
    public class Asteroid : TargetableObject
    {
        private AsteroidData m_AsteroidData = null;

        private Vector3 m_RotateSphere = Vector3.zero;

        protected internal override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_AsteroidData = userData as AsteroidData;
            if (m_AsteroidData == null)
            {
                Log.Error("Asteroid data is invalid.");
                return;
            }

            m_fixv3RenderPosition = m_AsteroidData.FPosition;
            RecordLastPos();

            Log.Info("{0} {1}", m_AsteroidData.Id, m_AsteroidData.FPosition);

            GameData.g_listAsteroid.Add(this);

            m_RotateSphere = Random.insideUnitSphere;
        }

        protected internal override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            GameData.g_listAsteroid.Remove(this);
        }

        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            //CachedTransform.Translate(Vector3.back * m_AsteroidData.Speed * elapseSeconds, Space.World);
            CachedTransform.Rotate(m_RotateSphere * m_AsteroidData.AngularSpeed * elapseSeconds, Space.Self);
        }

        protected override void OnDead(Entity attacker)
        {
            base.OnDead(attacker);

            GameData.g_listAsteroid.Remove(this);

            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 1)
            {
                Position = CachedTransform.localPosition,
            });
            GameEntry.Sound.PlaySound("explosion_asteroid");

            VarUInt v_score = GameEntry.DataNode.GetData<VarUInt>(Constant.DataNode.UserScore);
            v_score++;
            GameEntry.DataNode.SetData<VarUInt>(Constant.DataNode.UserScore, v_score);
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            m_fixv3RenderPosition += new FixVector3(Fix64.Zero, Fix64.Zero, -1 * Fix64.One * GameData.g_fixFrameLen * 5);
        }

        public override void UpdateRenderPosition(float interpolation)
        {
            base.UpdateRenderPosition(interpolation);

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
    }
}

