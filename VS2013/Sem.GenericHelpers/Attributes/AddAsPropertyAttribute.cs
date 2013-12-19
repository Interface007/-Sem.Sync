// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddAsPropertyAttribute.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This attribute determines whether the method should be treated as a read only property
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Attributes
{
    using System;

    /// <summary>
    /// This attribute determines whether the method should be treated as a read only property
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class AddAsPropertyAttribute : Attribute
    {
    }
}