/*
*    描述:
*          1. 
*
*    开发人: 邓平
*/
using System;
using System.Linq.Expressions;

namespace LtFramework.Util.Tools
{


    public static class UIEventTools
    {

        public static void OnButton()
        {

        }
    }

    public static class MemberInfoGetting
    {
        public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression) memberExpression.Body;
            return expressionBody.Member.Name;
        }

    }
}