/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/06/17
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using OfficeOpenXml;
using LT.DataTable;

namespace LT.Editor.DataTable
{
    /// <summary>
    /// 数据表生成器
    /// </summary>
    internal partial class DataTableGenerator
    {
        #region 属性
        /// <summary>
        /// C# 数据表代码模板路径
        /// </summary>
        private const string CSharpCodeTemplateFileName = "/LT/Runtime/Providers/DataTable/Editor/Template/DataTableCodeTemplate.txt";
        private const string CSharpAssetCodeTemplateFileName = "/LT/Runtime/Providers/DataTable/Editor/Template/DataTableScriptableObjectTemplate.txt";

        /// <summary>
        /// 数据表路径
        /// </summary>
        public static string DataTablePath
        {
            get { return EditorPrefs.GetString("DataTablePath", ""); }
            set { EditorPrefs.SetString("DataTablePath", value); }
        }

        /// <summary>
        /// 代码输出路径
        /// </summary>
        public static string CSharpCodePath
        {
            get { return EditorPrefs.GetString("CSharpCodePath", "/Generated/Code"); }
            set { EditorPrefs.SetString("CSharpCodePath", value); }
        }

        /// <summary>
        /// 资源输出路径
        /// </summary>
        public static string AssetPath
        {
            get { return EditorPrefs.GetString("TablesPath", "/Generated/Asset"); }
            set { EditorPrefs.SetString("TablesPath", value); }
        }

        private static readonly Regex EndWithNumberRegex = new Regex(@"\d+$");
        private static readonly Regex NameRegex = new Regex(@"^[A-Z][A-Za-z0-9_]*$");
        #endregion

        /// <summary>
        /// 处理所有数据表
        /// </summary>
        public static void GenerateDataTables()
        {
            // 全路径
            string fullPath = UnityEngine.Application.dataPath + DataTablePath;

            try
            {
                using (ExcelPackage package = new ExcelPackage(new FileStream(fullPath, FileMode.Open)))
                {
                    // 遍历表单sheet
                    List<GenerateCollection> generateCollection = new List<GenerateCollection>();
                    for (int i = 1; i <= package.Workbook.Worksheets.Count; i++)
                    {
                        EditorUtility.DisplayProgressBar("生成数据资源", "正在写入数据...", (float)i / package.Workbook.Worksheets.Count);
                        var worksheet = package.Workbook.Worksheets[i];
                        var dataTableProcessor = CreateDataTableProcessor(worksheet);
                        if (!CheckRawData(dataTableProcessor, worksheet.Name))
                        {
                            Debug.LogError(string.Format("Check raw data failure. DataTableName='{0}'", worksheet.Name));
                            break;
                        }

                        var bSuccess = false;
                        var dataTableName = Path.GetFileNameWithoutExtension(worksheet.Name);
                        bSuccess = GenerateCodeFile(dataTableProcessor, dataTableName);
                        bSuccess = GenerateScriptableObject(dataTableProcessor, dataTableName);

                        if (bSuccess)
                        {
                            AssetDatabase.Refresh();
                            generateCollection.Add(new GenerateCollection() { dataTableProcessor = dataTableProcessor, dataTableName = dataTableName, worksheet = worksheet });
                        }
                    }

                    AssetDatabase.Refresh();
                    foreach (var collect in generateCollection)
                    {
                        ParseDataRow(collect.dataTableProcessor, collect.dataTableName, collect.worksheet);
                    }
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        /// <summary>
        /// 创建数据表处理器
        /// </summary>
        private static DataTableProcessor CreateDataTableProcessor(ExcelWorksheet sheet)
        {
            return new DataTableProcessor(sheet, 1, 2, -1, 3, 4, 1);
        }

        /// <summary>
        /// 检测关键字命名规范
        /// </summary>
        public static bool CheckRawData(DataTableProcessor dataTableProcessor, string dataTableName)
        {
            for (int i = 0; i < dataTableProcessor.RawColumnCount; i++)
            {
                string name = dataTableProcessor.GetName(i);
                if (string.IsNullOrEmpty(name) || name == "#")
                {
                    continue;
                }

                if (!NameRegex.IsMatch(name))
                {
                    Debug.LogWarning(string.Format("Check raw data failure. DataTableName='{0}' Name='{1}'", dataTableName, name));
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 生成数据行代码
        /// </summary>
        public static bool GenerateCodeFile(DataTableProcessor dataTableProcessor, string dataTableName)
        {
            string fullPath = UnityEngine.Application.dataPath + CSharpCodeTemplateFileName;
            dataTableProcessor.SetCodeTemplate(fullPath, Encoding.Default);
            dataTableProcessor.SetCodeGenerator(InternalDataTableCodeGenerator);

            var outputPath = UnityEngine.Application.dataPath + CSharpCodePath;
            if (!Directory.Exists(outputPath))
            {
                // 如果不存在文件夹，则创建
                Directory.CreateDirectory(outputPath);
            }

            string filename = outputPath + "/DR" + dataTableName + ".cs";
            if (!dataTableProcessor.GenerateCodeFile(filename, Encoding.Default, dataTableName) && File.Exists(filename))
            {
                // 如果生成失败，则删除已写入的错误部分。
                File.Delete(filename);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 生成数据表代码
        /// </summary>
        private static void InternalDataTableCodeGenerator(DataTableProcessor dataTableProcessor, StringBuilder codeContent, object userData)
        {
            string dataTableName = (string)userData;

            codeContent.Replace("__DATA_TABLE_CREATE_TIME__", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            codeContent.Replace("__DATA_TABLE_CLASS_NAME1__", "DR" + dataTableName);
            codeContent.Replace("__DATA_TABLE_CLASS_NAME2__", "DR" + dataTableName);
            codeContent.Replace("__DATA_TABLE_COMMENT__", dataTableProcessor.GetValue(0, 1) + "。");
            codeContent.Replace("__DATA_TABLE_ID_COMMENT__", dataTableProcessor.GetComment(dataTableProcessor.IdColumn) + "。");
            codeContent.Replace("__DATA_TABLE_PROPERTIES__", GenerateDataTableProperties(dataTableProcessor));
            codeContent.Replace("__DATA_TABLE_STRING_PARSER__", GenerateDataTableStringParser(dataTableProcessor));
            //codeContent.Replace("__DATA_TABLE_PROPERTY_ARRAY__", GenerateDataTablePropertyArray(dataTableProcessor));
            codeContent.Replace("__DATA_TABLE_IDS__", GenerateDataTableRowIds(dataTableProcessor));
        }

        /// <summary>
        /// 生成ScriptableObject表类
        /// </summary>
        public static bool GenerateScriptableObject(DataTableProcessor dataTableProcessor, string dataTableName)
        {
            string fullPath = UnityEngine.Application.dataPath + CSharpAssetCodeTemplateFileName;
            dataTableProcessor.SetCodeTemplate(fullPath, Encoding.Default);
            dataTableProcessor.SetCodeGenerator(InternalDataTableAssetCodeGenerator);

            var outputPath = UnityEngine.Application.dataPath + CSharpCodePath + "/ScriptObjects/";
            if (!Directory.Exists(outputPath))
            {
                // 如果不存在文件夹，则创建
                Directory.CreateDirectory(outputPath);
            }

            var filename = outputPath + dataTableName + "ScriptObject.cs";
            if (!dataTableProcessor.GenerateCodeFile(filename, Encoding.Default, dataTableName) && File.Exists(filename))
            {
                // 如果生成失败，则删除已写入的错误部分。
                File.Delete(filename);
                return false;
            }
            return true;
        }

        private static void InternalDataTableAssetCodeGenerator(DataTableProcessor dataTableProcessor, StringBuilder codeContent, object userData)
        {
            string dataTableName = (string)userData;

            codeContent.Replace("__DATA_TABLE_CREATE_TIME__", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            codeContent.Replace("__DATA_TABLE_CLASS_NAME__", dataTableName + "ScriptObject");
            codeContent.Replace("__DATA_ROW_CLASS_NAME__", "DR" + dataTableName);
        }

        private static string GenerateDataTableProperties(DataTableProcessor dataTableProcessor)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dataTableProcessor.RawColumnCount; i++)
            {
                if (dataTableProcessor.IsCommentColumn(i))
                {
                    // 注释列
                    continue;
                }

                if (dataTableProcessor.IsIdColumn(i))
                {
                    // 编号列
                    continue;
                }

                GenerateTool.AddTab(sb);
                GenerateTool.AppendLine(sb, "/// <summary>");
                GenerateTool.AppendFormat(sb, "/// {0}。", dataTableProcessor.GetComment(i));
                GenerateTool.AppendLine(sb, "/// </summary>");
                GenerateTool.AppendFormat(sb, "public {0} {1};", dataTableProcessor.GetLanguageKeyword(i), dataTableProcessor.GetName(i));
                GenerateTool.ReduceTab(sb);
                GenerateTool.Enter(sb);
            }

            return sb.ToString();
        }

        private static string GenerateDataTableStringParser(DataTableProcessor dataTableProcessor)
        {
            StringBuilder sb = new StringBuilder();

            GenerateTool.AddTab(sb);
            GenerateTool.AppendLine(sb, "public bool ParseDataRow(string[] content)");
            GenerateTool.AppendLine(sb, "{");

            GenerateTool.AddTab(sb);
            GenerateTool.AppendLine(sb, "int index = 0;");

            for (int i = 0; i < dataTableProcessor.RawColumnCount; i++)
            {
                if (dataTableProcessor.IsCommentColumn(i))
                {
                    // 注释列
                    GenerateTool.AppendLine(sb, "index++;");
                    continue;
                }

                if (dataTableProcessor.IsIdColumn(i))
                {
                    // 编号列
                    GenerateTool.AppendLine(sb, "this.id = int.Parse(content[index++]);");
                    continue;
                }

                if (dataTableProcessor.IsSystem(i))
                {
                    string languageKeyword = dataTableProcessor.GetLanguageKeyword(i);
                    if (languageKeyword == "string")
                    {
                        GenerateTool.AppendFormat(sb, "{0} = content[index++];", dataTableProcessor.GetName(i));
                    }
                    else
                    {
                        GenerateTool.AppendFormat(sb, "{0} = {1}.Parse(content[index++]);", dataTableProcessor.GetName(i), languageKeyword);
                    }
                }
                else
                {
                    // todo
                    throw new LogicException("Non-system type is not supported at this time");
                }
            }

            GenerateTool.AppendLine(sb, "return true;");
            GenerateTool.ReduceTab(sb);
            GenerateTool.AppendLine(sb, "}");
            GenerateTool.ReduceTab(sb);
            GenerateTool.Enter(sb);

            return sb.ToString();
        }

        private static string GenerateDataTablePropertyArray(DataTableProcessor dataTableProcessor)
        {
            List<PropertyCollection> propertyCollections = new List<PropertyCollection>();
            for (int i = 0; i < dataTableProcessor.RawColumnCount; i++)
            {
                if (dataTableProcessor.IsCommentColumn(i))
                {
                    // 注释列
                    continue;
                }

                if (dataTableProcessor.IsIdColumn(i))
                {
                    // 编号列
                    continue;
                }

                string name = dataTableProcessor.GetName(i);
                if (!EndWithNumberRegex.IsMatch(name))
                {
                    continue;
                }

                string propertyCollectionName = EndWithNumberRegex.Replace(name, string.Empty);
                int id = int.Parse(EndWithNumberRegex.Match(name).Value);

                PropertyCollection propertyCollection = null;
                foreach (PropertyCollection pc in propertyCollections)
                {
                    if (pc.Name == propertyCollectionName)
                    {
                        propertyCollection = pc;
                        break;
                    }
                }

                if (propertyCollection == null)
                {
                    propertyCollection = new PropertyCollection(propertyCollectionName, dataTableProcessor.GetLanguageKeyword(i));
                    propertyCollections.Add(propertyCollection);
                }

                propertyCollection.AddItem(id, name);
            }

            StringBuilder sb = new StringBuilder();
            foreach (PropertyCollection propertyCollection in propertyCollections)
            {
                GenerateTool.AddTab(sb);
                GenerateTool.AppendFormat(sb, "private KeyValuePair<int, {1}>[] {0} = null;", propertyCollection.Name, propertyCollection.LanguageKeyword);

                // 1
                GenerateTool.Enter(sb);
                GenerateTool.AppendFormat(sb, "public int {0}Count", propertyCollection.Name);
                GenerateTool.AppendLine(sb, "{");
                GenerateTool.AddTab(sb);
                GenerateTool.AppendLine(sb, "get");
                GenerateTool.AppendLine(sb, "{");
                GenerateTool.AddTab(sb);
                GenerateTool.AppendFormat(sb, "return this.{0}.Length;", propertyCollection.Name);
                GenerateTool.ReduceTab(sb);
                GenerateTool.AppendLine(sb, "}");
                GenerateTool.ReduceTab(sb);
                GenerateTool.AppendLine(sb, "}");
                GenerateTool.ReduceTab(sb);

                // 2
                GenerateTool.Enter(sb);

                GenerateTool.AddTab(sb);
                GenerateTool.AppendFormat(sb, "public {1} Get{0}(int id)", propertyCollection.Name, propertyCollection.LanguageKeyword);
                GenerateTool.AppendLine(sb, "{");
                GenerateTool.AddTab(sb);

                GenerateTool.AppendFormat(sb, "foreach (KeyValuePair<int, {1}> i in this.{0})", propertyCollection.Name, propertyCollection.LanguageKeyword);
                GenerateTool.AppendLine(sb, "{");
                GenerateTool.AddTab(sb);

                GenerateTool.AppendLine(sb, "if (i.Key == id)");
                GenerateTool.AppendLine(sb, "{");
                GenerateTool.AddTab(sb);

                GenerateTool.AppendLine(sb, "return i.Value;");
                GenerateTool.ReduceTab(sb);
                GenerateTool.AppendLine(sb, "}");
                GenerateTool.ReduceTab(sb);
                GenerateTool.AppendLine(sb, "}");

                GenerateTool.Enter(sb);
                GenerateTool.AppendFormat(sb, "throw new ArgumentException(string.Format(\"Get{0} with invalid id '{{0}}'.\", id.ToString()));", propertyCollection.Name);
                GenerateTool.ReduceTab(sb);
                GenerateTool.AppendLine(sb, "}");
                GenerateTool.ReduceTab(sb);

                //3
                GenerateTool.Enter(sb);
                GenerateTool.AddTab(sb);
                GenerateTool.AppendFormat(sb, "public {1} Get{0}At(int index)", propertyCollection.Name, propertyCollection.LanguageKeyword);
                GenerateTool.AppendLine(sb, "{");
                GenerateTool.AddTab(sb);

                GenerateTool.AppendFormat(sb, "if (index < 0 || index >= this.{0}.Length)", propertyCollection.Name);
                GenerateTool.AppendLine(sb, "{");
                GenerateTool.AddTab(sb);
                GenerateTool.AppendFormat(sb, "throw new ArgumentException(string.Format(\"Get{0}At with invalid index '{{0}}'.\", index.ToString()));", propertyCollection.Name);
                GenerateTool.ReduceTab(sb);
                GenerateTool.AppendLine(sb, "}");
                GenerateTool.Enter(sb);
                GenerateTool.AppendFormat(sb, "return this.{0}[index].Value;", propertyCollection.Name);
                GenerateTool.ReduceTab(sb);
                GenerateTool.AppendLine(sb, "}");
                GenerateTool.ReduceTab(sb);
            }

            if (propertyCollections.Count > 0)
            {
                GenerateTool.Enter(sb);
            }

            GenerateTool.AddTab(sb);
            GenerateTool.AppendLine(sb, "private void GeneratePropertyArray()");
            GenerateTool.AppendLine(sb, "{");
            GenerateTool.AddTab(sb);

            foreach (PropertyCollection propertyCollection in propertyCollections)
            {
                GenerateTool.AppendFormat(sb, "this.{0} = new KeyValuePair<int, {1}>[]", propertyCollection.Name, propertyCollection.LanguageKeyword);
                GenerateTool.AppendLine(sb, "{");
                GenerateTool.AddTab(sb);

                int itemCount = propertyCollection.ItemCount;
                for (int i = 0; i < itemCount; i++)
                {
                    KeyValuePair<int, string> item = propertyCollection.GetItem(i);

                    GenerateTool.AppendFormat(sb, "new KeyValuePair<int, {0}>({1}, {2}),", propertyCollection.LanguageKeyword, item.Key.ToString(), item.Value);
                }

                GenerateTool.ReduceTab(sb);
                GenerateTool.AppendLine(sb, "}");
            }

            GenerateTool.ReduceTab(sb);
            GenerateTool.AppendLine(sb, "}");
            GenerateTool.ReduceTab(sb);
            return sb.ToString();
        }

        private static string GenerateDataTableRowIds(DataTableProcessor dataTableProcessor)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = dataTableProcessor.ContentStartRow; i < dataTableProcessor.RawRowCount; i++)
            {
                if (string.IsNullOrEmpty(dataTableProcessor.GetId(i)))
                    break;

                GenerateTool.AddTab(sb);
                GenerateTool.AppendLine(sb, "/// <summary>");
                GenerateTool.AppendFormat(sb, "/// {0}。", dataTableProcessor.GetIdComment(i));
                GenerateTool.AppendLine(sb, "/// </summary>");
                GenerateTool.AppendFormat(sb, "public const {0} {1} = {2}", dataTableProcessor.GetLanguageKeyword(dataTableProcessor.IdColumn), "id_" + dataTableProcessor.GetId(i), dataTableProcessor.GetId(i) + ";");
                GenerateTool.Enter(sb);
                GenerateTool.ReduceTab(sb);
            }
            return sb.ToString();
        }

        private sealed class PropertyCollection
        {
            private readonly string name;
            private readonly string languageKeyword;
            private readonly List<KeyValuePair<int, string>> items;

            public PropertyCollection(string name, string languageKeyword)
            {
                this.name = name;
                this.languageKeyword = languageKeyword;
                items = new List<KeyValuePair<int, string>>();
            }

            public string Name
            {
                get
                {
                    return name;
                }
            }

            public string LanguageKeyword
            {
                get
                {
                    return languageKeyword;
                }
            }

            public int ItemCount
            {
                get
                {
                    return items.Count;
                }
            }

            public KeyValuePair<int, string> GetItem(int index)
            {
                Guard.Verify<LogicException>(index < 0 || index >= items.Count, string.Format("GetItem with invalid index '{0}'.", index));

                return items[index];
            }

            public void AddItem(int id, string propertyName)
            {
                items.Add(new KeyValuePair<int, string>(id, propertyName));
            }
        }

        private sealed class GenerateCollection
        {
            public string dataTableName;
            public DataTableProcessor dataTableProcessor;
            public ExcelWorksheet worksheet;
        }

        /// <summary>
        /// 解析数据到Asset
        /// </summary>
        private static void ParseDataRow(DataTableProcessor dataTableProcessor, string dataTableName, ExcelWorksheet worksheet)
        {
            //Type baseType = typeof(ScriptableObjectBase<>);
            //Type drType = baseType.Assembly.GetType("DR" + dataTableName);
            //Type genericType = baseType.MakeGenericType(drType);
            //Debug.Log("generic:" + generic);
            //var obj = Activator.CreateInstance(generic) as ScriptableObjectBase;
            //var obj = ScriptableObject.CreateInstance(genericType);

            var scriptName = dataTableName + "ScriptObject";
            var instance = ScriptableObject.CreateInstance(scriptName) as ScriptableObjectBase;
            Guard.Verify<ArgumentException>(instance == null, $"{scriptName} instance is invaild.");

            for (int i = dataTableProcessor.ContentStartRow; i < worksheet.Dimension.Rows; i++)
            {
                string idString = worksheet.GetValueEx<string>(i, dataTableProcessor.IdColumn);
                if (string.IsNullOrEmpty(idString))
                {
                    // 如果 Id == null，则停止读取
                    break;
                }

                var values = worksheet.GetRow(i);
                instance.AddDataRow(values);
            }

            var outputPath = "Assets" + AssetPath;
            if (!Directory.Exists(outputPath))
            {
                // 如果不存在文件夹，则创建
                Directory.CreateDirectory(outputPath);
            }

            AssetDatabase.CreateAsset(instance, $"{outputPath}/{dataTableName}.asset");
        }
    }
}