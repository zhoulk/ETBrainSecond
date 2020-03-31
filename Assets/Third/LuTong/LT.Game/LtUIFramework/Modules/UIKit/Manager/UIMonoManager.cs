/*
*    描述:
*          1. UI框架管理类，
*
*    开发人: 邓平
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LtFramework.ResKit;
using LtFramework.Util;
using LtFramework.Util.Pools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;

namespace LtFramework.UI
{
    public class UIMonoManager : MonoBehaviour,ISingleton
    {

        #region 字段
        private static UIMonoManager _Instance;
        public EventSystem EventSystem;
        private CommandInvoker _Invoker;
        private InputEventSystem _InputEventSystem;

        //UI根节点
        private Transform _TraCanvasTransfrom = null;

        //全屏幕显示的节点
        private Transform _TraNormal = null;

        //固定显示的节点
        private Transform _TraFixed = null;

        //弹出节点
        private Transform _TraPopUp = null;

        //提示节点
        private Transform _TarPrompt = null;

        //UI管理脚本的节点
        private Transform _TraUIScripts = null;

        //UI相机
        private Transform _UICamera = null;

        private GameObject _TouchMask = null;

        #endregion


        #region API

        /// <summary>
        /// 获得UI
        /// </summary>
        /// <param UIName="uiFormName"></param>
        /// <returns></returns>
        public static IBaseUIForm GetUI(string uiFormName)
        {
            IBaseUIForm baseUiForms = null; //UI窗体基类
            if (string.IsNullOrEmpty(uiFormName)) return null;
            //根据UI窗体的名称，加载到“所有UI窗体”缓存集合中
            baseUiForms = Instance.LoadFormsToAllUIFormsCatch(uiFormName);
            return baseUiForms;
        }

        /// <summary>
        /// 获得UI
        /// </summary>
        /// <typeparam UIName="TBaseUI"></typeparam>
        /// <returns></returns>
        public static TBaseUI GetUI<TBaseUI>() where TBaseUI : IBaseUIForm
        {
            string name = typeof(TBaseUI).Name;
            return GetUI(name) as TBaseUI;

        }

        /// <summary>
        /// 判断当前窗口是否打开
        /// </summary>
        /// <param name="name">窗口名字</param>
        /// <returns></returns>
        public static bool IsOpenUI(string name)
        {
            return Instance._CurrentOpenNormalUIFormList.Contains(name);
        }

        /// <summary>
        /// 得到当前打开的NormalUI
        /// </summary>
        /// <param name="name">UI名字</param>
        /// <returns></returns>
        public static IBaseUIForm GetCurrentOpenUI(string name)
        {
            if (IsOpenUI(name))
            {
                return Instance._CurrentOpenNormalUIFormDic[name];
            }

            return null;
        }

        /// <summary>
        /// 获取当前显示的窗体
        /// </summary>
        /// <returns></returns>
        public static IBaseUIForm[] GetAllCurrentOpenUI
        {
            get { return Instance._CurrentOpenNormalUIFormDic.Values.ToArray(); }
        }

        /// <summary>
        /// 获取当前没有被冻结或关闭的UI
        /// </summary>
        public static IBaseUIForm[] GetEnableCtrlUI
        {
            get
            {
                List<IBaseUIForm> temp = new List<IBaseUIForm>();
                foreach (IBaseUIForm uiForm in Instance._CurrentOpenNormalUIFormDic.Values)
                {
                    if (uiForm.DisableSelectCount == 0)
                    {
                        temp.Add(uiForm);
                    }
                }

                return temp.ToArray();
            }
        }

        /// <summary>
        /// 是否存在 NewInstanceUI
        /// </summary>
        /// <param name="name">UI名字</param>
        /// <returns></returns>
        public static bool ExistNewInstanceUI(string name)
        {
            return Instance._AllNewInstanceCacheUIFormDic.ContainsKey(name);
        }

        /// <summary>
        /// 获取所有 NewInstanceUI
        /// </summary>
        /// <returns></returns>
        public static IBaseUIForm[] GetAllNewInstanceOpenUI()
        {

            List<IBaseUIForm> temp = new List<IBaseUIForm>();
            foreach (var uiFormDic in Instance._AllNewInstanceCacheUIFormDic.Values)
            {
                foreach (IBaseUIForm uiForm in uiFormDic.Values)
                {
                    if (uiForm.UIStage != UIState.Hide)
                        temp.Add(uiForm);
                }
            }

            return temp.ToArray();
        }

        public static IBaseUIForm[] GetAllNewInstanceCloseUI()
        {

            List<IBaseUIForm> temp = new List<IBaseUIForm>();
            foreach (var uiFormDic in Instance._AllNewInstanceCacheUIFormDic.Values)
            {
                foreach (IBaseUIForm uiForm in uiFormDic.Values)
                {
                    if (uiForm.UIStage == UIState.Hide)
                        temp.Add(uiForm);
                }
            }

            return temp.ToArray();
        }


        /// <summary>
        /// 得到所有 NewInstanceUI 窗口
        /// </summary>
        /// <typeparam name="TBaseUI">窗口类型</typeparam>
        /// <returns></returns>
        public static IBaseUIForm[] GetAllNewInstanceOpenUI<TBaseUI>() where TBaseUI : IBaseUIForm
        {
            string name = typeof(TBaseUI).Name;
            List<IBaseUIForm> temp = new List<IBaseUIForm>();
            if (Instance._AllNewInstanceCacheUIFormDic.ContainsKey(name))
            {
                var uiFormDic = Instance._AllNewInstanceCacheUIFormDic[name];
                foreach (IBaseUIForm uiForm in uiFormDic.Values)
                {
                    if (uiForm.UIStage != UIState.Hide)
                    {
                        temp.Add(uiForm);
                    }
                }
            }

            return temp.ToArray();
        }

        public static IBaseUIForm[] GetAllNewInstanceCloseUI<TBaseUI>()
        {
            string name = typeof(TBaseUI).Name;
            List<IBaseUIForm> temp = new List<IBaseUIForm>();
            if (Instance._AllNewInstanceCacheUIFormDic.ContainsKey(name))
            {
                var uiFormDic = Instance._AllNewInstanceCacheUIFormDic[name];
                foreach (IBaseUIForm uiForm in uiFormDic.Values)
                {
                    if (uiForm.UIStage == UIState.Hide)
                    {
                        temp.Add(uiForm);
                    }
                }
            }

            return temp.ToArray();
        }

        /// <summary>
        /// 销毁所有 关闭的普通窗体
        /// </summary>
        public static void DestoryAllNoramlUI()
        {
            List<IBaseUIForm> uiForms = new List<IBaseUIForm>();
            foreach (IBaseUIForm ui in Instance._AllCacheNormalUIFormDic.Values)
            {
                if (ui.UIStage == UIState.Hide)
                {
                    uiForms.Add(ui);
                }
            }

            for (int i = uiForms.Count -1; i >= 0; i--)
            {
                uiForms[i].DestoryUI();
            }

            uiForms.Clear();
        }

        /// <summary>
        /// 销毁所有 关闭的NewInstance窗体
        /// </summary>
        public static void DestoryAllNewInstanceUI()
        {
            List<IBaseUIForm> uiForms = new List<IBaseUIForm>();
            foreach (Dictionary<int, IBaseUIForm> dic in Instance._AllNewInstanceCacheUIFormDic.Values)
            {
                foreach (IBaseUIForm ui in dic.Values)
                {
                    if (ui.UIStage == UIState.Hide)
                    {
                        uiForms.Add(ui);
                    }
                }
            }

            for (int i = uiForms.Count - 1; i >= 0; i--)
            {
                uiForms[i].DestoryUI();
            }

            uiForms.Clear();
        }

        #endregion

        #region Get

        /// <summary>
        /// 正在打开窗体
        /// </summary>
        public static IBaseUIForm[] OpeningUIForms => Instance._OpeningUIFormList.ToArray();

        /// <summary>
        /// 正在关闭窗体
        /// </summary>
        public static IBaseUIForm[] ClosingUIForms => Instance._ClosingUIFormList.ToArray();

        /// <summary>
        /// 正在冻结窗体
        /// </summary>
        public static IBaseUIForm[] FreezingUIForms => Instance._FreezingUIFormList.ToArray();

        /// <summary>
        /// 正在解冻窗体
        /// </summary>
        public static IBaseUIForm[] ThawingUIForms => Instance._ThawingUIFormList.ToArray();

        #endregion

        #region 缓存

        /// <summary>
        /// UI窗体预设路径(参数1：窗体预设名称，2：表示窗体预设路径)
        /// </summary>
        private Dictionary<string, string> _UIFormsPathDic { get; } = new Dictionary<string, string>();

        #region Normal

        /// <summary>
        /// 所有 没有被销毁 的Normal UI窗体
        /// </summary>
        private Dictionary<string, IBaseUIForm> _AllCacheNormalUIFormDic { get; } =  new Dictionary<string, IBaseUIForm>();

        /// <summary>
        /// 当前 OpenUI窗体
        /// </summary>
        private Dictionary<string, IBaseUIForm> _CurrentOpenNormalUIFormDic { get; } = new Dictionary<string, IBaseUIForm>();

        /// <summary>
        /// 当前 OpenUI窗体 名字
        /// </summary>
        private List<string> _CurrentOpenNormalUIFormList { get; } = new List<string>();

        /// <summary>
        /// 添加打开窗口
        /// </summary>
        /// <param name="uiForm"></param>
        private void AddNormalCurrentOpenUI(IBaseUIForm uiForm)
        {
            if (uiForm.CurrentUIType.UIFormsShowMode != UIFormShowMode.NewInstance)
            {
                string uiName = uiForm.name;
                if (IsOpenUI(uiName) == false)
                {
                    _CurrentOpenNormalUIFormList.Add(uiName);
                    _CurrentOpenNormalUIFormDic.Add(uiName, uiForm);
                }
                else
                {
                    Debug.LogError("当前窗口已经打开 openUI :" + uiName);
                }
            }
        }

        /// <summary>
        /// 移除打开窗口
        /// </summary>
        /// <param name="uiName"></param>
        private void RemoveNormalCurrentOpenUI(IBaseUIForm uiForm)
        {
            if (uiForm.CurrentUIType.UIFormsShowMode != UIFormShowMode.NewInstance)
            {
                if (IsOpenUI(uiForm.name))
                {
                    _CurrentOpenNormalUIFormList.Remove(uiForm.name);
                    _CurrentOpenNormalUIFormDic.Remove(uiForm.name);
                }
                else
                {
                    Debug.LogWarning("当前窗口没有打开 closeUI :" + uiForm.name);
                }
            }

        }

        #endregion

        #region NewInstance

        /// <summary>
        /// 所有没被销毁 NewInstance UI窗体
        /// </summary>
        private Dictionary<string, Dictionary<int, IBaseUIForm>> _AllNewInstanceCacheUIFormDic { get; } = new Dictionary<string, Dictionary<int, IBaseUIForm>>();

        /// <summary>
        /// 当前OpenUI NewInstance窗体UI显示
        /// </summary>
        private Dictionary<int, IBaseUIForm> _CurrentOpenNewInstanceUIFormDic { get; } = new Dictionary<int, IBaseUIForm>();

        private List<int> _CurrentOpenNewInstanceUIFormList { get; } = new List<int>();


        /// <summary>
        /// 判断当前是否有NewInstanceUI
        /// </summary>
        /// <param name="name">窗口名字</param>
        /// <returns></returns>
        private bool ExistNewInstanceUI(int id)
        {
            return _CurrentOpenNewInstanceUIFormList.Contains(id);
        }


        /// <summary>
        /// 添加NewInstanceUI
        /// </summary>
        /// <param name="uiForm"></param>
        private void AddNewInstanceCurrentOpenUI(IBaseUIForm uiForm)
        {
            int id = uiForm.ID;
            if (ExistNewInstanceUI(id) == false)
            {
                _CurrentOpenNewInstanceUIFormList.Add(id);
                _CurrentOpenNewInstanceUIFormDic.Add(id, uiForm);
            }
            else
            {
                Debug.LogError("已经存在当前NewInstanceUI  ui:" + uiForm + "id :" + id);
            }
        }

        /// <summary>
        /// 移除NewInstanceUI
        /// </summary>
        /// <param name="uiName">窗口id</param>
        private void RemoveNewInstanceCurrentOpenUI(IBaseUIForm uiForm)
        {
            int id = uiForm.ID;
            if (ExistNewInstanceUI(id))
            {
                _CurrentOpenNewInstanceUIFormList.Remove(id);
                _CurrentOpenNewInstanceUIFormDic.Remove(id);
            }
            else
            {
                Debug.LogError("没有当前NewInstanceUI  id :" + id);
            }
        }

        private void AddCacheNewInstanceUI(IBaseUIForm uiForm)
        {
            Dictionary<int, IBaseUIForm> temp = null;
            _AllNewInstanceCacheUIFormDic.TryGetValue(uiForm.name, out temp);
            if (temp == null)
            {
                temp = new Dictionary<int, IBaseUIForm>();
                _AllNewInstanceCacheUIFormDic.Add(uiForm.name, temp);
            }

            if (!temp.ContainsKey(uiForm.ID))
            {
                temp.Add(uiForm.ID, uiForm);
            }
            else
            {
                Debug.LogError("已经添加当前ui " + uiForm.name + "id " + uiForm.ID);
            }
        }

        private void RemoveCacheNewInstance(IBaseUIForm uiForm)
        {
            Dictionary<int, IBaseUIForm> temp = null;
            _AllNewInstanceCacheUIFormDic.TryGetValue(uiForm.name, out temp);
            if (temp != null)
            {
                if (temp.ContainsKey(uiForm.ID))
                {
                    temp.Remove(uiForm.ID);
                }

                if (temp.Count == 0)
                {
                    temp.Clear();
                    _AllNewInstanceCacheUIFormDic.Remove(uiForm.name);
                }
            }
        }


        #endregion


        #region Ctrling

        /// <summary>
        /// 正在打开列表
        /// </summary>
        private List<IBaseUIForm> _OpeningUIFormList { get; } = new List<IBaseUIForm>();
        /// <summary>
        /// 正在关闭列表
        /// </summary>
        private List<IBaseUIForm> _ClosingUIFormList { get; } = new List<IBaseUIForm>();
        /// <summary>
        /// 正在冻结列表
        /// </summary>
        private List<IBaseUIForm> _FreezingUIFormList { get; } = new List<IBaseUIForm>();
        /// <summary>
        /// 正在解冻列表
        /// </summary>
        private List<IBaseUIForm> _ThawingUIFormList { get; } = new List<IBaseUIForm>();

        #endregion


        #endregion

        /// <summary>
        /// 定义“栈”集合,存储显示当前所有[弹窗]类型
        /// </summary>
        private Stack<IBaseUIForm> _CurrentUIFormStack { get; } = new Stack<IBaseUIForm>();

        /// <summary>
        /// 页面按键焦点缓存
        /// </summary>
        private Dictionary<string, string[]> _UIFormCurrentFocus { get; } = new Dictionary<string, string[]>();

        #region UI操作添加

        internal void AddOpeningUIForm(IBaseUIForm uiForm)
        {
            if (!_OpeningUIFormList.Contains(uiForm))
            {
                _OpeningUIFormList.Add(uiForm);
            }
            else
            {
                Debug.LogError("当前窗口正在打开");
            }
        }

        internal void RemoveOpeningUIForm(IBaseUIForm uiForm)
        {
            if (_OpeningUIFormList.Contains(uiForm))
            {
                _OpeningUIFormList.Remove(uiForm);
            }
            else
            {
                Debug.LogError("当前窗口已经关闭 + " + uiForm.name);
            }
        }

        internal void AddCloseingUIForm(IBaseUIForm uiForm)
        {
            if (!_ClosingUIFormList.Contains(uiForm))
            {
                _ClosingUIFormList.Add(uiForm);
            }
            else
            {
                Debug.LogError("当前窗口正在打开");
            }
            

        }

        internal void RemoveCloseingUIForm(IBaseUIForm uiForm)
        {
            if (_ClosingUIFormList.Contains(uiForm))
            {
                _ClosingUIFormList.Remove(uiForm);
            }
            else
            {
                Debug.LogError("当前窗口已经关闭");
            }
        }

        internal void AddFreezeingUIForm(IBaseUIForm uiForm)
        {
            if (!_FreezingUIFormList.Contains(uiForm))
            {
                _FreezingUIFormList.Add(uiForm);
            }
            else
            {
                Debug.LogError("当前窗口正在冻结");
            }

        }

        internal void RemoveFreezeingUIForm(IBaseUIForm uiForm)
        {
            if (_FreezingUIFormList.Contains(uiForm))
            {
                _FreezingUIFormList.Remove(uiForm);
            }
            else
            {
                Debug.LogError("当前窗口正在冻结");
            }
        }

        internal void AddThawingUIForm(IBaseUIForm uiForm)
        {
            if (!_ThawingUIFormList.Contains(uiForm))
            {
                _ThawingUIFormList.Add(uiForm);
            }
            else
            {
                Debug.LogError("当前窗口正在冻结");
            }

        }

        internal void RemoveThawingUIForm(IBaseUIForm uiForm)
        {
            if (_ThawingUIFormList.Contains(uiForm))
            {
                _ThawingUIFormList.Remove(uiForm);
            }
            else
            {
                Debug.LogError("当前窗口正在冻结");
            }
        }

        #endregion

        #region 控制

        internal void AddCtrlUIForm(IBaseUIForm uiFrom)
        {
            _InputEventSystem.AddCtrlUIForm(uiFrom);
        }

        internal void RemoveCtrlUIForm(IBaseUIForm uiFrom)
        {
            _InputEventSystem.RemoveCtrlUIForm(uiFrom);
        }

        internal List<IBaseUIForm> CtrlUIForm()
        {
            return _InputEventSystem.CtrlUIForm();
        }

        /// <summary>
        /// 保存焦点
        /// </summary>
        /// <param UIName="uiForm"></param>
        /// <param UIName="button"></param>
        internal void SaveFocus(UIFormLogic uiForm, LtButton button, LtButton button_2P)
        {
            if (!_UIFormCurrentFocus.ContainsKey(uiForm.name))
            {
                string[] btnCache = new string[2];
                _UIFormCurrentFocus.Add(uiForm.name, btnCache);
            }

            string[] focus = _UIFormCurrentFocus[uiForm.name];

            focus[0] = button.name;
            if (button_2P != null)
            {
                focus[1] = button_2P.name;
            }
            else
            {
                focus[1] = string.Empty;
            }
        }

        /// <summary>
        /// 得到焦点
        /// </summary>
        /// <param UIName="uiForm"></param>
        /// <returns></returns>
        internal string[] GetFocus(UIFormLogic uiForm)
        {
            string[] focus = null;
            if (_UIFormCurrentFocus.ContainsKey(uiForm.name))
            {
                focus = new string[2];
                for (int i = 0; i < 2; i++)
                {
                    focus[i] = _UIFormCurrentFocus[uiForm.name][i];
                }
            }

            return focus;
        }

        #endregion

        #region 添加命令

        private ClassObjectPool<UICommand> _UICommandPool = new ClassObjectPool<UICommand>(20);

        internal void AddOpenCommand(IBaseUIForm optionUI, float time,params object[] paramValues)
        {
            if (optionUI.CurrentUIType.UIFormsShowMode != UIFormShowMode.NewInstance)
            {
                AddNormalCurrentOpenUI(optionUI);
            }
            else
            {
                AddNewInstanceCurrentOpenUI(optionUI);
            }

            optionUI.DisableSelectCount--;
            UICommand open = _UICommandPool.Spawn();
            open.InitDate(UICommandType.OpenUI, optionUI, time, paramValues);
            _Invoker.AddCommand(open);


            //Debug.LogError("打开 :" + optionUI.name);
        }

        internal void AddOpenUIEndCommand(IBaseUIForm optionUI, params object[] paramValues)
        {
            UICommand openUIDown = _UICommandPool.Spawn();
            openUIDown.InitDate(UICommandType.OpenEndUI, optionUI, 0, paramValues);
            _Invoker.AddCommand(openUIDown);
        }

        internal void AddShowUICommand(IBaseUIForm optionUI, float time, params object[] paramValues)
        {
            UICommand showUI = _UICommandPool.Spawn();
            showUI.InitDate(UICommandType.ShowUI, optionUI, time, paramValues);
            _Invoker.AddCommandLastSecond(showUI);
        }

        internal void CancelShowUI(IBaseUIForm optionUI)
        {
            _Invoker.CancelShowUI(optionUI);
        }


        internal void AddFreezeCommand(IBaseUIForm optionUI, float time, params object[] paramValues)
        {
            optionUI.DisableSelectCount++;
            UICommand openUIDown = _UICommandPool.Spawn();
            openUIDown.InitDate(UICommandType.FreezeUI, optionUI, time, paramValues);
            _Invoker.AddCommand(openUIDown);

            //Debug.LogError("冻结 :" + optionUI.name);
        }
        internal void AddFreezeEndCommand(IBaseUIForm optionUI, params object[] paramValues)
        {
            UICommand openUIDown = _UICommandPool.Spawn();
            openUIDown.InitDate(UICommandType.FreezeEndUI, optionUI, 0, paramValues);
            _Invoker.AddCommand(openUIDown);
        }


        internal void AddThawCommand(IBaseUIForm optionUI, float time, params object[] paramValues)
        {
            optionUI.DisableSelectCount--;
            UICommand openUIDown = _UICommandPool.Spawn();
            openUIDown.InitDate(UICommandType.ThawUI, optionUI, time, paramValues);
            _Invoker.AddCommand(openUIDown);

            //Debug.LogError("解冻 :" + optionUI.name);
        }

        internal void AddThawEndCommand(IBaseUIForm optionUI, params object[] paramValues)
        {
            UICommand openUIDown = _UICommandPool.Spawn();
            openUIDown.InitDate(UICommandType.ThawEndUI, optionUI, 0, paramValues);
            _Invoker.AddCommand(openUIDown);
        }

        internal void AddCloseCommand(IBaseUIForm optionUI, float time, params object[] paramValues)
        {
            optionUI.DisableSelectCount++;

            if (optionUI.CurrentUIType.UIFormsShowMode != UIFormShowMode.NewInstance)
            {
                RemoveNormalCurrentOpenUI(optionUI);
            }
            else
            {
                RemoveNewInstanceCurrentOpenUI(optionUI);
            }

            UICommand openUIDown = _UICommandPool.Spawn();
            openUIDown.InitDate(UICommandType.CloseUI, optionUI, time, paramValues);
            _Invoker.AddCommand(openUIDown);

            //Debug.LogError("关闭 :" + optionUI.name);
        }

        internal void AddCloseEndCommand(IBaseUIForm optionUI, params object[] paramValues)
        {
            UICommand openUIDown = _UICommandPool.Spawn();
            openUIDown.InitDate(UICommandType.CloseEndUI, optionUI, 0, paramValues);
            _Invoker.AddCommand(openUIDown);
        }

        internal void AddDestroyCommand(IBaseUIForm optionUI)
        {
            if (_AllCacheNormalUIFormDic.ContainsKey(optionUI.name))
            {
                _AllCacheNormalUIFormDic.Remove(optionUI.name);
            }

            RemoveCacheNewInstance(optionUI);

            UICommand openUIDown = _UICommandPool.Spawn();
            openUIDown.InitDate(UICommandType.DestroyUI, optionUI, 0, null);
            _Invoker.AddCommand(openUIDown);

            //Debug.LogError("销毁 :" + optionUI.name);
        }

        #endregion

        internal bool SendNavigationEvents
        {
            get { return EventSystem.sendNavigationEvents; }
            set
            {
                EventSystem.sendNavigationEvents = value;
                touchMask = !value;
            }
        }

        internal static UIMonoManager Instance => MonoSingletonProperty<UIMonoManager>.Instance;

        public void OnSingletonInit()
        {

        }

        //初始化核心数据，加载“UI窗体路径”到集合中。
        void Awake()
        {
            _Invoker = new CommandInvoker();
            _InputEventSystem = new InputEventSystem();

            //初始化加载（根UI窗体）Canvas预设
            InitRootCanvas();
            //得到UI根节点、全屏节点、固定节点、弹出节点
            _TraCanvasTransfrom = GameObject.FindGameObjectWithTag(UIConfig.SysTagCanvas).transform;
            _TraNormal = _TraCanvasTransfrom.FindChildNode(UIConfig.SysNormalNode);
            _TraFixed = _TraCanvasTransfrom.FindChildNode(UIConfig.SysFixedNode);
            _TraPopUp = _TraCanvasTransfrom.FindChildNode(UIConfig.SysPopUpNode);
            _TarPrompt = _TraCanvasTransfrom.FindChildNode(UIConfig.SysPromptNode);
            _TraUIScripts = _TraCanvasTransfrom.FindChildNode(UIConfig.SysScriptManagerNode);
            _UICamera = _TraCanvasTransfrom.FindChildNode(UIConfig.SysUICameraNode);
            _TouchMask = _TraCanvasTransfrom.FindChildNode(UIConfig.SysTouchMask).gameObject;
            EventSystem = _TraCanvasTransfrom.GetChildComponet<EventSystem>(UIConfig.SysEventSystemNode);

            this.gameObject.transform.SetParent(_TraUIScripts);
            DontDestroyOnLoad(_TraCanvasTransfrom);
            //初始化“UI窗体预设”路径数据
            InitUIFormsPathData();
        }

        void Update()
        {
            _Invoker.Update();
            _InputEventSystem.Update();
        }

        void OnGUI()
        {
            _InputEventSystem.OnGUI();
        }

        #region 预加载 和销毁


        public void PreLoadUI(string uiName)
        {
            ObjManager.PreSpawn(_UIFormsPathDic[uiName]);
        }

        public void PreLoadUI<TBaseUIForm>() where TBaseUIForm : IBaseUIForm
        {
            string uiName = typeof(TBaseUIForm).Name;
            PreLoadUI(uiName);
        }

        public void PreLoadUIAsync(string uiName,Action<IBaseUIForm,object[]> complete)
        {

        }

        public void PreLoadUIAsync<TBaseUIForm>(Action<IBaseUIForm, object[]> complete) where TBaseUIForm : IBaseUIForm
        {
            string uiName = typeof(TBaseUIForm).Name;

        }

        #endregion


        internal bool touchMask
        {
            get { return _TouchMask.activeSelf;}
            set
            {
                _TouchMask.SetActive(value);
            }
        }


        #region 私有方法

        /// <summary>
        /// 初始化加载Canvas预设
        /// </summary>
        private void InitRootCanvas()
        {
            var prefab = Resources.Load(UIConfig.SysPathCanvas);
            GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// 根据UI窗体的名称，加载到“所有UI窗体”缓存集合中
        /// 功能： 检查“所有UI窗体”集合中，是否已经加载过，否则才加载。
        /// </summary>
        /// <param UIName="uiFormsName">UI窗体（预设）的名称</param>
        /// <returns></returns>
        private IBaseUIForm LoadFormsToAllUIFormsCatch(string uiFormsName)
        {
            IBaseUIForm baseUiResult = null; //加载的返回UI窗体基类
            _AllCacheNormalUIFormDic.TryGetValue(uiFormsName, out baseUiResult);
            if (baseUiResult == null)
            {
                //加载指定名称的“UI窗体”
                baseUiResult = LoadUIForm(uiFormsName);
            }

            return baseUiResult;
        }

        /// <summary>
        /// 加载指定名称的“UI窗体”
        /// 功能：
        ///    1：根据“UI窗体名称”，加载预设克隆体。
        ///    2：根据不同预设克隆体中带的脚本中不同的“位置信息”，加载到“根窗体”下不同的节点。
        ///    3：隐藏刚创建的UI克隆体。
        ///    4：把克隆体，加入到“所有UI窗体”（缓存）集合中。
        /// 
        /// </summary>
        /// <param UIName="uiFormName">UI窗体名称</param>
        private IBaseUIForm LoadUIForm(string uiFormName)
        {
            string strUIFormPaths = null; //UI窗体路径
            GameObject goCloneUIPrefabs = null; //创建的UI克隆体预设
            IBaseUIForm baseUiForm = null; //窗体基类
            
            //根据UI窗体名称，得到对应的加载路径
            _UIFormsPathDic.TryGetValue(uiFormName, out strUIFormPaths);
            //根据“UI窗体名称”，加载“预设克隆体”
            if (!string.IsNullOrEmpty(strUIFormPaths))
            {
                goCloneUIPrefabs = ObjManager.Spawn(strUIFormPaths);
                goCloneUIPrefabs.name = uiFormName;
            }

            //设置“UI克隆体”的父节点（根据克隆体中带的脚本中不同的“位置信息”）
            if (_TraCanvasTransfrom != null && goCloneUIPrefabs != null)
            {
                //判断是否挂载baseUIFrame脚本
                baseUiForm = goCloneUIPrefabs.GetComponent<IBaseUIForm>();
                if (baseUiForm == null)
                {
                    baseUiForm = AddIbaseUIForm(goCloneUIPrefabs);
                }
                else
                {
                    if (baseUiForm.name != goCloneUIPrefabs.name)
                    {
                        Debug.LogError("添加脚本错误 脚本名字和UI窗体名字可以不一样" + goCloneUIPrefabs.name);
                        Destroy(baseUiForm);
                        baseUiForm = AddIbaseUIForm(goCloneUIPrefabs);
                    }
                }

                if (baseUiForm == null)
                {
                    Debug.LogError("建议将请将脚本挂载到prefab上,节省查找性能 prefab名字 :" + goCloneUIPrefabs.name);

                    Assembly assembly = typeof(IBaseUIForm).Assembly;
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.Name == goCloneUIPrefabs.name)
                        {
                            goCloneUIPrefabs.gameObject.AddComponent(type);
                            baseUiForm = goCloneUIPrefabs.GetComponent<IBaseUIForm>();
                        }
                    }
                }

                if (baseUiForm == null)
                {
                    Debug.LogError("脚本有命名空间 动态添加脚本 UIName: " + goCloneUIPrefabs.name);
                }


                baseUiForm.InitUIMode();

                //设置节点
                switch (baseUiForm.CurrentUIType.UIFormsType)
                {
                    case UIFormType.Normal:
                        goCloneUIPrefabs.transform.SetParent(_TraNormal, false);
                        break;
                    case UIFormType.Fixed:
                        goCloneUIPrefabs.transform.SetParent(_TraFixed, false);
                        break;
                    case UIFormType.PopUp:
                        goCloneUIPrefabs.transform.SetParent(_TraPopUp, false);
                        break;
                    case UIFormType.Prompt:
                        goCloneUIPrefabs.transform.SetParent(_TarPrompt, false);
                        break;
                    default:
                        Debug.LogError("未定义类型 " + baseUiForm.CurrentUIType.UIFormsType);
                        break;
                }

                //设置隐藏
                goCloneUIPrefabs.SetActive(false);
                //把克隆体，加入到“所有UI窗体”（缓存）集合中。
                switch (baseUiForm.CurrentUIType.UIFormsShowMode)
                {
                    case UIFormShowMode.Normal:
                    //case UIFormShowMode.Root:
                        _AllCacheNormalUIFormDic[uiFormName] = baseUiForm;
                        break;
                    case UIFormShowMode.NewInstance:
                        AddCacheNewInstanceUI(baseUiForm);
                        break;
                    default:
                        Debug.LogError("未定义类型 " + baseUiForm.CurrentUIType.UIFormsShowMode);
                        break;
                }

                //_AllCacheUIFormDic.Add(uiFormName, baseUiForm);
                return baseUiForm;
            }
            else
            {
                Debug.Log("_TraCanvasTransfrom==null Or goCloneUIPrefabs==null!! ,Plese Check!, 参数uiFormName=" +
                          uiFormName);
            }

            Debug.Log("出现不可以预估的错误，请检查，参数 uiFormName=" + uiFormName);
            return null;
        }

        private IBaseUIForm AddIbaseUIForm(GameObject uiObj)
        {
            try
            {
                Type uiType = Type.GetType(uiObj.name);
                uiObj.gameObject.AddComponent(uiType);
                return uiObj.GetComponent<IBaseUIForm>();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Debug.LogError("添加脚本错误 没有和窗体同名的脚本" + uiObj.name);
                return null;
            }
        }

        /// <summary>
        /// 初始化“UI窗体预设”路径数据
        /// </summary>
        private void InitUIFormsPathData()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(BaseUIForm));
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.BaseType == typeof(BaseUIForm))
                {
                    string path = UIConfig.SysUIPrefabsPath + type.Name;
                    _UIFormsPathDic.Add(type.Name, path);
                }
            }
        }
        #endregion

    }
}
