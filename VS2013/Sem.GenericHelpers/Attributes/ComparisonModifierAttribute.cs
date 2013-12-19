// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComparisonModifierAttribute.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This attribute determines the way a property is compared to the same property of
//   another object while merging and comparing
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Attributes
{
    using System;

    /// <summary>
    /// This attribute determines the way a property is compared to the same property of 
    ///   another object while merging and comparing
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ComparisonModifierAttribute : Attribute
    {
        #region Properties

        /// <summary>
        ///   Gets or sets a value indicating whether the merge/compare of this property should be made case insensitive
        /// </summary>
        public bool CaseInsensitive { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the comparison of this property should be skipped
        /// </summary>
        public bool SkipCompare { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the merge of this property should be skipped
        /// </summary>
        public bool SkipMerge { get; set; }

        #endregion
    }
}