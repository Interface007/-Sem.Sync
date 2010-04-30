// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionVisitor.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ExpressionVisitor type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq.Expressions;

    public abstract class ExpressionVisitor
    {
        protected virtual Expression Visit(Expression originalExpression)
        {
            if (originalExpression == null)
            {
                return originalExpression;
            }

            switch (originalExpression.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                    return this.VisitUnary((UnaryExpression)originalExpression);

                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                    return this.VisitBinary((BinaryExpression)originalExpression);

                case ExpressionType.TypeIs:
                    return this.VisitTypeIs((TypeBinaryExpression)originalExpression);

                case ExpressionType.Conditional:
                    return this.VisitConditional((ConditionalExpression)originalExpression);

                case ExpressionType.Constant:
                    return this.VisitConstant((ConstantExpression)originalExpression);

                case ExpressionType.Parameter:
                    return this.VisitParameter((ParameterExpression)originalExpression);

                case ExpressionType.MemberAccess:
                    return this.VisitMemberAccess((MemberExpression)originalExpression);

                case ExpressionType.Call:
                    return this.VisitMethodCall((MethodCallExpression)originalExpression);

                case ExpressionType.Lambda:
                    return this.VisitLambda((LambdaExpression)originalExpression);

                case ExpressionType.New:
                    return this.VisitNew((NewExpression)originalExpression);

                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    return this.VisitNewArray((NewArrayExpression)originalExpression);

                case ExpressionType.Invoke:
                    return this.VisitInvocation((InvocationExpression)originalExpression);

                case ExpressionType.MemberInit:
                    return this.VisitMemberInit((MemberInitExpression)originalExpression);

                case ExpressionType.ListInit:
                    return this.VisitListInit((ListInitExpression)originalExpression);

                default:
                    throw new Exception(string.Format(CultureInfo.InvariantCulture, "Unhandled expression type: '{0}'", originalExpression.NodeType));
            }
        }

        protected virtual MemberBinding VisitBinding(MemberBinding originalExpression)
        {
            switch (originalExpression.BindingType)
            {
                case MemberBindingType.Assignment:
                    return this.VisitMemberAssignment((MemberAssignment)originalExpression);

                case MemberBindingType.MemberBinding:
                    return this.VisitMemberMemberBinding((MemberMemberBinding)originalExpression);

                case MemberBindingType.ListBinding:
                    return this.VisitMemberListBinding((MemberListBinding)originalExpression);

                default:
                    throw new Exception(string.Format(CultureInfo.InvariantCulture, "Unhandled binding type '{0}'", originalExpression.BindingType));
            }
        }

        protected virtual ElementInit VisitElementInitializer(ElementInit originalExpression)
        {
            ReadOnlyCollection<Expression> arguments = this.VisitExpressionList(originalExpression.Arguments);
            return arguments != originalExpression.Arguments 
                ? Expression.ElementInit(originalExpression.AddMethod, arguments) 
                : originalExpression;
        }

        protected virtual Expression VisitUnary(UnaryExpression originalExpression)
        {
            Expression operand = this.Visit(originalExpression.Operand);
            return operand != originalExpression.Operand 
                ? Expression.MakeUnary(originalExpression.NodeType, operand, originalExpression.Type, originalExpression.Method) 
                : originalExpression;
        }

        protected virtual Expression VisitBinary(BinaryExpression originalExpression)
        {
            Expression left = this.Visit(originalExpression.Left);
            Expression right = this.Visit(originalExpression.Right);
            Expression conversion = this.Visit(originalExpression.Conversion);
            if (left != originalExpression.Left || right != originalExpression.Right || conversion != originalExpression.Conversion)
            {
                return originalExpression.NodeType == ExpressionType.Coalesce && originalExpression.Conversion != null
                           ? Expression.Coalesce(left, right, conversion as LambdaExpression)
                           : Expression.MakeBinary(originalExpression.NodeType, left, right, originalExpression.IsLiftedToNull, originalExpression.Method);
            }

            return originalExpression;
        }

        protected virtual Expression VisitTypeIs(TypeBinaryExpression originalExpression)
        {
            Expression expr = this.Visit(originalExpression.Expression);
            return expr != originalExpression.Expression 
                ? Expression.TypeIs(expr, originalExpression.TypeOperand) 
                : originalExpression;
        }

        protected virtual Expression VisitConstant(ConstantExpression originalExpression)
        {
            return originalExpression;
        }

        protected virtual Expression VisitConditional(ConditionalExpression originalExpression)
        {
            Expression test = this.Visit(originalExpression.Test);
            Expression ifTrue = this.Visit(originalExpression.IfTrue);
            Expression ifFalse = this.Visit(originalExpression.IfFalse);
            if (test != originalExpression.Test || ifTrue != originalExpression.IfTrue || ifFalse != originalExpression.IfFalse)
            {
                return Expression.Condition(test, ifTrue, ifFalse);
            }

            return originalExpression;
        }

        protected virtual Expression VisitParameter(ParameterExpression originalExpression)
        {
            return originalExpression;
        }

        protected virtual Expression VisitMemberAccess(MemberExpression originalExpression)
        {
            Expression exp = this.Visit(originalExpression.Expression);
            return exp != originalExpression.Expression 
                ? Expression.MakeMemberAccess(exp, originalExpression.Member) 
                : originalExpression;
        }

        protected virtual Expression VisitMethodCall(MethodCallExpression originalExpression)
        {
            Expression obj = this.Visit(originalExpression.Object);
            IEnumerable<Expression> args = this.VisitExpressionList(originalExpression.Arguments);
            if (obj != originalExpression.Object || args != originalExpression.Arguments)
            {
                return Expression.Call(obj, originalExpression.Method, args);
            }

            return originalExpression;
        }

        protected virtual ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> originalExpression)
        {
            List<Expression> list = null;
            for (int i = 0, n = originalExpression.Count; i < n; i++)
            {
                Expression p = this.Visit(originalExpression[i]);
                if (list != null)
                {
                    list.Add(p);
                }
                else if (p != originalExpression[i])
                {
                    list = new List<Expression>(n);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(originalExpression[j]);
                    }

                    list.Add(p);
                }
            }

            return list != null ? list.AsReadOnly() : originalExpression;
        }

        protected virtual MemberAssignment VisitMemberAssignment(MemberAssignment originalExpression)
        {
            Expression e = this.Visit(originalExpression.Expression);
            return e != originalExpression.Expression ? Expression.Bind(originalExpression.Member, e) : originalExpression;
        }

        protected virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding originalExpression)
        {
            IEnumerable<MemberBinding> bindings = this.VisitBindingList(originalExpression.Bindings);
            return bindings != originalExpression.Bindings ? Expression.MemberBind(originalExpression.Member, bindings) : originalExpression;
        }

        protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding originalExpression)
        {
            IEnumerable<ElementInit> initializers = this.VisitElementInitializerList(originalExpression.Initializers);
            return initializers != originalExpression.Initializers ? Expression.ListBind(originalExpression.Member, initializers) : originalExpression;
        }

        protected virtual IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> originalExpression)
        {
            List<MemberBinding> list = null;
            for (int i = 0, n = originalExpression.Count; i < n; i++)
            {
                MemberBinding b = this.VisitBinding(originalExpression[i]);
                if (list != null)
                {
                    list.Add(b);
                }
                else if (b != originalExpression[i])
                {
                    list = new List<MemberBinding>(n);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(originalExpression[j]);
                    }

                    list.Add(b);
                }
            }

            if (list != null)
            {
                return list;
            }

            return originalExpression;
        }

        protected virtual IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> originalExpression)
        {
            List<ElementInit> list = null;
            for (int i = 0, n = originalExpression.Count; i < n; i++)
            {
                ElementInit init = this.VisitElementInitializer(originalExpression[i]);
                if (list != null)
                {
                    list.Add(init);
                }
                else if (init != originalExpression[i])
                {
                    list = new List<ElementInit>(n);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(originalExpression[j]);
                    }

                    list.Add(init);
                }
            }

            if (list != null)
            {
                return list;
            }

            return originalExpression;
        }

        protected virtual Expression VisitLambda(LambdaExpression originalExpression)
        {
            Expression body = this.Visit(originalExpression.Body);
            return body != originalExpression.Body ? Expression.Lambda(originalExpression.Type, body, originalExpression.Parameters) : originalExpression;
        }

        protected virtual NewExpression VisitNew(NewExpression originalExpression)
        {
            IEnumerable<Expression> args = this.VisitExpressionList(originalExpression.Arguments);
            if (args != originalExpression.Arguments)
            {
                return originalExpression.Members != null ? Expression.New(originalExpression.Constructor, args, originalExpression.Members) : Expression.New(originalExpression.Constructor, args);
            }

            return originalExpression;
        }

        protected virtual Expression VisitMemberInit(MemberInitExpression originalExpression)
        {
            NewExpression n = this.VisitNew(originalExpression.NewExpression);
            IEnumerable<MemberBinding> bindings = this.VisitBindingList(originalExpression.Bindings);
            if (n != originalExpression.NewExpression || bindings != originalExpression.Bindings)
            {
                return Expression.MemberInit(n, bindings);
            }

            return originalExpression;
        }

        protected virtual Expression VisitListInit(ListInitExpression originalExpression)
        {
            NewExpression n = this.VisitNew(originalExpression.NewExpression);
            IEnumerable<ElementInit> initializers = this.VisitElementInitializerList(originalExpression.Initializers);
            if (n != originalExpression.NewExpression || initializers != originalExpression.Initializers)
            {
                return Expression.ListInit(n, initializers);
            }
            return originalExpression;
        }

        protected virtual Expression VisitNewArray(NewArrayExpression originalExpression)
        {
            IEnumerable<Expression> exprs = this.VisitExpressionList(originalExpression.Expressions);
            if (exprs != originalExpression.Expressions)
            {
                return originalExpression.NodeType == ExpressionType.NewArrayInit 
                    ? Expression.NewArrayInit(originalExpression.Type.GetElementType(), exprs) 
                    : Expression.NewArrayBounds(originalExpression.Type.GetElementType(), exprs);
            }

            return originalExpression;
        }

        protected virtual Expression VisitInvocation(InvocationExpression originalExpression)
        {
            IEnumerable<Expression> args = this.VisitExpressionList(originalExpression.Arguments);
            Expression expr = this.Visit(originalExpression.Expression);
            if (args != originalExpression.Arguments || expr != originalExpression.Expression)
            {
                return Expression.Invoke(expr, args);
            }

            return originalExpression;
        }
    }
}
