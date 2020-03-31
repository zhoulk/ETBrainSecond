/*
 *    描述:
 *          1. UI框架常量
 *
 *    开发人: 邓平
 */
namespace LtFramework.UI
{
    public class UIConfig
    {
        /* 路径常量 */
        public const string SysUIPrefabsPath = "Prefabs/UIWindows/";


        public const string SysPathCanvas = "Prefabs/LtUICanvas";
        public const string SysPahtUIFormsConfigInfo = "Json/UIFrame/UIFormsConfigInfo";
        public const string SysPahtConfigInfo = "Json/UIFrame/SysConfigInfo";

        /* 标签常量 */
        public const string SysTagCanvas = "_TagCanvas";

        /* 节点常量 */
        public const string SysNormalNode = "Normal";
        public const string SysFixedNode = "Fixed";
        public const string SysPopUpNode = "PopUp";
        public const string SysPromptNode = "Prompt";
        public const string SysScriptManagerNode = "_ScriptMgr";
        public const string SysUICameraNode = "UICamera";
        public const string SysTouchMask = "TouchMask";
        public const string SysUIPrefabs = "UIPrefabs";
        public const string SysEventSystemNode = "EventSystem";

        /* 遮罩管理器中，透明度常量 */
        public const float SysUIMaskLucencyColorRGB = 255 / 255F;
        public const float SysUIMaskLucencyColorRGBA = 0F / 255F;

        public const float SysUIMaskTransLucencyColorRGB = 0 / 255F;
        public const float SysUIMaskTransLucencyColorRGBA = 0 / 255F;

        public const float SysUIMaskImpenetrableColorRGB = 0 / 255F;
        public const float SysUIMaskImpenetrableColorRGBA = 0 / 255F;

    }
}