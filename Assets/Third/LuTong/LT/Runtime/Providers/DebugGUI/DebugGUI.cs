/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/05/16
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LT.MonoDriver;

namespace LT.DebugerGUI
{
    internal class DebugGUI : IDebugGUI, IUpdate, IOnDestroy, IOnGUI
    {
        struct Log
        {
            public string message;
            public string stackTrace;
            public LogType type;
        }

        #region Inspector Settings  
        /// <summary>  
        /// 在打印的时候是否开启堆栈打印
        /// </summary>  
        public bool StackLog = false;

        /// <summary>
        /// 开启的日志
        /// </summary>
        public LogType type;

        /// <summary>  
        /// 显示字体大小
        /// </summary>  
        public float FontSize = 24;

        /// <summary>  
        /// 显示滑动条大小
        /// </summary>  
        public float ScrollbarSize = 80;

        /// <summary>  
        /// 在删除旧的日志之前保持日志的数量。 
        /// </summary>  
        public int maxLogs = 1000;
        #endregion

        readonly List<Log> logs = new List<Log>();

        /// <summary>
        /// 对应横向、纵向滑动条对应的X,Y数值
        /// </summary>
        public Vector2 scrollPosition;

        /// <summary>
        /// 可见
        /// </summary>
        private bool visible;

        /// <summary>
        /// 是否可见
        /// </summary>
        private bool itCanBeVisible;

        /// <summary>
        /// 折叠
        /// </summary>
        private bool collapse;

        private static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>
        {
            { LogType.Assert, Color.white },
            { LogType.Error, Color.red },
            { LogType.Exception, Color.magenta },
            { LogType.Log, Color.green },
            { LogType.Warning, Color.yellow },
        };
        #region OnGUI
        private const string windowTitle = "Debug（打印日志）";

        //边缘
        private const int margin = 10;

        private static readonly GUIContent clearLabel = new GUIContent("Clear", "清空打印信息.");
        private static readonly GUIContent closeLabel = new GUIContent("Close", "关闭打印面板.");
        private static readonly GUIContent collapseLabel = new GUIContent("Collapse", "隐藏重复信息.");

        private readonly Rect titleBarRect = new Rect(0, 0, 10000, 40);
        private Rect windowRect;
        private ScreenOrientation currentOrientation;
        private IMonoDriver monoDriver;
        #endregion

        public DebugGUI(IMonoDriver monoDriver)
        {
            this.monoDriver = monoDriver;
            this.itCanBeVisible = true;
            UnityEngine.Application.logMessageReceived += HandleLog;
            currentOrientation = Screen.orientation;
            RefreshWindow();
        }

        /// <inheritdoc />
        public void Update()
        {
            Running();
        }

        public void OnDestroy()
        {
            UnityEngine.Application.logMessageReceived -= HandleLog;
        }

        public void Running()
        {
            if (Input.touchCount >= 4 && itCanBeVisible)
            {
                itCanBeVisible = false;
                RefreshWindow();
                visible = !visible;
                monoDriver.StartCoroutine(DelaySetVisible());
            }

            if (((Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
                || Input.GetKeyDown(KeyCode.Alpha0))
                && itCanBeVisible)
            {
                itCanBeVisible = false;
                RefreshWindow();
                visible = !visible;
                monoDriver.StartCoroutine(DelaySetVisible());
            }

            if (App.HasBind<IInput>())
            {
                //清理日志
                if (LTInput.GetKeyDown(KeyCode2.X))
                {
                    logs.Clear();
                }

                //堆栈开关
                if (LTInput.GetKeyDown(KeyCode2.Y))
                {
                    StackLog = !StackLog;
                }

                if (LTInput.GetKey(KeyCode2.Down))
                {
                    scrollPosition += new Vector2(0, 10);
                }

                if (LTInput.GetKey(KeyCode2.Up))
                {
                    scrollPosition -= new Vector2(0, 10);
                }
            }
        }

        IEnumerator DelaySetVisible()
        {
            yield return new WaitForSeconds(1);
            itCanBeVisible = true;
        }

        public void OnGUI()
        {

            if (!visible)
            {
                return;
            }

            if (currentOrientation != Screen.orientation)
            {
                RefreshWindow();
            }
            windowRect = GUILayout.Window(666, windowRect, DrawConsoleWindow, windowTitle);
        }

        private void RefreshWindow()
        {
            windowRect = new Rect(margin, margin, Screen.width - (margin * 2), Screen.height - (margin * 2));
        }

        /// <summary>  
        /// 显示一个列出已记录日志的窗口。
        /// </summary>  
        /// <param name="windowID">Window ID.</param>  
        private void DrawConsoleWindow(int windowID)
        {
            DrawLogsList();
            DrawToolbar();

            //允许拖动window的触发范围.  
            GUI.DragWindow(titleBarRect);
        }

        /// <summary>  
        /// 绘制log列表
        /// </summary>  
        private void DrawLogsList()
        {
            GUIStyle gs_vertica = GUI.skin.verticalScrollbar;
            GUIStyle gs1_vertica = GUI.skin.verticalScrollbarThumb;

            gs_vertica.fixedWidth = ScrollbarSize;
            gs1_vertica.fixedWidth = ScrollbarSize;

            GUIStyle gs_horizontal = GUI.skin.horizontalScrollbar;
            GUIStyle gs1_horizontal = GUI.skin.horizontalScrollbarThumb;

            gs_horizontal.fixedHeight = ScrollbarSize / 2;
            gs1_horizontal.fixedHeight = ScrollbarSize / 2;

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);

            for (var i = 0; i < logs.Count; i++)
            {
                var log = logs[i];

                //如果选择折叠选项，则组合相同的消息。 
                if (collapse && i > 0)
                {
                    var previousMessage = logs[i - 1].message;

                    if (log.message == previousMessage)
                    {
                        continue;
                    }
                }
                GUI.contentColor = logTypeColors[log.type];
                GUILayout.Label(log.message);

                if (StackLog)
                {
                    GUILayout.Label(log.stackTrace);
                }
            }
            GUI.color = Color.magenta;
            GUILayout.EndScrollView();

            gs_vertica.fixedWidth = 0;
            gs1_vertica.fixedWidth = 0;

            gs_horizontal.fixedHeight = 0;
            gs1_horizontal.fixedHeight = 0;

            // 在绘制其他组件之前，确保GUI颜色被重置。  
            GUI.contentColor = Color.white;
        }

        /// <summary>  
        /// Log日志工具栏
        /// </summary>  
        private void DrawToolbar()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(clearLabel, GUILayout.Height(40)))
            {
                logs.Clear();
            }

            if (GUILayout.Button("Stack开关", GUILayout.Height(40)))
            {
                StackLog = !StackLog;
            }

            if (GUILayout.Button(closeLabel, GUILayout.Height(40)))
            {
                visible = false;
            }
            collapse = GUILayout.Toggle(collapse, collapseLabel, GUILayout.ExpandWidth(true), GUILayout.Height(40));// GUILayout.ExpandWidth保持长宽一致

            GUILayout.EndHorizontal();
        }

        /// <summary>  
        /// Debug 对应的回调处理
        /// </summary>  
        /// <param name="message">信息.</param>  
        /// <param name="stackTrace">信息的来源</param>  
        /// <param name="type">信息类型 (error, exception, warning, assert).</param>  
        private void HandleLog(string message, string stackTrace, LogType type)
        {
            if (type == LogType.Warning) return;

            logs.Add(new Log
            {
                message = "<size=" + FontSize + ">" + message + "</size>",
                stackTrace = "<size=" + FontSize + ">" + stackTrace + "</size>",
                type = type,
            });

            TrimExcessLogs();
        }

        /// <summary>  
        /// 删除超过允许的最大数量的旧日志。
        /// </summary>  
        private void TrimExcessLogs()
        {
            var amountToRemove = Mathf.Max(logs.Count - maxLogs, 0);

            if (amountToRemove == 0)
            {
                return;
            }

            logs.RemoveRange(0, amountToRemove);
        }
    }
}