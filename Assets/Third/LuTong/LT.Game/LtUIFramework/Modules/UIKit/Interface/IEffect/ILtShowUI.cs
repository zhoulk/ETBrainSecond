/*
 *    描述:
 *          1. UI ShowUI回调接口
 *
 *    开发人: 邓平
 */
namespace LtFramework.UI
{
    interface ILtShowUI : ILtDisplayStateUI
    {
        void OnShowUIStart(float time, params object[] paramValues);

        void OnShowUIUpdata(float remainTime, params object[] paramValues);

        void OnShowUIEnd(params object[] paramValues);
    }
}
