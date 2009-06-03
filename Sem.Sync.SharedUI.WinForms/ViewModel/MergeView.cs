// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MergeView.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the MergeView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SharedUI.WinForms.ViewModel
{
    using SyncBase.Merging;

    public class MergeView
    {
        public string ContactName { get; set; }
        public string PropertyName { get; set; }
        public string SourceValue { get; set; }
        public string TargetValue { get; set; }

        public MergeConflict Conflict { get; set; }
    }
}