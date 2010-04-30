// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppointmentItemContainer.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   to cache the items of the outlook appointment folder and to be able
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
    public class AppointmentItemContainer
    {
        /// <summary>
        /// name of the custom outlook property that does hold the Sem.Sync entity id
        /// </summary>
        private const string AppointmentIdOutlookPropertyName = "SemSyncId";

        /// <summary>
        /// backing variable of the appointment subject
        /// </summary>
        private string subject;

        /// <summary>
        /// backing variable of the appointment unique id
        /// </summary>
        private string iD;

        /// <summary>
        /// Gets or sets the cached original contact item
        /// </summary>
        internal AppointmentItem Item { get; set; }

        /// <summary>
        /// Gets the subject of the cached appointment item
        /// </summary>
        public string LastName
        {
            get
            {
                // check cache and read from item, if empty
                if (this.subject == null)
                {
                    this.subject = this.Item.Subject ?? string.Empty;
                }

                return this.subject;
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
                    var prop = this.Item.UserProperties[AppointmentIdOutlookPropertyName];
                    this.iD = (prop == null) ? string.Empty : prop.Value.ToString();
                }

                return this.iD;
            }
        }
    }
}