// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullLiftModifier.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The method that kicks off the tree walk is protected, so we need to
//   define a public method that provides access to it.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers
{
    using System.Linq.Expressions;

    /// <summary>
    /// The method that kicks off the tree walk is protected, so we need to 
    ///   define a public method that provides access to it.
    /// </summary>
    public class NullLiftModifier : ExpressionVisitor
    {
        /// <summary>
        /// The modify.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <returns>
        /// </returns>
        public Expression Modify(Expression expression)
        {
            return this.Visit(expression);
        }

        /// <summary>
        /// Change how '.' performs member access.
        /// </summary>
        /// <param name="originalExpression">
        /// </param>
        /// <returns>
        /// </returns>
        protected override Expression VisitMemberAccess(MemberExpression originalExpression)
        {
            var memberAccessExpression = (MemberExpression)base.VisitMemberAccess(originalExpression);

            if (memberAccessExpression.Expression == null)
            {
                return memberAccessExpression;
            }

            var valueType = memberAccessExpression.Expression.Type;
            var memberType = memberAccessExpression.Type;

            var valueNull = valueType.GetDefaultValue();
            var memberNull = memberType.GetDefaultValue();

            var nullTest = Expression.Equal(
                memberAccessExpression.Expression, Expression.Constant(valueNull, valueType));

            return Expression.Condition(nullTest, Expression.Constant(memberNull, memberType), memberAccessExpression);
        }

        /// <summary>
        /// The visit method call.
        /// </summary>
        /// <param name="originalExpression">
        /// The original expression.
        /// </param>
        /// <returns>
        /// </returns>
        protected override Expression VisitMethodCall(MethodCallExpression originalExpression)
        {
            var invocationExpression = (MethodCallExpression)base.VisitMethodCall(originalExpression);
            var argument = invocationExpression.Object;

            if (argument == null)
            {
                return invocationExpression;
            }

            Expression nullTest = Expression.Equal(argument, Expression.Constant(null, argument.Type));

            return Expression.Condition(
                nullTest, Expression.Constant(null, invocationExpression.Method.ReturnType), invocationExpression);
        }
    }
}