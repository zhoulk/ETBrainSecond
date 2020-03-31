using System;

namespace LT
{
    /// <summary>
    /// 指定MonoBehaviour的执行顺序
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class ExecutionOrderAttribute : System.Attribute
    {
        /// <summary>
        /// 深度
        /// </summary>
        public int order;

        /// <summary>
        /// 执行顺序
        /// </summary>
        /// <param name="order">深度</param>
        public ExecutionOrderAttribute(int order)
        {
            this.order = order;
        }
    }

    /// <summary>
    /// 在目标类型之后执行
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class ExecuteAfterAttribute : System.Attribute
    {
        public Type targetType;
        public int orderIncrease;

        public ExecuteAfterAttribute(Type targetType)
        {
            this.targetType = targetType;
            this.orderIncrease = 10;
        }
    }

    /// <summary>
    /// 在目标类型之前执行
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class ExecuteBeforeAttribute : System.Attribute
    {
        public Type targetType;
        public int orderDecrease;

        public ExecuteBeforeAttribute(Type targetType)
        {
            this.targetType = targetType;
            this.orderDecrease = 10;
        }
    }
}