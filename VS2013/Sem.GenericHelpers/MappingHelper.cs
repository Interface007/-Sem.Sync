// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MappingHelper.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Provides helper extensions for mapping data from one entity to another.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Provides helper extensions for mapping data from one entity to another.
    /// </summary>
    public static class MappingHelper
    {
        /// <summary>
        ///   Dictionary for the modified and compiled expression trees
        /// </summary>
        private static readonly Dictionary<string, Delegate> Expressions = new Dictionary<string, Delegate>();

        /// <summary>
        ///   Expression modifier instance
        /// </summary>
        private static readonly NullLiftModifier Modifier = new NullLiftModifier();

        /// <summary>
        /// compares old to new and sets the destination if both are different.
        /// </summary>
        /// <param name="dirty"> The dirty-flag (set to true if a modification in the destination object has been done).  </param>
        /// <param name="newSource"> The new std element.  </param>
        /// <param name="oldSource"> The old std element.  </param>
        /// <param name="valueExtractionExpression"> The expression to extract the value from the source type.  </param>
        /// <param name="setter"> The setter method for the destination object.  </param>
        /// <typeparam name="TDestination"> The type of the destination property.  </typeparam>
        /// <typeparam name="TSource"> The type of the source object.  </typeparam>
        public static void MapIfDiffers<TDestination, TSource>(
            ref bool dirty,
            TSource newSource,
            TSource oldSource,
            Expression<Func<TSource, TDestination>> valueExtractionExpression,
            Action<TDestination> setter)
        {
            var modifier = new NullLiftModifier();
            var function =
                ((Expression<Func<TSource, TDestination>>)modifier.Modify(valueExtractionExpression)).Compile();
            var newValue = function(newSource);
            if (Equals(function(oldSource), newValue))
            {
                return;
            }

            setter(newValue);
            dirty = true;
        }

        /// <summary>
        /// Trys to evaluate a series of 3 functions and assignes the result of the last function to
        ///   the target <paramref name="target"/>. The evaluation does stop if any result of an evaluation
        ///   is null.
        /// </summary>
        /// <example>
        /// The following code shows how to use the method:
        ///   <code language="c#">
        /// // source.myProp2 is null, so source.myProp2.Password cannot be evaluated and x should stay the same
        ///     target.Passwort = "hallo";
        ///     source.MapIfExist(y =&gt; y.x, y =&gt; y.myProp2, y =&gt; y.Password, ref target.Passwort);
        ///     Assert.IsTrue(target.Passwort == "hallo");
        ///     // source.myProp1 is "geheim1", so source.myProp1.Password can be evaluated and x should be updated
        ///     target.Passwort = "hallo";
        ///     source.MapIfExist(y =&gt; y.x, y =&gt; y.myProp1, y =&gt; y.Password, ref target.Passwort);
        ///     Assert.IsTrue(target.Passwort == "geheim1");
        ///   </code>
        /// </example>
        /// <param name="obj"> The root object.  </param>
        /// <param name="evaluationFunction1"> The function for the 1st evaluation.  </param>
        /// <param name="evaluationFunction2"> The function for the 2nd evaluation.  </param>
        /// <param name="evaluationFunction3"> The function for the 3rd evaluation  </param>
        /// <param name="target"> The target of the mapping.  </param>
        /// <typeparam name="TResult1"> The result type of the 1st evaluation.  </typeparam>
        /// <typeparam name="TResult2"> The result type of the 2nd evaluation.  </typeparam>
        /// <typeparam name="TResult3"> The result type of the 3rd evaluation.  </typeparam>
        /// <typeparam name="TObject"> The type of the root object  </typeparam>
        public static void MapIfExist<TResult1, TResult2, TResult3, TObject>(
            this TObject obj,
            Func<TObject, TResult1> evaluationFunction1,
            Func<TResult1, TResult2> evaluationFunction2,
            Func<TResult2, TResult3> evaluationFunction3,
            ref TResult3 target) where TResult1 : class where TResult2 : class
        {
            var result1 = !Equals(obj, default(TObject)) ? evaluationFunction1(obj) : null;
            if (result1 == default(TResult1))
            {
                return;
            }

            result1.MapIfExist(evaluationFunction2, evaluationFunction3, ref target);
        }

        /// <summary>
        /// Trys to evaluate a series of 2 functions and assignes the result of the last function to
        ///   the target <paramref name="target"/>. The evaluation does stop if any result of an evaluation
        ///   is null.
        /// </summary>
        /// <example>
        /// The following code shows how to use the method:
        ///   <code language="c#">
        /// // source.myProp2 is null, so source.myProp2.Password cannot be evaluated and x should stay the same
        ///     target.Passwort = "hallo";
        ///     source.MapIfExist(y =&gt; y.myProp2, y =&gt; y.Password, ref target.Passwort);
        ///     Assert.IsTrue(target.Passwort == "hallo");
        ///     // source.myProp1 is "geheim1", so source.myProp1.Password can be evaluated and x should be updated
        ///     target.Passwort = "hallo";
        ///     source.MapIfExist(y =&gt; y.myProp1, y =&gt; y.Password, ref target.Passwort);
        ///     Assert.IsTrue(target.Passwort == "geheim1");
        ///   </code>
        /// </example>
        /// <param name="obj"> The root object.  </param>
        /// <param name="evaluationFunction1"> The function for the 1st evaluation.  </param>
        /// <param name="evaluationFunction2"> The function for the 2nd evaluation.  </param>
        /// <param name="target"> The target of the mapping.  </param>
        /// <typeparam name="TResult1"> The result type of the 1st evaluation.  </typeparam>
        /// <typeparam name="TResult2"> The result type of the 2nd evaluation.  </typeparam>
        /// <typeparam name="TObject"> The type of the root object  </typeparam>
        public static void MapIfExist<TResult1, TResult2, TObject>(
            this TObject obj,
            Func<TObject, TResult1> evaluationFunction1,
            Func<TResult1, TResult2> evaluationFunction2,
            ref TResult2 target) where TResult1 : class
        {
            var result1 = !Equals(obj, default(TObject)) ? evaluationFunction1(obj) : null;
            if (result1 == default(TResult1))
            {
                return;
            }

            result1.MapIfExist(evaluationFunction2, ref target);
        }

        /// <summary>
        /// Trys to evaluate a one function and assignes the result to
        ///   the target <paramref name="target"/>. The evaluation does stop if the result of the evaluation
        ///   is null.
        /// </summary>
        /// <example>
        /// The following code shows how to use the method:
        ///   <code language="c#">
        /// // source.myProp2 is null, so source.myProp2.Password cannot be evaluated and x should stay the same
        ///     target.Passwort = "hallo";
        ///     source.myProp2.MapIfExist(y =&gt; y.Password, ref target.Passwort);
        ///     Assert.IsTrue(target.Passwort == "hallo");
        ///     // source.myProp1 is "geheim1", so source.myProp1.Password can be evaluated and x should be updated
        ///     target.Passwort = "hallo";
        ///     source.myProp1.MapIfExist(y =&gt; y.Password, ref target.Passwort);
        ///     Assert.IsTrue(target.Passwort == "geheim1");
        ///   </code>
        /// </example>
        /// <param name="obj"> The root object.  </param>
        /// <param name="evaluationFunction1"> The function for the 1st evaluation.  </param>
        /// <param name="target"> The target of the mapping.  </param>
        /// <typeparam name="TResult1"> The result type of the 1st evaluation.  </typeparam>
        /// <typeparam name="TObject"> The type of the root object  </typeparam>
        public static void MapIfExist<TResult1, TObject>(
            this TObject obj, Func<TObject, TResult1> evaluationFunction1, ref TResult1 target)
        {
            if (Equals(obj, default(TObject)))
            {
                return;
            }

            target = evaluationFunction1(obj);
        }

        /// <summary>
        /// Maps one property to another objects property using a full property path.
        /// </summary>
        /// <param name="obj"> The root object. </param>
        /// <param name="evaluationFunction1"> The expression that does describe the path to the property.  </param>
        /// <param name="target"> The target property to write the result of the expression.  </param>
        /// <typeparam name="TResult1">The type of the expressions result  </typeparam>
        /// <typeparam name="TObject"> The type of the root object  </typeparam>
        public static void MapIfExist2<TResult1, TObject>(
            this TObject obj, Expression<Func<TObject, TResult1>> evaluationFunction1, ref TResult1 target)
        {
            var method = GetMethod(evaluationFunction1);
            var result = method(obj);

            if (Equals(result, default(TObject)))
            {
                return;
            }

            target = result;
        }

        /// <summary>
        /// Creates and caches a function that is euqivalent to the expression <paramref name="expressionToGetTheFunctionFor"/>.
        ///   The expression is modified to return NULL instead of throwing ReferenceNullExceptions if a part of the expression is null.
        /// </summary>
        /// <param name="expressionToGetTheFunctionFor"> The expression that should be translated into a compiled function  </param>
        /// <typeparam name="TObject"> The type of the object that is the parameter for the function  </typeparam>
        /// <typeparam name="TResult1"> The type of the result of the function  </typeparam>
        /// <returns> The compiled function that is equivalent to the expression  </returns>
        private static Func<TObject, TResult1> GetMethod<TObject, TResult1>(
            Expression<Func<TObject, TResult1>> expressionToGetTheFunctionFor)
        {
            var key = expressionToGetTheFunctionFor + typeof(TObject).FullName;
            if (!Expressions.ContainsKey(key))
            {
                var x = (Expression<Func<TObject, TResult1>>)Modifier.Modify(expressionToGetTheFunctionFor);
                Expressions.Add(key, x.Compile());
            }

            return (Func<TObject, TResult1>)Expressions[key];
        }
    }
}