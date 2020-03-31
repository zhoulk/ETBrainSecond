/*
*    描述:
*          1. 随机数工具
*
*    开发人: 邓平
*/
using System.Collections.Generic;
using UnityEngine;

namespace LtFramework.Util.Math
{
    public class RandomUtil
    {
        #region  Static

        private static readonly System.Random Ran = new System.Random();

        /// <summary>
        /// 随机整型  [min,max)
        /// </summary>
        /// <param UIName="min">最小值 包括</param>
        /// <param UIName="max">最大值 不包括</param>
        /// <returns></returns>
        public static int GetRange(int min, int max)
        {
            return Ran.Next(min, max);
        }

        /// <summary>
        /// 随机浮点型  [min,max)
        /// </summary>
        /// <param UIName="min">最小值 包括</param>
        /// <param UIName="max">最大值 不包括</param>
        /// <returns></returns>
        public static float GetRange(float min, float max)
        {
            var r = Ran.NextDouble();
            return (float) (r * (max - min) + min);
        }


        /// <summary>
        /// 获取不同个数随机值
        /// </summary>
        /// <param UIName="number">随机值个数</param>
        /// <param UIName="min">最下范围 包含</param>
        /// <param UIName="max">最大范围 包含</param>
        /// <returns> 取得的随机值 </returns>
        public static List<int> GetDifferentRandom(int number, int min, int max)
        {
            List<int> origin = new List<int>();
            List<int> tmp = new List<int>();

            #region 错误检查

#if UNITY_EDITOR
            if (max < min)
            {
                Debug.LogError("最大值比最下值小 max :" + max + " min :" + min);
                return null;
            }

            if (number <= 0)
            {
                Debug.LogError("取值个数小于等于0 " + number);
                return null;
            }

            if (number - 1 > max - min)
            {
                Debug.LogError("取值个数大于取值范围 number " + number + " max - min  " + (max - min));
                return null;
            }
#endif

            #endregion

            for (int i = min; i <= max; i++)
            {
                origin.Add(i);
            }

            for (int i = 0; i < number; i++)
            {
                int index = UnityEngine.Random.Range(0, origin.Count);
                tmp.Add(origin[index]);
                origin.RemoveAt(index);
            }

            return tmp;
        }

        #endregion

        private readonly System.Random _Random;

        public RandomUtil()
        {
            _Random = new System.Random();
        }

        /// <summary>
        /// 创建 随机值生成器 
        /// </summary>
        /// <param UIName="seed">随机种子</param>
        public RandomUtil(int seed)
        {
            _Random = new System.Random(seed);
        }

        /// <summary>
        /// 随机整型  [min,max)
        /// </summary>
        /// <param UIName="min">最小值 包括</param>
        /// <param UIName="max">最大值 不包括</param>
        /// <returns></returns>
        public int Range(int min, int max)
        {
            return _Random.Next(min, max);
        }

        /// <summary>
        /// 随机浮点数 [min,max)
        /// </summary>
        /// <param UIName="min">最小值 包括</param>
        /// <param UIName="max">最大值 不包括</param>
        /// <returns></returns>
        public float Range(float min, float max)
        {
            var r = _Random.NextDouble();
            return (float) (r * (max - min) + min);
        }

        /// <summary>
        /// 类似正太分布
        /// </summary>
        /// <param UIName="min">最大值 包含</param>
        /// <param UIName="max">最小值 包含</param>
        /// <returns></returns>
        public int NormalDistribution(int min, int max)
        {
            int x = Range(min, max + 1);
            int y = Range(min, max + 1);
            int ret = (x + y) / 2;
            return ret;
        }

        /// <summary>
        /// 类似正太分布
        /// </summary>
        /// <param UIName="min">最大值</param>
        /// <param UIName="max">最小值</param>
        /// <returns></returns>
        public float NormalDistribution(float min, float max)
        {
            float x = Range(min, max);
            float y = Range(min, max);
            float ret = (x + y) / 2;
            return ret;
        }

    }
}
