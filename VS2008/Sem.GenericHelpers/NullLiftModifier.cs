// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullLiftModifier.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the NullLiftModifier type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// The method that kicks off the tree walk is protected, so we need to 
    /// define a public method that provides access to it.
    /// </summary>
    public class NullLiftModifier : ExpressionVisitor
    {
        public Expression Modify(Expression expression)
        {
            return Visit(expression);
        }

        protected override Expression VisitMethodCall(MethodCallExpression originalExpression)
        {
            var invocationExpression = (MethodCallExpression)base.VisitMethodCall(originalExpression);
            var argument = invocationExpression.Object;

            if (argument == null)
            {
                return invocationExpression;
            }
            
            Expression nullTest = Expression.Equal(
                argument,
                Expression.Constant(null, argument.Type));

            return Expression.Condition(nullTest, Expression.Constant(null, invocationExpression.Method.ReturnType), invocationExpression);
        }

        /// <summary>
        /// Change how '.' performs member access.
        /// </summary>
        /// <param name="originalExpression"></param>
        /// <returns></returns>
        protected override Expression VisitMemberAccess(MemberExpression originalExpression)
        {
            var memberAccessExpression = (MemberExpression)base.VisitMemberAccess(originalExpression);

            if (memberAccessExpression.Expression == null)
            {
                return memberAccessExpression;
            }

            var valueType = memberAccessExpression.Expression.Type;
            var memberType = memberAccessExpression.Type;

            var valueNull = GetNullMember(valueType);
            var memberNull = GetNullMember(memberType);

            var nullTest = Expression.Equal(
                memberAccessExpression.Expression,
                Expression.Constant(valueNull, valueType));

            return Expression.Condition(
                nullTest, 
                Expression.Constant(memberNull, memberType), 
                memberAccessExpression);
        }

        private static object GetNullMember(Type memberType)
        {
            if (memberType.IsValueType)
            {
                if (memberType.Name == "DateTime")
                {
                    return new System.DateTime();
                }

                if (memberType.BaseType.Name == "Enum")
                {
                    return Enum.GetValues(memberType).GetValue(0);
                }
            }

            return null;
        }
    }
}
