//-----------------------------------------------------------------------
// <copyright file="ReplacementLists.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.DetailData
{
    using System.Collections.Generic;

    /// <summary>
    /// A replacements list does contain different lists of key value pairs that 
    /// define replacement configuration to use a standardized string for one or
    /// multiple input strings. E.g. you  can write the company name "SDX AG" in
    /// many ways like "Sdx Ag" or "SDX-AG". To have a normalized set or strings
    /// inside the internal data, these lists provide translations.
    /// </summary>
    public class ReplacementLists
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ReplacementLists"/> class.
        /// </summary>
        public ReplacementLists()
        {
            this.BusinessCompanyName = new List<KeyValuePair>();
            this.BusinessHomepage = new List<KeyValuePair>();
        }

        /// <summary>
        /// The list of permutations of company names with replacement strings.
        /// </summary>
        public List<KeyValuePair> BusinessCompanyName { get; set; }

        /// <summary>
        /// The list of permutations of business URLs with replacement strings.
        /// </summary>
        public List<KeyValuePair> BusinessHomepage { get; set; }
    }
}
