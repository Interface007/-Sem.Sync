﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MatchCandidateView.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the MatchCandidateView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SharedUI.WinForms.ViewModel
{
    using SyncBase;

    public class MatchCandidateView
    {
        public string ContactName { get; set; }

        public StdContact Element { get; set; }
    }
}