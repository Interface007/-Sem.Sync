// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncCollection.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Binding
{
    using System.ComponentModel;

    /// <summary>
    /// implements a binding list for the SyncDescription class
    /// </summary>
    public class SyncCollection : BindingList<SyncDescription>
    {
        /// <summary>
        /// overrides the AddNewCore method to create the correct object instance
        /// </summary>
        /// <returns>a new item of the SyncDescription class</returns>
        protected override object AddNewCore()
        {
            var newItem = new SyncDescription();
            this.Add(newItem);
            return newItem;
        }
    }
}