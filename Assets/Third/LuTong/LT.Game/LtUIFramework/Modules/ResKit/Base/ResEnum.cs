/*
 *    描述:
 *          1.
 *
 *    开发人: 邓平
 */
namespace LtFramework.ResKit
{

    /// <summary>
    /// 资源加载等级
    /// </summary>
    public enum LoadResPriority
    {
        Res_Hight = 0, //最高优先级
        Res_Middle, //一般优先级
        Res_Slow, //低优先级
        Res_Num, //优先级数量
    }

    /// <summary>
    /// 资源加载模式
    /// </summary>
    public enum LoadResMode
    {
        Resource,
        AssetBundle,
    }
}
