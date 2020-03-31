/*
 *    描述:
 *          1. UI依赖关系类
 *
 *    开发人: 邓平
 */
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LtFramework.UI
{
    public class UIFormNode
    {
        public int ID;
        public string UIName;
        public IBaseUIForm Node;

        public void Init(IBaseUIForm node)
        {
            if (node != null)
            {
                ID = node.ID;
                UIName = node.name;
                this.Node = node;
            }
            else
            {
                Reset();
            }
        }

        public void Reset()
        {
            ID = 0;
            UIName = string.Empty;
            Node = null;
        }
    }

    /// <summary>
    /// UI节点
    /// </summary>
    public class UIFormDepend
    {
        /// <summary>
        /// 父节点
        /// </summary>
        private UIFormNode parentNode = new UIFormNode();

        /// <summary>
        /// 子节点
        /// </summary>
        private Dictionary<int, UIFormNode> childrenNode;

        private List<int> childrenNodeID;

        public UIFormDepend()
        {
            parentNode.Reset();
            childrenNode = new Dictionary<int, UIFormNode>();
            childrenNodeID = new List<int>();
        }

        public void SetParent(IBaseUIForm parent)
        {
            parentNode.Init(parent);
        }

        public UIFormNode GetParent
        {
            get { return parentNode; }
        }

        public List<UIFormNode> GetClild
        {
            get { return childrenNode.Values.ToList(); }
        }

        public List<int> GetClildID
        {
            get { return childrenNode.Keys.ToList(); }
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param UIName="child"></param>
        internal void AddChildNode(IBaseUIForm child)
        {
            if (!childrenNodeID.Contains(child.ID))
            {
                //todo 对象池获取
                UIFormNode node = new UIFormNode();
                node.Init(child);
                childrenNode.Add(node.ID, node);
                childrenNodeID.Add(node.ID);
            }
            else
            {
                if (!childrenNode.ContainsKey(child.ID))
                {
                    UIFormNode node = new UIFormNode();
                    node.Init(child);
                    childrenNode.Add(node.ID, node);
                    Debug.LogError("添加子节点有误");
                }
                else
                {
                    childrenNode[child.ID].Init(child);
                }
            }
        }

        /// <summary>
        /// 移除子节点
        /// </summary>
        /// <param UIName="child"></param>
        internal void RemoveChildNode(IBaseUIForm child)
        {
            if (childrenNodeID.Contains(child.ID))
            {
                UIFormNode node = childrenNode[child.ID];
                node.Reset();
                //todo 回收node
                childrenNode.Remove(child.ID);
                childrenNodeID.Remove(child.ID);
            }
            else
            {
                if (childrenNode.ContainsKey(child.ID))
                {
                    UIFormNode node = childrenNode[child.ID];
                    node.Reset();
                    //todo 回收node
                    childrenNode.Remove(child.ID);
                    Debug.LogError("移除子节点有误");
                }
            }
        }

        /// <summary>
        /// 冻结所有子节点
        /// </summary>
        /// <param UIName="circle">是否递归冻结</param>
        internal void FreezeAllClildren(bool circle = true, float time = 0, params object[] paramValues)
        {
            if (circle)
            {
                for (int i = childrenNodeID.Count - 1; i >= 0; i--)
                {
                    int id = childrenNodeID[i];
                    IBaseUIForm uiForm = childrenNode[id].Node;
                    CircleFreeze(uiForm, time, paramValues);
                    uiForm.FreezeUI(time, paramValues);
                }
            }
            else
            {
                for (int i = childrenNodeID.Count - 1; i >= 0; i--)
                {
                    int id = childrenNodeID[i];
                    IBaseUIForm uiForm = childrenNode[id].Node;
                    uiForm.FreezeUI(time, paramValues);
                }
            }
        }

        /// <summary>
        /// 冻结所有子节点
        /// </summary>
        /// <param UIName="circle">是否递归冻结</param>
        internal void ThawAllClildren(bool circle = true, float time = 0, params object[] paramValues)
        {
            if (circle)
            {
                for (int i = childrenNodeID.Count - 1; i >= 0; i--)
                {
                    int id = childrenNodeID[i];
                    IBaseUIForm uiForm = childrenNode[id].Node;
                    CircleThaw(uiForm, time, paramValues);
                    uiForm.ThawUI(time, paramValues);
                }
            }
            else
            {
                for (int i = childrenNodeID.Count - 1; i >= 0; i--)
                {
                    int id = childrenNodeID[i];
                    IBaseUIForm uiForm = childrenNode[id].Node;
                    uiForm.ThawUI(time, paramValues);
                }
            }
        }

        internal void CloseAllClidren(bool circle = true, float time = 0, params object[] paramValues)
        {
            if (circle)
            {
                for (int i = childrenNodeID.Count - 1; i >= 0; i--)
                {
                    int id = childrenNodeID[i];
                    IBaseUIForm uiForm = childrenNode[id].Node;
                    CircleClose(uiForm, time, paramValues);
                    if (uiForm.UIStage != UIState.Hide)
                    {
                        uiForm.CloseUI(time, paramValues);
                    }
                }
            }
            else
            {
                for (int i = childrenNodeID.Count - 1; i >= 0; i--)
                {
                    int id = childrenNodeID[i];
                    IBaseUIForm uiForm = childrenNode[id].Node;
                    if (uiForm.UIStage != UIState.Hide)
                    {
                        uiForm.CloseUI(time, paramValues);
                    }
                }
            }
        }


        /// <summary>
        /// 递归冻结子节点
        /// </summary>
        /// <param UIName="uiForm"></param>
        private void CircleFreeze(IBaseUIForm uiForm, float time, params object[] paramValues)
        {
            List<int> clildrenID = uiForm.UIFormDepend.childrenNodeID;
            for (int i = clildrenID.Count - 1; i >= 0; i--)
            {
                IBaseUIForm ui = uiForm.UIFormDepend.childrenNode[childrenNodeID[i]].Node;

                CircleFreeze(ui, time, paramValues);
                ui.FreezeUI(time, paramValues);
            }
        }

        /// <summary>
        /// 递归解冻子节点
        /// </summary>
        /// <param UIName="uiForm"></param>
        private void CircleThaw(IBaseUIForm uiForm, float time, params object[] paramValues)
        {
            List<int> clildrenID = uiForm.UIFormDepend.childrenNodeID;
            for (int i = clildrenID.Count - 1; i >= 0; i--)
            {
                IBaseUIForm ui = uiForm.UIFormDepend.childrenNode[childrenNodeID[i]].Node;
                CircleThaw(ui, time, paramValues);
                ui.ThawUI(time, paramValues);
            }
        }

        /// <summary>
        /// 递归关闭子节点
        /// </summary>
        /// <param UIName="uiForm"></param>
        private void CircleClose(IBaseUIForm uiForm, float time, params object[] paramValues)
        {
            List<int> clildrenID = uiForm.UIFormDepend.childrenNodeID;
            for (int i = clildrenID.Count - 1; i >= 0; i--)
            {
                IBaseUIForm ui = uiForm.UIFormDepend.childrenNode[childrenNodeID[i]].Node;
                CircleClose(ui, time, paramValues);
                if (ui.UIStage != UIState.Hide)
                {
                    ui.CloseUI(time, paramValues);
                }
            }
        }
    }
}

