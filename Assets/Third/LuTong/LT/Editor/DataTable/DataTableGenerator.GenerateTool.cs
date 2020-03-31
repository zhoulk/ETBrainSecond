/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/06/27
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System.IO;
using System.Text;

namespace LT.Editor.DataTable
{
    internal partial class DataTableGenerator
    {
        /// <summary>
        /// 生成工具
        /// </summary>
        public class GenerateTool
        {
            /// <summary>
            /// 制表符串
            /// </summary>
            private static string tabString = "";

            /// <summary>
            /// 层级
            /// </summary>
            private static int tabLevel = 0;

            /// <summary>
            /// 回车
            /// </summary>
            public static void Enter(StringBuilder sw)
            {
                sw.Append("\r");
            }

            /// <summary>
            /// 开始
            /// </summary>
            public static void AddTab(StringBuilder sw)
            {
                tabLevel++;

                if (tabLevel > 0) tabString += "\t";
            }

            /// <summary>
            /// 写入一行内容
            /// </summary>
            public static void AppendLine(StringBuilder sw, string content)
            {
                sw.Append(tabString);
                sw.AppendLine(content);
            }

            /// <summary>
            /// 写入一行内容
            /// </summary>
            /// <param name="sw"></param>
            /// <param name="content"></param>
            /// <param name="args"></param>
            public static void AppendFormat(StringBuilder sw, string content, params object[] args)
            {
                sw.Append(tabString);
                sw.AppendFormat(content, args).AppendLine(); ;
            }

            /// <summary>
            /// 结束
            /// </summary>
            public static void ReduceTab(StringBuilder sw)
            {
                tabLevel--;

                if (tabLevel < 0) tabLevel = 0;

                if (tabLevel >= 0) tabString = tabString.Remove(tabLevel, 1);
            }
        }
    }
}
