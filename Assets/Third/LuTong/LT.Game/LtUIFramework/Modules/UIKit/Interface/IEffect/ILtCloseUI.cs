/*
 *    描述:
 *          1. UI关闭回调接口
 *
 *    开发人: 邓平
 */
namespace LtFramework.UI
{
    interface ILtCloseUI : ILtDisplayStateUI
    {
        void OnCloseUIStart(float time,params object[] paramValues);

        void OnCloseUIUpdata(float remainTime, params object[] paramValues);

        void OnCloseUIEnd(params object[] paramValues);
    }
}
