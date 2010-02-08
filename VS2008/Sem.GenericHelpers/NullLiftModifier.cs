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

        /// <summary>
        /// Change how '.' performs member access.
        /// </summary>
        /// <param name="originalExpression"></param>
        /// <returns></returns>
        protected override Expression VisitMemberAccess(MemberExpression originalExpression)
        {
            MemberExpression memberAccessExpression = (MemberExpression)base.VisitMemberAccess(originalExpression);

            Expression nullTest = Expression.Equal(
                memberAccessExpression.Expression, 
                Expression.Constant(null, memberAccessExpression.Expression.Type));

            return Expression.Condition(nullTest, Expression.Constant(null, memberAccessExpression.Type), memberAccessExpression);
        }
    }
}
