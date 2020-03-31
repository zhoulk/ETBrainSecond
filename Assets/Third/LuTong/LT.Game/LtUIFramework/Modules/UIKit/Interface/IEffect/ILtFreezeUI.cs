/*
 *    描述:
 *          1. UI冻结回调接口
 *
 *    开发人: 邓平
 */
namespace LtFramework.UI
{
    interface ILtFreezeUI : ILtDisplayStateUI
    {
        void OnFreezeUIStart(float time, params object[] paramValues);

        void OnFreezeUIUpdata(float remainTime, params object[] paramValues);

        void OnFreezeUIEnd(params object[] paramValues);
    }
}
