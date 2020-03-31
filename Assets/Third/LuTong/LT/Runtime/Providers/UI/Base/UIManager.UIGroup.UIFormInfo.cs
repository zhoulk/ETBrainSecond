/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/08/01
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;

namespace LT.UI
{
    internal sealed partial class UIManager
    {
        private sealed partial class UIGroup
        {
            /// <summary>
            /// 界面组界面信息。
            /// </summary>
            private sealed class UIFormInfo
            {
                private readonly IUIForm uiForm;
                private bool paused;
                private bool covered;

                /// <summary>
                /// 初始化界面组界面信息的新实例。
                /// </summary>
                /// <param name="uiForm">界面。</param>
                public UIFormInfo(IUIForm uiForm)
                {
                    Guard.Verify<ArgumentException>(uiForm == null, "UI form is invalid.");

                    this.uiForm = uiForm;
                    this.paused = true;
                    this.covered = true;
                }

                /// <summary>
                /// 获取界面。
                /// </summary>
                public IUIForm UIForm
                {
                    get
                    {
                        return uiForm;
                    }
                }

                /// <summary>
                /// 获取或设置界面是否暂停。
                /// </summary>
                public bool Paused
                {
                    get
                    {
                        return paused;
                    }
                    set
                    {
                        paused = value;
                    }
                }

                /// <summary>
                /// 获取或设置界面是否遮挡。
                /// </summary>
                public bool Covered
                {
                    get
                    {
                        return covered;
                    }
                    set
                    {
                        covered = value;
                    }
                }
            }
        }
    }
}