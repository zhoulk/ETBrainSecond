/*
*    描述:
*          1. 专门负责对于JSon 由于路径错误，或者Json 格式错误造成的异常，进行捕获。
*
*    开发人: 邓平
*/
using System;

namespace LtFramework.Util
{
    public class JsonAnlysisException : Exception
    {
        public JsonAnlysisException() : base()
        {
        }

        public JsonAnlysisException(string exceptionMessage) : base(exceptionMessage)
        {
        }
    }
}