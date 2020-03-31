/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/08/10
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.IO;

namespace LT.Editor.DataTable
{
    public sealed partial class DataTableProcessor
    {
        public abstract class DataProcessor
        {
            public abstract Type Type
            {
                get;
            }

            public abstract bool IsId
            {
                get;
            }

            public abstract bool IsComment
            {
                get;
            }

            public abstract bool IsSystem
            {
                get;
            }

            public abstract string LanguageKeyword
            {
                get;
            }

            public abstract string[] GetTypeStrings();

            public abstract void WriteToStream(BinaryWriter stream, string value);
        }
    }
}
