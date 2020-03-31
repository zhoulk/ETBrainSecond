/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/08/10
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System.IO;
using UnityEngine;

namespace LT.Editor.DataTable
{
    public sealed partial class DataTableProcessor
    {
        private sealed class RectProcessor : GenericDataProcessor<Rect>
        {
            public override bool IsSystem
            {
                get
                {
                    return false;
                }
            }

            public override string LanguageKeyword
            {
                get
                {
                    return "Rect";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "rect",
                    "unityengine.rect"
                };
            }

            public override Rect Parse(string value)
            {
                string[] splitValue = value.Split(',');
                return new Rect(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]), float.Parse(splitValue[3]));
            }

            public override void WriteToStream(BinaryWriter stream, string value)
            {
                Rect rect = Parse(value);
                stream.Write(rect.x);
                stream.Write(rect.y);
                stream.Write(rect.width);
                stream.Write(rect.height);
            }
        }
    }
}
