// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MatchView.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the MatchView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SharedUI.WinForms.ViewModel
{
    using System;

    public class MatchView : IComparable<MatchView>
    {
        public string ContactName { get; set; }
        public string ContactNameMatch { get; set; }
        public Guid BaselineId { get; set; }

        public override string ToString()
        {
            return this.ContactName + " is matched to " + this.ContactNameMatch;
        }

        #region IComparable<MatchView> Members

        public int CompareTo(MatchView other)
        {
            return this.ToString().CompareTo(other.ToString());
        }

        #endregion
    }
}