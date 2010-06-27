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

    public class Mapper
    {
        private Dictionary<Tuple<Type, Type>, List<Tuple<object, object>>> mappings;

        public void Register<TSource, TTarget>(
            Func<TSource, object> getEntity,
            Action<TTarget, object> setEntity)
        {
            var typeTupel = new Tuple<Type, Type>(typeof(TSource), typeof(TTarget));

            if (this.mappings == null)
            {
                this.mappings = new Dictionary<Tuple<Type, Type>, List<Tuple<object, object>>>();
            }

            if (!this.mappings.ContainsKey(typeTupel))
            {
                this.mappings.Add(typeTupel, new List<Tuple<object, object>>());
            }

            var mapping = this.mappings[typeTupel];
            mapping.Add(new Tuple<object, object>(getEntity, setEntity));
        }

        public bool Map<TSource, TTarget>(TSource source, TTarget target)
        {
            var typeTupel = new Tuple<Type, Type>(typeof(TSource), typeof(TTarget));
            var mapping = this.mappings[typeTupel];
            foreach (var tuple in mapping)
            {
                var get = (Func<TSource, object>)tuple.Item1;
                var set = (Action<TTarget, object>)tuple.Item2;

                set.Invoke(target, get.Invoke(source));
            }

            return true;
        }
    }
}
