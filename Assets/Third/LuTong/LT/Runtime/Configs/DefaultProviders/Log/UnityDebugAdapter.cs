/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/05/16
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/


namespace LT
{
    /// <summary>
    /// UnityDebug适配器
    /// </summary>
    internal class UnityDebugAdapter : ILog
    {
        private IApplication application;
        private ISyncContext context;

        public UnityDebugAdapter(IApplication app, ISyncContext context)
        {
            this.application = app;
            this.context = context;
        }

        public void Debug(object message)
        {
            if (!application.IsMainThread)
            {
                context.Post((state) => { UnityEngine.Debug.Log(message); });
                return;
            }

            UnityEngine.Debug.Log(message);
        }

        public void Warning(object message)
        {
            if (!application.IsMainThread)
            {
                context.Post((state) => { UnityEngine.Debug.LogWarning(message); });
                return;
            }

            UnityEngine.Debug.LogWarning(message);
        }

        public void Error(object message)
        {
            if (!application.IsMainThread)
            {
                context.Post((state) => { UnityEngine.Debug.LogError(message); });
                return;
            }

            UnityEngine.Debug.LogError(message);
        }
    }
}