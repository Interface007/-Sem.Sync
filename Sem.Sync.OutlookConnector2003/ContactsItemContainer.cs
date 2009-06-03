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

namespace Sem.Sync.OutlookConnector2003
{
    using Microsoft.Office.Interop.Outlook;

    /// <summary>
    /// to cache the items of the outlook contacts folder and to be able
    /// to cache some of the properties of that object that will be needed
    /// in linq-queries.
    /// </summary>
    public class ContactsItemContainer
    {
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
        private string id;

        /// <summary>
        /// gets or sets the cached original contact item
        /// </summary>
        public ContactItem Item { get; set; }

        /// <summary>
        /// gets the last name of the cached contact item
        /// </summary>
        public string LastName
        {
            get
            {
                // check cache and read from item, if empty
                if (lastName == null)
                {
                    lastName = this.Item.LastName ?? "";
                }

                return lastName;
            }
        }

        /// <summary>
        /// gets the first name of the cached contact item
        /// </summary>
        public string FirstName
        {
            get
            {
                if (firstName == null)
                {
                    firstName = this.Item.FirstName ?? "";
                }

                return firstName;
            }
        }

        /// <summary>
        /// gets the unique identifier of the cached contact item
        /// </summary>
        public string Id
        {
            get
            {
                if (id == null)
                {
                    var prop = this.Item.UserProperties[ContactIdOutlookPropertyName];
                    id = (prop == null) ? "" : prop.Value.ToString();
                }

                return id;
            }
        }
    }
}
