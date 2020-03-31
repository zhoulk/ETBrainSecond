/*
*    描述:
*          1.数量任务
*
*    开发人: 邓平
*/
namespace LtFramework.Util.Task
{
    public class ITaskCount : ITask
    {
        public ITaskCount(TaskTable table) : base(table)
        {
            TaskExcuType = TaskExcuType.ExcuCount;
            Init();
        }

        //任务执行次数
        private int _ExcuCount = 0;
        //执行间隔
        private float _ExcuInterval = 0;
        //执行计数
        private int _ExcuIndex = 0;
        //第一次执行是否计时
        private bool _ExcuOnceAfterInterval = true;

        /// <summary>
        /// 设置执行数量
        /// </summary>
        /// <param UIName="count">执行数量, 数量小于等于0 即为一直循环执行</param>
        /// <param UIName="interval">FixedUpdate 两次执行的时间间隔 interval 小于等于0 即按照 unity设置的间隔执行</param>
        /// <param name="excuOnceAfterInterval">第一次执行是否计时</param>
        public void SetExcuCount(int count, float interval,bool excuOnceAfterInterval = false)
        {
            _ExcuCount = count;
            _TaskTimer.SetTimer(interval);
            _ExcuInterval = interval;
            _ExcuOnceAfterInterval = excuOnceAfterInterval;
        }


        internal override void StartBase()
        {
            if (_IsExcu == false)
            {
                _IsExcu = true;
                _ExcuIndex = 0;
                _TaskTimer.ResetTimer();
                Start();
                //任务开始
            }
        }


        internal override void UpdateBase()
        {
            Update();
        }

        internal override void FixedUpdateBase()
        {
            if (_ExcuInterval > 0)
            {
                if (_ExcuOnceAfterInterval && _ExcuIndex == 0)
                {
                    Fixed();
                }
                else if (_TaskTimer.AlarmFixTime(true))
                {
                    Fixed();
                }
            }
            else
            {
                Fixed();
            }
        }

        

        private void Fixed()
        {
            if (_ExcuCount > 0)
            {
                _ExcuIndex++;
                if (_ExcuIndex >= _ExcuCount && _IsExcu)
                {
                    _TaskTable.RemoveTask(this);
                    FixedUpdate();
                    Complete();
                    _IsExcu = true;
                    return;
                }
                FixedUpdate();
            }
            else
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
