/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/12/24
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using LT;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI导航扩展。
/// 如需支持LTInput的操控，则需在一个全局对象中挂载此脚本。
/// </summary>
public class LTNavigation : MonoBehaviour
{
    private void Update()
    {
        Auto();
    }

    public void Auto()
    {
        if (EventSystem.current == null || EventSystem.current.currentSelectedGameObject == null) return;

        // 禁用默认的事件发送器
        EventSystem.current.sendNavigationEvents = false;

        // 获取当前焦点
        var focus = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
        var handler = focus as ISubmitHandler;

        if (LTInput.GetKeyDown(KeyCode2.A) && handler != null)
        {
            handler.OnSubmit(new BaseEventData(EventSystem.current));
            return;
        }

        // 下一焦点
        Selectable nextFocus = null;
        if (LTInput.GetKeyDown(KeyCode2.Up)) nextFocus = focus.FindSelectableOnUp();
        else if (LTInput.GetKeyDown(KeyCode2.Down)) nextFocus = focus.FindSelectableOnDown();
        else if (LTInput.GetKeyDown(KeyCode2.Left)) nextFocus = focus.FindSelectableOnLeft();
        else if (LTInput.GetKeyDown(KeyCode2.Right)) nextFocus = focus.FindSelectableOnRight();

        if (nextFocus != null)
        {
            // 如果下一个方向对象不为空，则设为新焦点
            EventSystem.current.SetSelectedGameObject(nextFocus.gameObject);
        }
    }
}