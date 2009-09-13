//-----------------------------------------------------------------------
// <copyright file="AddressDetail.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
using System;
using System.Text.RegularExpressions;

namespace Sem.Sync.SyncBase.DetailData
{
    public enum AddressFormatting
    {
        /// <summary>
        /// plain text with all information
        /// </summary>
        Default = 0,

        /// <summary>
        /// e.g. "Sesamstreet 56\n65287 SomewhereTown"
        /// </summary>
        StreetAndCity = 1
    }

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
        /// Creates a new instance of the <see cref="AddressDetail"/> type.
        /// </summary>
        public AddressDetail()
        {
            
        }

        /// <summary>
        /// Creates a new instance of the <see cref="AddressDetail"/> type.
        /// This constructor also parses the string parameter <paramref name="address"/> by lines to extract the address information.
        /// </summary>
        /// <param name="address"></param>
        public AddressDetail(string address)
        {
            var lines = address.Split('\n');
            foreach (var parts in lines)
            {
                var line = parts
                    .Replace("\t", string.Empty)
                    .Replace("\n", string.Empty)
                    .Trim();

                if (Regex.IsMatch(line, "^[0-9]+ "))
                {
                    this.PostalCode = line.Split(' ')[0];
                    this.CityName = line.Split(' ')[1];
                    continue;
                }

                if (Regex.IsMatch(line, "^[a-zA-Z -.ﬂ‰ˆ¸ƒ÷‹]+[0-9]+"))
                {
                    this.StreetName = line;
                    continue;
                }

                if (Regex.IsMatch(line, "^[a-zA-Z -.ﬂ‰ˆ¸ƒ÷‹]+"))
                {
                    this.CountryName += line;
                    continue;
                }

                Console.WriteLine("??? : " + line);
            }
        }

        /// <summary>
        /// Builds up a string representation of the information inside this object.
        /// </summary>
        /// <returns>a well formatted string representation of the data</returns>
        public override string ToString()
        {
            return this.ToString(AddressFormatting.Default);
        }

        /// <summary>
        /// Builds up a string representation of the information inside this object.
        /// </summary>
        /// <returns>a well formatted string representation of the data</returns>
        public string ToString(AddressFormatting format)
        {
            var result = string.Empty;

            switch (format)
            {
                case AddressFormatting.StreetAndCity:
                    result += this.StreetName ?? string.Empty;
                    result = result.Replace(this.StreetNumber + (this.StreetNumberExtension ?? string.Empty), string.Empty);
                    result += this.StreetNumber > 0 ? " " + this.StreetNumber : string.Empty;
                    result += string.IsNullOrEmpty(this.StreetNumberExtension) ? string.Empty : " " + this.StreetNumberExtension;
                    result += string.IsNullOrEmpty(result) ? string.Empty : "\n";

                    result += string.IsNullOrEmpty(this.PostalCode) ? string.Empty : this.PostalCode + " ";
                    result += string.IsNullOrEmpty(this.CityName) ? string.Empty : this.CityName;
                    break;

                default:
                    result += this.StreetName ?? string.Empty;
                    result = result.Replace(this.StreetNumber + (this.StreetNumberExtension ?? string.Empty), string.Empty);
                    result += this.StreetNumber > 0 ? " " + this.StreetNumber : string.Empty;
                    result += string.IsNullOrEmpty(this.StreetNumberExtension) ? string.Empty : " " + this.StreetNumberExtension;
                    result += string.IsNullOrEmpty(result) ? string.Empty : " / ";

                    result += string.IsNullOrEmpty(this.PostalCode) ? string.Empty : this.PostalCode + " ";
                    result += string.IsNullOrEmpty(this.CityName) ? string.Empty : this.CityName + " ";
                    result += string.IsNullOrEmpty(this.StateName) ? string.Empty : "(" + this.StateName + ") ";
                    result += string.IsNullOrEmpty(this.CountryName) ? string.Empty : this.CountryName + " ";
                    result += (this.Phone == null) ? string.Empty : (" Phone: " + this.Phone);

                    result = result.Trim().Replace("  ", " ");
                    break;
            }
            
            return result;
        }
    }
}