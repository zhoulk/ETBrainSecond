/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/08/09
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;
using UnityEngine.EventSystems;
using LT;
using LT.UI;

/// <summary>
/// UGUI 窗体基类。
/// <para> 1.统一可交互控制, </para>
/// <para> 2.统一UI开关过程， </para>
/// <para> 3.以及提供过程完成回调接口(在处理同一帧内关闭UI，同时恢复键盘事件响应逻辑的临界点问题，尤为好用)。</para>
/// </summary>
public abstract class UGuiForm : UIForm
{
    protected CanvasGroup m_CanvasGroup;

    /// <summary>
    /// 可交互的
    /// </summary>
    public override bool Interactable => m_CanvasGroup.interactable;

    /// <summary>
    /// 初始化界面。
    /// </summary>
    /// <param name="uiFormAssetName">界面资源名称。</param>
    /// <param name="uiGroup">界面所处的界面组。</param>
    /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
    /// <param name="isNewInstance">是否是新实例。</param>
    /// <param name="userData">用户自定义数据。</param>
    public override void OnInit(string uiFormAssetName, IUIGroup uiGroup, bool pauseCoveredUIForm, bool isNewInstance, object userData)
    {
        base.OnInit(uiFormAssetName, uiGroup, pauseCoveredUIForm, isNewInstance, userData);

        if (isNewInstance)
        {
            m_CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
            m_CanvasGroup.interactable = false;
            m_CanvasGroup.alpha = 0f;
        }
    }

    /// <summary>
    /// 界面打开。
    /// </summary>
    /// <param name="userData">用户自定义数据。</param>
    public override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        gameObject.SetActive(true);

        OpenUIFormAnimation();
    }

    /// <summary>
    /// 界面轮询 Interactable == false时,则不执行
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    /// <summary>
    /// 界面关闭。
    /// </summary>
    /// <param name="userData">用户自定义数据。</param>
    public override void OnClose(object userData)
    {
        base.OnClose(userData);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 界面被遮挡
    /// </summary>
    public override void OnCover()
    {
        base.OnCover();
        m_CanvasGroup.interactable = false;
    }

    /// <summary>
    /// 界面遮挡恢复
    /// </summary>
    public override void OnReveal()
    {
        base.OnReveal();
        m_CanvasGroup.interactable = true;
    }

    /// <summary>
    /// 界面暂停。
    /// </summary>
    public override void OnPause()
    {
        base.OnPause();
        m_CanvasGroup.interactable = false;
    }

    /// <summary>
    /// 界面暂停恢复。
    /// </summary>
    public override void OnResume()
    {
        base.OnResume();
        m_CanvasGroup.interactable = true;
    }

    /// <summary>
    /// 设置当前焦点
    /// </summary>
    /// <param name="target">焦点目标</param>
    public void Focus(GameObject target)
    {
        EventSystem.current.SetSelectedGameObject(target);
    }

    /// <summary>
    /// 获取当前焦点
    /// </summary>
    /// <returns></returns>
    public GameObject GetFocus()
    {
        return EventSystem.current.currentSelectedGameObject;
    }

    /// <summary>
    /// 关闭UI窗体
    /// </summary>
    /// <param name="uiForm">待关闭的ui窗体</param>
    /// <param name="isRelease">是否释放UI</param>
    public void CloseUIForm(IUIForm uiForm, bool isRelease = false)
    {
        CloseUIFormAnimation(uiForm, null, isRelease);
    }

    /// <summary>
    /// 关闭UI窗体
    /// </summary>
    /// <param name="uiForm">待关闭的ui窗体</param>
    /// <param name="userData">用户数据</param>
    /// <param name="isRelease">是否释放UI</param>
    public void CloseUIForm(IUIForm uiForm, object userData, bool isRelease = false)
    {
        CloseUIFormAnimation(uiForm, userData, isRelease);
    }

    /// <summary>
    /// 播放打开动画
    /// </summary>
    public virtual void OpenUIFormAnimation()
    {
        StopAllCoroutines();

        StartCoroutine(m_CanvasGroup.Fade2Alpha(1, 0.3f, () =>
        {
            m_CanvasGroup.interactable = true;
        }));
    }

    /// <summary>
    /// 播放关闭UI窗体动画
    /// </summary>
    /// <param name="uiForm">待关闭的ui窗体</param>
    /// <param name="isRelease">是否释放UI</param>
    public virtual void CloseUIFormAnimation(IUIForm uiForm, object userData, bool isRelease = false)
    {
        StopAllCoroutines();
        m_CanvasGroup.interactable = false;

        StartCoroutine(m_CanvasGroup.Fade2Alpha(0f, 0.3f, () =>
        {
            App.Make<IUIManager>().CloseUIForm(uiForm, userData, isRelease);
        }));
    }
}