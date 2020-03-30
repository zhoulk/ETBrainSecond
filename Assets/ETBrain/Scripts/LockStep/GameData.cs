
using System.Collections.Generic;

namespace ETBrain
{
    public class GameData
    {
        //所有士兵的队列
        public static List<Aircraft> g_listAircreaft = new List<Aircraft>();

        public static List<Asteroid> g_listAsteroid = new List<Asteroid>();

        public static List<Bullet> g_listBullet = new List<Bullet>();

        //预定的每帧的时间长度
        public static Fix64 g_fixFrameLen = Fix64.FromRaw(273);

        //游戏的逻辑帧
        public static int g_uGameLogicFrame = 0;

        //随机数对象
        public static SRandom g_srand = new SRandom(1000);
    }
}

