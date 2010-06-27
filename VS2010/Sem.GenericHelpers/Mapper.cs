// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Mapper.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the Mapper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Implements functionality to map properties from one object to another using 
    /// Linq-expressions reading the value and an <see cref="Action"/> to write the
    /// property.
    /// </summary>
    /// <example>
    /// <code language="c#">
    /// // register the property mapping of a source and a destination 
    /// // type with trasformations (in this case we do add a string to
    /// // the name to generate a calendar title of "Birthday: Riddle, Tom")
    /// var mapper = new Mapper();
    /// mapper.Register&lt;StdContact, StdCalendarItem>(contact => contact.Id, (calendar, entity) => calendar.Id = (Guid)entity);
    /// mapper.Register&lt;StdContact, StdCalendarItem>(contact => contact.GetFullName(), (calendar, entity) => calendar.Title = "Birthday: " + entity);
    /// 
    /// // setup a source and a destination object
    /// var source = new StdContact { Name = new PersonName("Tom Riddle") };
    /// var target = new StdCalendarItem();
    /// 
    /// // perform the mapping
    /// mapper.Map(source, target);
    /// 
    /// // test it
    /// Assert.AreEqual(source.Id, target.Id);
    /// Assert.AreEqual("Birthday: " + source.GetFullName(), target.Title);
    /// </code>
    /// </example>
    public class Mapper
    {
        /// <summary>
        /// Global default instance of the mapper. Use <see cref="RegisterGlobal{TSource,TTarget}"/> to
        /// add new mappings to the global mapper that will be respected in all <see cref="Map{TSource,TTarget}"/>
        /// calls (even in the instance versions). See <see cref="Mapper"/> for an example.
        /// </summary>
        private static readonly Mapper DefaultInstance = new Mapper();
        
        /// <summary>
        /// Mapping table for the instance. Use <see cref="Register{TSource,TTarget}"/> to add new mappings 
        /// to the mapper that will be used in <see cref="Map{TSource,TTarget}"/> calls of this instance.
        /// Use <see cref="RegisterGlobal{TSource,TTarget}"/> to add new mappings to the global mapper that 
        /// will be respected in all <see cref="Map{TSource,TTarget}"/> calls (even in the instance versions).
        /// </summary>
        private readonly Dictionary<Tuple<Type, Type>, List<Tuple<object, object>>> mappings = new Dictionary<Tuple<Type, Type>, List<Tuple<object, object>>>();

        /// <summary>
        /// Register a mapping in the global instance. Mappings that are registered globally
        /// will be used in <see cref="MapGlobal{TSource,TTarget}"/> and in <see cref="Map{TSource,TTarget}"/>,
        /// where <see cref="Map{TSource,TTarget}"/> will execute all global mappings first and then the 
        /// private mappings.
        /// </summary>
        /// <param name="getEntity"> The get expression. </param>
        /// <param name="setEntity"> The set method. </param>
        /// <typeparam name="TSource">The source object type</typeparam>
        /// <typeparam name="TTarget">The target object type</typeparam>
        public static void RegisterGlobal<TSource, TTarget>(
            Expression<Func<TSource, object>> getEntity,
            Action<TTarget, object> setEntity)
        {
            DefaultInstance.Register(getEntity, setEntity);
        }

        /// <summary>
        /// Performs the registered mapping code for this entity type (multiple 
        /// mappings can be specified for a source/target pair). This static version 
        /// will only execute the global mappings.
        /// </summary>
        /// <param name="source"> The source object instance. </param>
        /// <param name="target"> The target object instance. </param>
        /// <typeparam name="TSource">The source object type</typeparam>
        /// <typeparam name="TTarget">The target object type</typeparam>
        /// <returns>A value indicating if a mapping has been executed.</returns>
        public static bool MapGlobal<TSource, TTarget>(TSource source, TTarget target)
        {
            return DefaultInstance.Map(source, target);
        }

        /// <summary>
        /// Register a mapping in the private instance.
        /// </summary>
        /// <param name="getEntity"> The get expression. </param>
        /// <param name="setEntity"> The set method. </param>
        /// <typeparam name="TSource">The source object type</typeparam>
        /// <typeparam name="TTarget">The target object type</typeparam>
        public void Register<TSource, TTarget>(
            Expression<Func<TSource, object>> getEntity,
            Action<TTarget, object> setEntity)
        {
            var typeTupel = new Tuple<Type, Type>(typeof(TSource), typeof(TTarget));

            if (!this.mappings.ContainsKey(typeTupel))
            {
                this.mappings.Add(typeTupel, new List<Tuple<object, object>>());
            }

            var mapping = this.mappings[typeTupel];
            var modifier = new NullLiftModifier();
            var getEntityFunction = ((Expression<Func<TSource, object>>)modifier.Modify(getEntity)).Compile();
            mapping.Add(new Tuple<object, object>(getEntityFunction, setEntity));
        }

        /// <summary>
        /// Performs the registered mapping code for this entity type (multiple 
        /// mappings can be specified for a source/target pair).
        /// </summary>
        /// <param name="source"> The source object instance. </param>
        /// <param name="target"> The target object instance. </param>
        /// <typeparam name="TSource">The source object type</typeparam>
        /// <typeparam name="TTarget">The target object type</typeparam>
        /// <returns>A value indicating if a mapping has been executed.</returns>
        public bool Map<TSource, TTarget>(TSource source, TTarget target)
        {
            var mapResult = 
                this != DefaultInstance 
                ? DefaultInstance.Map(source, target)
                : false;

            var typeTupel = new Tuple<Type, Type>(typeof(TSource), typeof(TTarget));
            if (!this.mappings.ContainsKey(typeTupel))
            {
                return mapResult;
            }

            var mapping = this.mappings[typeTupel];
            foreach (var tuple in mapping)
            {
                var get = (Func<TSource, object>)tuple.Item1;
                var set = (Action<TTarget, object>)tuple.Item2;

                set.Invoke(target, get.Invoke(source));
                mapResult = true;
            }

            return mapResult;
        }
    }
}
