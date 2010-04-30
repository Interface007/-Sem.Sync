// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactsItemContainer.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   to cache the items of the outlook contacts folder and to be able
//   to cache some of the properties of that object that will be needed
//   in linq-queries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Outlook
{
    using Microsoft.Office.Interop.Outlook;

    /// <summary>
    /// to cache the items of the outlook contacts folder and to be able
    /// to cache some of the properties of that object that will be needed
    /// in linq-queries.
    /// </summary>
    public class ContactsItemContainer
    {
        /// <summary>
        /// name of the custom outlook property that does hold the Sem.Sync entity id
        /// </summary>
        private const string ContactIdOutlookPropertyName = "SemSyncId";

        /// <summary>
        /// backing variable of the contacts last name
        /// </summary>
        private string lastName;

        /// <summary>
        /// backing variable of the contacts first name
        /// </summary>
        private string firstName;

        /// <summary>
        /// backing variable of the contacts unique id
        /// </summary>
        private string iD;

        /// <summary>
        /// Gets or sets the cached original contact item
        /// </summary>
        internal ContactItem Item { get; set; }

        /// <summary>
        /// Gets the last name of the cached contact item
        /// </summary>
        public string LastName
        {
            get
            {
                // check cache and read from item, if empty
                if (this.lastName == null)
                {
                    this.lastName = this.Item.LastName ?? string.Empty;
                }

                return this.lastName;
            }
        }

        /// <summary>
        /// Gets the first name of the cached contact item
        /// </summary>
        public string FirstName
        {
            get
            {
                if (this.firstName == null)
                {
                    this.firstName = this.Item.FirstName ?? string.Empty;
                }

                return this.firstName;
            }
        }

        /// <summary>
        /// Gets the unique identifier of the cached contact item
        /// </summary>
        public string Id
        {
            get
            {
                if (this.iD == null)
                {
                    var prop = this.Item.UserProperties[ContactIdOutlookPropertyName];
                    this.iD = (prop == null) ? string.Empty : prop.Value.ToString();
                }

                return this.iD;
            }
        }
    }
}
