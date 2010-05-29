// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConflictTestContainer.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   While collecting information about conflicts in object properties this container will be filled for
//   each property of an object.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Merging
{
    using System;

    using Sem.Sync.SyncBase.DetailData;

    /// <summary>
    /// While collecting information about conflicts in object properties this container will be filled for
    ///   each property of an object.
    /// </summary>
    public class ConflictTestContainer
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the object reference for the base line object this property belongs to - this might be null
        /// </summary>
        public StdElement BaselineObject { get; set; }

        /// <summary>
        ///   Gets or sets the object reference for the base line property
        /// </summary>
        public object BaselineProperty { get; set; }

        /// <summary>
        ///   Gets or sets the name of the property
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        ///   Gets or sets the type information for the property
        /// </summary>
        public Type PropertyType { get; set; }

        /// <summary>
        ///   Gets or sets the object reference for the source object this property belongs to
        /// </summary>
        public StdElement SourceObject { get; set; }

        /// <summary>
        ///   Gets or sets the object reference for the source property
        /// </summary>
        public object SourceProperty { get; set; }

        /// <summary>
        ///   Gets or sets the object reference for the target object this property belongs to
        /// </summary>
        public StdElement TargetObject { get; set; }

        /// <summary>
        ///   Gets or sets the object reference for the target property
        /// </summary>
        public object TargetProperty { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a meaningful string representation for this object
        /// </summary>
        /// <returns>
        /// a meaningful string representation for this object
        /// </returns>
        public override string ToString()
        {
            return this.SourceObject + " vs. " + this.TargetObject + " : " + this.PropertyName;
        }

        #endregion
    }
}