/*
*    描述:
*          1.任务基类
*
*    开发人: 邓平
*/
using LtFramework.Util.Tools;

namespace LtFramework.Util.Task
{
    //任务类型
    public enum TaskType
    {
        Add,//叠加, 相同任务不影响同时执行
        Order,//添加, 相同任务按照添加顺序依次执行 先添加先执行
        Replace,//替换  如果有相同任务就替换
        Discard,//丢弃 如果有相同任务就抛弃新加的任务
    }

    internal enum TaskExcuType
    {
        ExcuTime,   //时间执行
        ExcuCount,  //次数执行
    }

    public abstract class ITask
    {
        //所属任务表
        protected TaskTable _TaskTable;
        //任务类型
        public TaskType TaskType = TaskType.Add;
        //任务执行类型
        internal TaskExcuType TaskExcuType { get; set; }
        //是在执行
        protected bool _IsExcu = false;
        //计时器
        protected LtTimer _TaskTimer = new LtTimer();

        public ITask(TaskTable table)
        {
            _TaskTable = table;
        }

        public float AllTime
        {
            get { return _TaskTimer.time; }
        }

        public float RemainTimer
        {
            get { return _TaskTimer.timer; }
        }



        internal abstract void StartBase();

        internal abstract void UpdateBase();

        internal abstract void FixedUpdateBase();

        internal abstract void EndInterrupt();

        internal abstract void EndComplete();

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Init()
        {

        }

        /// <summary>
        /// 开始
        /// </summary>
        public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void FixedUpdate()
        {

        }

        /// <summary>
        /// 结束
        /// </summary>
        public virtual void Complete()
        {

        }

        /// <summary>
        /// 中断
        /// </summary>
        public virtual void Interrupt()
        {

        } 
    }
}
