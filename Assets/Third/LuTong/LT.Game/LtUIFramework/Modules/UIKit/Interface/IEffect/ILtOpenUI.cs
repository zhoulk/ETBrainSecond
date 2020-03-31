/*
 *    描述:
 *          1. UI打开回调接口
 *
 *    开发人: 邓平
 */
namespace LtFramework.UI
{
    interface ILtOpenUI: ILtDisplayStateUI
    {
        void OnOpenUIStart(float time, params object[] paramValues);

        void OnOpenUIUpdata(float remainTime, params object[] paramValues);

        void OnOpenUIEnd(params object[] paramValues);
    }
}
