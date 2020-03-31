/*
 *    描述:
 *          1. UI解冻回调接口
 *
 *    开发人: 邓平
 */
namespace LtFramework.UI
{
    interface ILtThawUI : ILtDisplayStateUI
    {
        void OnThawUIStart(float time, params object[] paramValues);

        void OnThawUIUpdata(float remainTime, params object[] paramValues);

        void OnThawUIEnd(params object[] paramValues);
    }
}
