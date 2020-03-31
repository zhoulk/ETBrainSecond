/*
*    描述:
*          1. UI类型定义
*
*    开发人: 邓平
*/
namespace LtFramework.UI
{
    #region 系统枚举类型

    /// <summary>
    /// UI窗体（位置）类型
    /// </summary>
    public enum UIFormType
    {
        //普通窗体
        Normal,

        //固定窗体                              
        Fixed,

        //弹出窗体
        PopUp,

        //提示窗体
        Prompt,
    }

    /// <summary>
    /// UI窗体的显示类型
    /// </summary>
    public enum UIFormShowMode
    {
        //普通
        Normal,

        //根节点
        Root,

        //生成新的实例
        NewInstance,
    }

    /// <summary>
    /// UI窗体透明度类型
    /// </summary>
    public enum UIFormLucenyType
    {
        //完全透明，不能穿透
        Lucency,

        //半透明，不能穿透
        Translucence,

        //低透明度，不能穿透
        ImPenetrable,

        //可以穿透
        Pentrate
    }

    //ui的状态
    public enum UIState
    {
        Null,
        Normal,      //可操作
        Display,     //显示
        ReDisplay,   //重新显示
        Hide,        //隐藏
        Freeze,      //冻结
        ReadyThaw,
        Thaw      //解冻
    }

    //显示状态
    public enum DisplayState
    {
        OpenUI,      //显示
        ShowUI,
        ThawUI,   //解冻  
        
        HideUI,        //隐藏          
        FreezeUI,      //冻结 
        CloseUI,    
    }

    public enum CursorType
    {
        Both,
        Single,
    }

    public enum TimeScaleType
    {
        DeltaTime,
        UnscaledDeltaTime,
    }

    #endregion

    /// <summary>
    /// UI类型
    /// </summary>
    public class UIType
    {
        //UI窗体（位置）类型，窗体实例化到哪个位置节点
        public UIFormType UIFormsType = UIFormType.Normal;

        //UI窗体显示类型,窗体以什么方式显示
        public UIFormShowMode UIFormsShowMode = UIFormShowMode.Normal;

        //UI窗体透明度类型,窗体的透明度
        public UIFormLucenyType UIFormLucencyType = UIFormLucenyType.Lucency;
    }
}