/*
 *    描述:
 *          1. UI 框架Base类
 *                     虚拟手柄对应键盘   上   下    左    右      A       B       X        Y        L          R
 *             1P 操作 虚拟手柄对应键盘   Up   Down  Left  Right   Num1    Num2    Num4     Num5     Num7       Num8
 *             2P 操作 虚拟手柄对应键盘   W    S     A     D       J       K       U        I        Alpha7     Alpha8
 *
 *    开发人: 邓平
 */

namespace LtFramework.UI
{
    public abstract class BaseUIForm : IBaseUIForm
    {
        /*****************UI窗体公共方法********************/

        public override LtButton OnLeftSelected(LtButton currentButton, LtButton autoSelecteButton, CtrlHandler ctrl)
        {
            AudioMgr.Instance.PlayEffect(SoundType.UI.UI_move);

            return base.OnLeftSelected(currentButton, autoSelecteButton, ctrl);
        }

        public override LtButton OnRightSelected(LtButton currentButton, LtButton autoSelecteButton, CtrlHandler ctrl)
        {
            AudioMgr.Instance.PlayEffect(SoundType.UI.UI_move);

            return base.OnRightSelected(currentButton, autoSelecteButton, ctrl);
        }

        public override LtButton OnUpSelected(LtButton currentButton, LtButton autoSelecteButton, CtrlHandler ctrl)
        {
            AudioMgr.Instance.PlayEffect(SoundType.UI.UI_move);

            return base.OnUpSelected(currentButton, autoSelecteButton, ctrl);
        }

        public override LtButton OnDownSelected(LtButton currentButton, LtButton autoSelecteButton, CtrlHandler ctrl)
        {
            AudioMgr.Instance.PlayEffect(SoundType.UI.UI_move);

            return base.OnDownSelected(currentButton, autoSelecteButton, ctrl);
        }
    }
}
