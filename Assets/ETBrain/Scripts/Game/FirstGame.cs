
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace ETBrain
{
    public class FirstGame : GameBase
    {
        private float m_ElapseSeconds = 0f;

        private Fix64 f_ElapseSeconds = Fix64.Zero;

        private FixVector3 f_EnemySpawnBoundaryPos = new FixVector3(Fix64.FromRaw(-26624), Fix64.Zero, Fix64.One * 17);
        private FixVector3 f_EnemySpawnBoundarySize = new FixVector3(Fix64.One*13, Fix64.Zero, Fix64.One * 2);

        public override int Level
        {
            get
            {
                return 1;
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            GameEntry.Entity.ShowAircraft(new AircraftData(GameEntry.Entity.GenerateSerialId(), 10000));
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);

            //m_ElapseSeconds += elapseSeconds;
            //if (m_ElapseSeconds >= 1f)
            //{
            //    m_ElapseSeconds = 0f;
            //    //IDataTable<DRAsteroid> dtAsteroid = GameEntry.DataTable.GetDataTable<DRAsteroid>();
            //    float randomPositionX = SceneBackground.EnemySpawnBoundary.bounds.min.x + SceneBackground.EnemySpawnBoundary.bounds.size.x * (float)Utility.Random.GetRandomDouble();
            //    float randomPositionZ = SceneBackground.EnemySpawnBoundary.bounds.min.z + SceneBackground.EnemySpawnBoundary.bounds.size.z * (float)Utility.Random.GetRandomDouble();
            //    GameEntry.Entity.ShowAsteroid(new AsteroidData(GameEntry.Entity.GenerateSerialId(), 60000 + Utility.Random.GetRandom(20))
            //    {
            //        Position = new Vector3(randomPositionX, 0f, randomPositionZ),
            //    });
            //}
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            if (SceneBackground == null)
            {
                return;
            }

            f_ElapseSeconds += GameData.g_fixFrameLen;
            if (f_ElapseSeconds >= Fix64.One)
            {
                f_ElapseSeconds = Fix64.Zero;
                //IDataTable<DRAsteroid> dtAsteroid = GameEntry.DataTable.GetDataTable<DRAsteroid>();
                Fix64 randomPositionX = f_EnemySpawnBoundaryPos.x + GameData.g_srand.Range(0, (uint)f_EnemySpawnBoundarySize.x);
                Fix64 randomPositionZ = f_EnemySpawnBoundaryPos.z + GameData.g_srand.Range(0, (uint)f_EnemySpawnBoundarySize.z);
                GameEntry.Entity.ShowAsteroid(new AsteroidData(GameEntry.Entity.GenerateSerialId(), 60000 + Utility.Random.GetRandom(20))
                {
                    //Position = new Vector3(randomPositionX, 0f, randomPositionZ),
                    FPosition = new FixVector3(randomPositionX, Fix64.Zero, randomPositionZ),
                });
            }
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }

        protected override void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            base.OnShowEntityFailure(sender, e);
        }
    }
}

