/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/08/01
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using LT.ObjectPool;

namespace LT.UI
{
    internal sealed partial class UIManager : IUIManager
    {
        /// <summary>
        /// 界面实例对象。
        /// </summary>
        private sealed class UIFormInstanceObject : ObjectBase
        {
            private readonly IUIFormHelper uiFormHelper;

            public UIFormInstanceObject(string name, object uiFormInstance, IUIFormHelper uiFormHelper)
                : base(name, uiFormInstance)
            {
                this.uiFormHelper = uiFormHelper;
            }

            protected internal override void OnRelease(bool isShutdown)
            {
                uiFormHelper.ReleaseUIForm(Target);
            }
        }
    }
}