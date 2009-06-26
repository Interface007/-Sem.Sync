//-----------------------------------------------------------------------
// <copyright file="PhoneNumber.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.DetailData
{
    using System.Text.RegularExpressions;
    using System.Globalization;

    /// <summary>
    /// This class represents the information needed  to establish a phone connection
    /// to the parent entity. No information should be omitted.
    /// </summary>
    public class PhoneNumber
    {

        /// <summary>
        /// Creates a new instance of the <see cref="PhoneNumber"/> class without any pre-initialized values
        /// </summary>
        public PhoneNumber()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="PhoneNumber"/> class and initializes the content witht the
        /// value of <paramref name="phoneNumber"/>. This value will be interpreted.
        /// </summary>
        /// <param name="phoneNumber">The phone number this instance should represent.</param>
        public PhoneNumber(string phoneNumber)
        {
            this.DenormalizedPhoneNumber = phoneNumber;
        }

        private string denormalizedPhoneNumber = string.Empty;

        /// <summary>
        /// Gets or sets a denormalized phone number as one single string. If the
        /// value is not interpretable as a phone number, it will not initialize
        /// the more precise properties like <see cref="AreaCode"/> or <see cref="Number"/>.
        /// </summary>
        public string DenormalizedPhoneNumber
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Number)) return null;
                return this.ToString();
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var phoneNumberExtract = new Regex("[0-9]+");

                    var matches = phoneNumberExtract.Matches(value);
                    if ((matches.Count > 2))
                    {
                        this.CountryCode = (CountryCode)int.Parse(matches[0].Captures[0].ToString(), CultureInfo.InvariantCulture);
                        this.AreaCode = int.Parse(matches[1].Captures[0].ToString(), CultureInfo.InvariantCulture);
                        for (var i = 2; i < matches.Count; i++)
                        {
                            this.Number += matches[i].Captures[0].ToString();
                        }
                    }
                    else
                    {
                        this.denormalizedPhoneNumber = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the country code (the number to dial after "00" in germany).
        /// </summary>
        public CountryCode CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the area code (e.g. number of the city).
        /// </summary>
        public int AreaCode { get; set; }

        /// <summary>
        /// Gets or sets the final number of the phone.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Returns a formatted string of the phone number if a qualified representation
        /// of the phone number is available. 
        /// Returns the value of the property <see cref="DenormalizedPhoneNumber"/> if no
        /// qualified properties are available.
        /// </summary>
        /// <returns>The formatted number.</returns>
        /// <example>+49 (1234) 456789</example>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(this.denormalizedPhoneNumber))
            {
                return this.denormalizedPhoneNumber;
            }

            // +49 (1234) 456789
            var result = "";
            result += (this.CountryCode == CountryCode.unspecified) ? "" : "+" + (int)this.CountryCode + " ";
            result += (this.AreaCode == 0) ? "" : "(" + this.AreaCode + ")";
            result += (string.IsNullOrEmpty(this.Number)) ? "" : " " + this.Number;

            return result.Trim();
        }
    }
}