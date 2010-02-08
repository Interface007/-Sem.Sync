// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MappingHelper.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the MappingHelper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers
{
    using System;

    /// <summary>
    /// Provides helper extensions for mapping data from one entity to another.
    /// </summary>
    public static class MappingHelper
    {
        /// <summary>
        /// Trys to evaluate a series of 3 functions and assignes the result of the last function to
        /// the target <paramref name="target"/>. The evaluation does stop if any result of an evaluation
        /// is null.
        /// </summary>
        /// <example>
        /// The following code shows how to use the method:
        /// <code language="c#">
        /// // source.myProp2 is null, so source.myProp2.Password cannot be evaluated and x should stay the same
        /// target.Passwort = "hallo";
        /// source.MapIfExist(y => y.x, y => y.myProp2, y => y.Password, ref target.Passwort);
        /// Assert.IsTrue(target.Passwort == "hallo");
        /// // source.myProp1 is "geheim1", so source.myProp1.Password can be evaluated and x should be updated
        /// target.Passwort = "hallo";
        /// source.MapIfExist(y => y.x, y => y.myProp1, y => y.Password, ref target.Passwort);
        /// Assert.IsTrue(target.Passwort == "geheim1");
        /// </code>
        /// </example>
        /// <param name="obj"> The root object. </param>
        /// <param name="f1"> The function for the 1st evaluation. </param>
        /// <param name="f2"> The function for the 2nd evaluation. </param>
        /// <param name="f3"> The function for the 3rd evaluation </param>
        /// <param name="target"> The target of the mapping. </param>
        /// <typeparam name="TResult1"> The result type of the 1st evaluation. </typeparam>
        /// <typeparam name="TResult2"> The result type of the 2nd evaluation. </typeparam>
        /// <typeparam name="TResult3"> The result type of the 3rd evaluation. </typeparam>
        /// <typeparam name="TObject"> The type of the root object </typeparam>
        public static void MapIfExist<TResult1, TResult2, TResult3, TObject>(this TObject obj, Func<TObject, TResult1> f1, Func<TResult1, TResult2> f2, Func<TResult2, TResult3> f3, ref TResult3 target)
            where TResult1 : class
            where TResult2 : class
        {
            var result1 = !Equals(obj, default(TObject)) ? f1(obj) : null;
            if (result1 == default(TResult1))
            {
                return;
            }

            result1.MapIfExist(f2, f3, ref target);
        }

        /// <summary>
        /// Trys to evaluate a series of 2 functions and assignes the result of the last function to
        /// the target <paramref name="target"/>. The evaluation does stop if any result of an evaluation
        /// is null.
        /// </summary>
        /// <example>
        /// The following code shows how to use the method:
        /// <code language="c#">
        /// // source.myProp2 is null, so source.myProp2.Password cannot be evaluated and x should stay the same
        /// target.Passwort = "hallo";
        /// source.MapIfExist(y => y.myProp2, y => y.Password, ref target.Passwort);
        /// Assert.IsTrue(target.Passwort == "hallo");
        /// // source.myProp1 is "geheim1", so source.myProp1.Password can be evaluated and x should be updated
        /// target.Passwort = "hallo";
        /// source.MapIfExist(y => y.myProp1, y => y.Password, ref target.Passwort);
        /// Assert.IsTrue(target.Passwort == "geheim1");
        /// </code>
        /// </example>
        /// <param name="obj"> The root object. </param>
        /// <param name="f1"> The function for the 1st evaluation. </param>
        /// <param name="f2"> The function for the 2nd evaluation. </param>
        /// <param name="target"> The target of the mapping. </param>
        /// <typeparam name="TResult1"> The result type of the 1st evaluation. </typeparam>
        /// <typeparam name="TResult2"> The result type of the 2nd evaluation. </typeparam>
        /// <typeparam name="TObject"> The type of the root object </typeparam>
        public static void MapIfExist<TResult1, TResult2, TObject>(this TObject obj, Func<TObject, TResult1> f1, Func<TResult1, TResult2> f2, ref TResult2 target)
            where TResult1 : class
        {
            var result1 = !Equals(obj, default(TObject)) ? f1(obj) : null;
            if (result1 == default(TResult1))
            {
                return;
            }

            result1.MapIfExist(f2, ref target);
        }

        /// <summary>
        /// Trys to evaluate a one function and assignes the result to
        /// the target <paramref name="target"/>. The evaluation does stop if the result of the evaluation
        /// is null.
        /// </summary>
        /// <example>
        /// The following code shows how to use the method:
        /// <code language="c#">
        /// // source.myProp2 is null, so source.myProp2.Password cannot be evaluated and x should stay the same
        /// target.Passwort = "hallo";
        /// source.myProp2.MapIfExist(y => y.Password, ref target.Passwort);
        /// Assert.IsTrue(target.Passwort == "hallo");
        /// // source.myProp1 is "geheim1", so source.myProp1.Password can be evaluated and x should be updated
        /// target.Passwort = "hallo";
        /// source.myProp1.MapIfExist(y => y.Password, ref target.Passwort);
        /// Assert.IsTrue(target.Passwort == "geheim1");
        /// </code>
        /// </example>
        /// <param name="obj"> The root object. </param>
        /// <param name="f1"> The function for the 1st evaluation. </param>
        /// <param name="target"> The target of the mapping. </param>
        /// <typeparam name="TResult1"> The result type of the 1st evaluation. </typeparam>
        /// <typeparam name="TObject"> The type of the root object </typeparam>
        public static void MapIfExist<TResult1, TObject>(this TObject obj, Func<TObject, TResult1> f1, ref TResult1 target)
        {
            if (Equals(obj, default(TObject)))
            {
                return;
            }

            target = f1(obj);
        }
    }
}
