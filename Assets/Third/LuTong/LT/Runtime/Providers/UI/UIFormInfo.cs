/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2020/02/26
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

namespace LT.UI
{
    /// <summary>
    /// UI窗体的信息
    /// </summary>
    public struct UIFormInfo
    {
        /// <summary>
        /// UIForm资源名称
        /// </summary>
        public string AssetName;

        /// <summary>
        /// UIGroup名称
        /// </summary>
        public string UIGroupName;

        /// <summary>
        /// UIGroup深度
        /// </summary>
        public int UIGroupDepth;

        /// <summary>
        /// 是否暂停被覆盖的界面
        /// </summary>
        public bool PauseCoveredUIForm;
    }
}