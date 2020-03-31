/*
*    描述:
*          1. 数学函数工具
*
*    开发人: 邓平
*/
namespace LtFramework.Util.Math
{
    public partial class MathUtil
    {

        /// <summary>
        /// 百分百概率
        /// </summary>
        /// <param UIName="percent"> 0-99 </param>
        /// <returns></returns>
        public static bool Percent(int percent)
        {
            return UnityEngine.Random.Range(0, 100) < percent;
        }

        /// <summary>
        /// 随机获取数组中的一个元素
        /// </summary>
        /// <typeparam UIName="T">类型</typeparam>
        /// <param UIName="values">数组</param>
        /// <returns>类型</returns>
        public static T GetRandomValueFrom<T>(params T[] values)
        {
            return values[UnityEngine.Random.Range(0, values.Length)];
        }

        public static bool FloatEqual(float value1, float value2)
        {
            if (value1 - value2 < SysConst.FloatPrecision)
            {
                return true;
            }


            return false;
        }
    }
}
