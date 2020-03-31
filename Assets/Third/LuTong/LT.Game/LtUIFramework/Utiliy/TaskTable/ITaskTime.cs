/*
*    描述:
*          1.时间任务
*
*    开发人: 邓平
*/
namespace LtFramework.Util.Task
{

    public abstract class ITaskTime : ITask
    {

        protected ITaskTime(TaskTable table) : base(table)
        {
            TaskExcuType = TaskExcuType.ExcuTime;
            Init();
        }

        /// <summary>
        /// 设置执行时间
        /// </summary>
        /// <param UIName="time">执行时间</param>
        public void SetExcuTime(float time)
        {
            _TaskTimer.SetTimer(time);
        }

        internal override void StartBase()
        {
            if (_IsExcu == false)
            {
                _IsExcu = true;
                _TaskTimer.ResetTimer();
                Start();
            }
        }

        internal override void UpdateBase()
        {
            StartBase();

            if (_TaskTimer.Alarm(false) && _IsExcu)
            {
                _IsExcu = false;
                _TaskTable.RemoveTask(this);
                Complete();
                return;
            }
            Update();
        }

        internal override void FixedUpdateBase()
        {
            if (_IsExcu)
            {
                FixedUpdate();
            }
        }

        internal override void EndInterrupt()
        {
            if (_IsExcu)
            {
                Interrupt();
                _IsExcu = false;
            }
        }

        internal override void EndComplete()
        {
            if (_IsExcu)
            {
                Complete();
                _IsExcu = false;
            }
        }
    }
}
