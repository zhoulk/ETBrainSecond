/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/08/10
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using OfficeOpenXml;

namespace LT.Editor.DataTable
{
    public sealed partial class DataTableProcessor
    {
        /// <summary>
        /// 代码生成委托
        /// </summary>
        public delegate void DataTableCodeGenerator(DataTableProcessor dataTableProcessor, StringBuilder codeContent, object userData);

        /// <summary>
        /// 注释行标注符
        /// </summary>
        public const string CommentLineSeparator = "#";

        private readonly int nameRow;
        private readonly int typeRow;
        private readonly int defaultValueRow;
        private readonly int commentRow;
        private readonly int contentStartRow;
        private readonly int idColumn;
        private readonly int idCommentColumn;

        private readonly DataProcessor[] dataProcessor;
        private readonly ExcelWorksheet excelWorksheet;

        private string codeTemplate;
        private DataTableCodeGenerator codeGenerator;

        public DataTableProcessor(ExcelWorksheet excelWorksheet, int nameRow, int typeRow, int defaultValueRow, int commentRow, int contentStartRow, int idColumn)
        {
            this.excelWorksheet = excelWorksheet;

            Guard.Verify<LogicException>(nameRow < 0 || nameRow >= excelWorksheet.Dimension.Rows, string.Format("Name row '{0}' is invalid.", nameRow));
            Guard.Verify<LogicException>(typeRow < 0 || typeRow >= excelWorksheet.Dimension.Rows, string.Format("Type row '{0}' is invalid.", typeRow));
            Guard.Verify<LogicException>(contentStartRow < 0 || contentStartRow > excelWorksheet.Dimension.Rows, string.Format("Content start '{0}' is invalid.", contentStartRow));
            Guard.Verify<LogicException>(idColumn < 0 || idColumn >= excelWorksheet.Dimension.Rows, string.Format("Id column '{0}' is invalid.", idColumn));

            this.nameRow = nameRow;
            this.typeRow = typeRow;
            this.defaultValueRow = defaultValueRow;
            this.commentRow = commentRow;
            this.contentStartRow = contentStartRow;
            this.idColumn = idColumn;

            int rawColumnCount = this.excelWorksheet.Dimension.Columns;
            dataProcessor = new DataProcessor[rawColumnCount];
            for (int i = 0; i < rawColumnCount; i++)
            {
                if (i == IdColumn)
                {
                    dataProcessor[i] = DataProcessorUtility.GetDataProcessor("id");
                }
                else
                {
                    dataProcessor[i] = DataProcessorUtility.GetDataProcessor(excelWorksheet.GetValueEx<string>(typeRow, i));
                }
            }

            // 查找作为Id值注释的列,comment行带@符号
            this.idCommentColumn = idColumn;
            for (int i = 0; i < rawColumnCount; i++)
            {
                if (excelWorksheet.GetValueEx<string>(commentRow, i).IndexOf('@') > -1)
                {
                    idCommentColumn = i;
                    break;
                }
            }

            codeTemplate = null;
            codeGenerator = null;
        }

        public int RawRowCount
        {
            get
            {
                return excelWorksheet.Dimension.Rows;
            }
        }

        public int RawColumnCount
        {
            get
            {
                return excelWorksheet.Dimension.Columns;
            }
        }

        public int ContentStartRow
        {
            get
            {
                return contentStartRow;
            }
        }

        public int IdColumn
        {
            get
            {
                return idColumn;
            }
        }

        public bool IsIdColumn(int rawColumn)
        {
            Guard.Verify<LogicException>(rawColumn < 0 || rawColumn >= RawColumnCount,
                string.Format("Raw column '{0}' is out of range.", rawColumn));

            return dataProcessor[rawColumn].IsId;
        }

        public bool IsCommentRow(int rawRow)
        {
            Guard.Verify<LogicException>(rawRow < 0 || rawRow >= RawRowCount,
                string.Format("Raw row '{0}' is out of range.", rawRow));

            return GetValue(rawRow, 0).StartsWith(CommentLineSeparator);
        }

        public bool IsCommentColumn(int rawColumn)
        {
            Guard.Verify<LogicException>(rawColumn < 0 || rawColumn >= RawColumnCount,
                string.Format("Raw column '{0}' is out of range.", rawColumn));

            return string.IsNullOrEmpty(GetName(rawColumn)) || dataProcessor[rawColumn].IsComment;
        }

        public string GetId(int rawRow)
        {
            return excelWorksheet.GetValueEx<string>(rawRow, idColumn);
        }

        public string GetIdComment(int rawRow)
        {
            return excelWorksheet.GetValueEx<string>(rawRow, idCommentColumn);
        }

        public string GetName(int rawColumn)
        {
            Guard.Verify<LogicException>(rawColumn < 0 || rawColumn >= RawColumnCount, string.Format("Raw column '{0}' is out of range.", rawColumn));

            if (IsIdColumn(rawColumn))
            {
                return "Id";
            }

            return excelWorksheet.GetValueEx<string>(nameRow, rawColumn);
        }

        public bool IsSystem(int rawColumn)
        {
            Guard.Verify<LogicException>(rawColumn < 0 || rawColumn >= RawColumnCount,
               string.Format("Raw column '{0}' is out of range.", rawColumn));

            return dataProcessor[rawColumn].IsSystem;
        }

        public Type GetType(int rawColumn)
        {
            Guard.Verify<LogicException>(rawColumn < 0 || rawColumn >= RawColumnCount,
                string.Format("Raw column '{0}' is out of range.", rawColumn));

            return dataProcessor[rawColumn].Type;
        }

        public string GetLanguageKeyword(int rawColumn)
        {
            Guard.Verify<LogicException>(rawColumn < 0 || rawColumn >= RawColumnCount,
               string.Format("Raw column '{0}' is out of range.", rawColumn));

            return dataProcessor[rawColumn].LanguageKeyword;
        }

        public string GetDefaultValue(int rawColumn)
        {
            Guard.Verify<LogicException>(rawColumn < 0 || rawColumn >= RawColumnCount,
               string.Format("Raw column '{0}' is out of range.", rawColumn));

            return defaultValueRow != -1 ? excelWorksheet.GetValueEx<string>(defaultValueRow, rawColumn) : null;
        }

        public string GetComment(int rawColumn)
        {
            Guard.Verify<LogicException>(rawColumn < 0 || rawColumn >= RawColumnCount,
               string.Format("Raw column '{0}' is out of range.", rawColumn));

            return commentRow != -1 ? excelWorksheet.GetValueEx<string>(commentRow, rawColumn) : null;
        }

        public string GetValue(int rawRow, int rawColumn)
        {

            Guard.Verify<LogicException>(rawRow < 0 || rawRow >= RawRowCount,
               string.Format("Raw row '{0}' is out of range.", rawRow));

            Guard.Verify<LogicException>(rawColumn < 0 || rawColumn >= RawColumnCount,
               string.Format("Raw column '{0}' is out of range.", rawColumn));

            return excelWorksheet.GetValueEx<string>(rawRow, rawColumn);
        }

        /// <summary>
        /// 生成数据文件
        /// </summary>
        /// <param name="outputFileName"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public bool GenerateDataFile(string outputFileName, Encoding encoding)
        {
            Guard.NotEmptyOrNull(outputFileName, "Output file name is invalid.");

            try
            {
                using (FileStream fileStream = new FileStream(outputFileName, FileMode.Create))
                {
                    using (BinaryWriter stream = new BinaryWriter(fileStream, encoding))
                    {
                        for (int i = ContentStartRow; i < RawRowCount; i++)
                        {
                            if (IsCommentRow(i))
                            {
                                continue;
                            }

                            int startPosition = (int)stream.BaseStream.Position;
                            stream.BaseStream.Position += sizeof(int);
                            for (int j = 0; j < RawColumnCount; j++)
                            {
                                if (IsCommentColumn(j))
                                {
                                    continue;
                                }

                                try
                                {
                                    dataProcessor[j].WriteToStream(stream, GetValue(i, j));
                                }
                                catch
                                {
                                    if (dataProcessor[j].IsId || string.IsNullOrEmpty(GetDefaultValue(j)))
                                    {
                                        Debug.LogError(string.Format("Parse raw value failure. OutputFileName='{0}' RawRow='{1}' RowColumn='{2}' Name='{3}' Type='{4}' RawValue='{5}'", outputFileName, i, j, GetName(j), GetLanguageKeyword(j), GetValue(i, j)));
                                        return false;
                                    }
                                    else
                                    {
                                        Debug.LogWarning(string.Format("Parse raw value failure, will try default value. OutputFileName='{0}' RawRow='{1}' RowColumn='{2}' Name='{3}' Type='{4}' RawValue='{5}'", outputFileName, i, j, GetName(j), GetLanguageKeyword(j), GetValue(i, j)));
                                        try
                                        {
                                            dataProcessor[j].WriteToStream(stream, GetDefaultValue(j));
                                        }
                                        catch
                                        {
                                            Debug.LogError(string.Format("Parse default value failure. OutputFileName='{0}' RawRow='{1}' RowColumn='{2}' Name='{3}' Type='{4}' RawValue='{5}'", outputFileName, i, j, GetName(j), GetLanguageKeyword(j), GetComment(j)));
                                            return false;
                                        }
                                    }
                                }
                            }

                            int endPosition = (int)stream.BaseStream.Position;
                            int length = endPosition - startPosition - sizeof(int);
                            stream.BaseStream.Position = startPosition;
                            stream.Write(length);
                            stream.BaseStream.Position = endPosition;
                        }
                    }
                }

                Debug.Log(string.Format("Parse data table '{0}' success.", outputFileName));
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(string.Format("Parse data table '{0}' failure, exception is '{1}'.", outputFileName, exception.Message));
                return false;
            }
        }

        /// <summary>
        /// 设置代码模板
        /// </summary>
        /// <param name="codeTemplateFileName"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public bool SetCodeTemplate(string codeTemplateFileName, Encoding encoding)
        {
            try
            {
                codeTemplate = File.ReadAllText(codeTemplateFileName, encoding);
                Debug.Log(string.Format("Set code template '{0}' success.", codeTemplateFileName));
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(string.Format("Set code template '{0}' failure, exception is '{1}'.", codeTemplateFileName, exception.Message));
                return false;
            }
        }

        /// <summary>
        /// 设置代码生成器
        /// </summary>
        public void SetCodeGenerator(DataTableCodeGenerator codeGenerator)
        {
            this.codeGenerator = codeGenerator;
        }

        /// <summary>
        /// 生成代码文件
        /// </summary>
        /// <param name="outputFileName"></param>
        /// <param name="encoding"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        public bool GenerateCodeFile(string outputFileName, Encoding encoding, object userData = null)
        {
            Guard.NotEmptyOrNull(codeTemplate, "You must set code template first.");
            Guard.NotEmptyOrNull(outputFileName, "Output file name is invalid.");

            try
            {
                StringBuilder stringBuilder = new StringBuilder(codeTemplate);
                if (codeGenerator != null)
                {
                    codeGenerator(this, stringBuilder, userData);
                }

                using (FileStream fileStream = new FileStream(outputFileName, FileMode.Create))
                {
                    using (StreamWriter stream = new StreamWriter(fileStream, encoding))
                    {
                        stream.Write(stringBuilder);
                    }
                }

                Debug.Log(string.Format("Generate code file '{0}' success.", outputFileName));
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(string.Format("Generate code file '{0}' failure, exception is '{1}'.", outputFileName, exception.Message));
                return false;
            }
        }
    }
}