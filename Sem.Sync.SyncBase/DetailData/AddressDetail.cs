//-----------------------------------------------------------------------
// <copyright file="AddressDetail.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.DetailData
{
    /// <summary>
    /// Holds the detail data of an address
    /// </summary>
    public class AddressDetail
    {
        /// <summary>
        /// Gets or sets the name of the country (USA / Germany / Spain...)
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// Gets or sets the name of the state or province (Texas (in USA) / Hessen (in Germany) / ...)
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// Gets or sets the postal code or "zip-code"
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the city ("Washington DC", "Fankfurt am Main"...)
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// Gets or sets the name of the street
        /// </summary>
        public string StreetName { get; set; }

        /// <summary>
        /// Gets or sets the number of the house in the street
        /// </summary>
        public int StreetNumber { get; set; }

        /// <summary>
        /// Gets or sets an identifier for the room inside the house/building
        /// </summary>
        public string Room { get; set; }

        /// <summary>
        /// Gets or sets an addition to the street number (like the "a" in "Birkenweg 21a")
        /// </summary>
        public string StreetNumberExtension { get; set; }

        /// <summary>
        /// Gets or sets the phone number that belongs to the address
        /// </summary>
        public PhoneNumber Phone { get; set; }

        /// <summary>
        /// Builds up a string representation of the information inside this object.
        /// </summary>
        /// <returns>a well formatted string representation of the data</returns>
        public override string ToString()
        {
            var result = this.StreetName ?? string.Empty;
            result += " " + this.StreetNumber;
            result += (this.StreetNumberExtension ?? string.Empty) + " / ";
            result += (this.PostalCode ?? string.Empty) + " ";
            result += this.CityName ?? string.Empty;
            result += " (" + (this.StateName ?? string.Empty) + ")";
            result += this.CountryName ?? string.Empty;
            result += " Phone: " + ((this.Phone == null) ? string.Empty : this.Phone.ToString());

            return (result == " 0 /   () Phone: ") ? null : result;
        }
    }
}