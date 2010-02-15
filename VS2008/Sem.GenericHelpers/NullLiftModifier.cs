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

            if (memberAccessExpression.Type == typeof(System.DateTime)
                || memberAccessExpression.Type.BaseType == typeof(System.Enum))
            {
                return memberAccessExpression;
            }

            var nullTest = Expression.Equal(
                memberAccessExpression.Expression, 
                Expression.Constant(null, memberAccessExpression.Expression.Type));

            return Expression.Condition(nullTest, Expression.Constant(null, memberAccessExpression.Type), memberAccessExpression);
        }
    }
}
