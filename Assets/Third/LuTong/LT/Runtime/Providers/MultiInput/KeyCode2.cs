/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：自定义多人模式键值,以取代UnityEngine.KeyCode
 * 
 * ------------------------------------------------------------------------------*/

namespace LT
{
    /// <summary>
    /// 自定义多人游戏键值
    /// </summary>
    public enum KeyCode2 : int
    {
        Up = 19,
        Down = 20,
        Left = 21,
        Right = 22,
        A = 23,
        B = 97,
        X = 99,
        Y = 100,
        L = 102,
        R = 103,
    }

    /// <summary>
    /// 多套键值扩展,如2p,3p....
    /// </summary>
    public static class KeyCode2Extend
    {
        public const int Up2 = 51;
        public const int Down2 = 47;
        public const int Left2 = 29;
        public const int Right2 = 32;
        public const int A2 = 40;
        public const int B2 = 39;
        public const int X2 = 38;
        public const int Y2 = 37;
        public const int L2 = 49;
        public const int R2 = 43;
    }
}