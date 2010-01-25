//-----------------------------------------------------------------------
// <copyright file="PhoneNumber.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.DetailData
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    using GenericHelpers.Attributes;

    /// <summary>
    /// This class represents the information needed  to establish a phone connection
    /// to the parent entity. No information should be omitted.
    /// </summary>
    [Serializable]
    public class PhoneNumber 
    {
        /// <summary>
        /// stores the denormalized phone number
        /// </summary>
        private string denormalizedPhoneNumber = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneNumber"/> class without any pre-initialized values
        /// </summary>
        public PhoneNumber()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneNumber"/> class and initializes the content witht the
        /// value of <paramref name="phoneNumber"/>. This value will be interpreted.
        /// </summary>
        /// <param name="phoneNumber">
        /// The phone number this instance should represent.
        /// </param>
        public PhoneNumber(string phoneNumber)
        {
            this.DenormalizedPhoneNumber = phoneNumber;
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
        /// Gets or sets a denormalized phone number as one single string. If the
        /// value is not interpretable as a phone number, it will not initialize
        /// the more precise properties like <see cref="AreaCode"/> or <see cref="Number"/>.
        /// </summary>
        public string DenormalizedPhoneNumber
        {
            get
            {
                return 
                    !string.IsNullOrEmpty(this.Number) 
                    ? null 
                    : this.ToString();
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                var matches = Regex.Matches(value, "[0-9]+");
                if ((matches.Count > 2) && Enum.IsDefined(typeof(CountryCode), int.Parse(matches[0].Captures[0].ToString(), CultureInfo.InvariantCulture)))
                {
                    this.CountryCode = (CountryCode)int.Parse(matches[0].Captures[0].ToString(), CultureInfo.InvariantCulture);
                    this.AreaCode = int.Parse(matches[1].Captures[0].ToString(), CultureInfo.InvariantCulture);
                    for (var i = 2; i < matches.Count; i++)
                    {
                        this.Number += matches[i].Captures[0].ToString();
                    }
                        
                    return;
                }
                 
                if ((matches.Count == 2) && matches[0].ToString().StartsWith("0", StringComparison.Ordinal))
                {
                    this.AreaCode = int.Parse(matches[0].ToString(), CultureInfo.InvariantCulture);
                    this.Number = matches[1].ToString();
                    return;
                }
   
                this.denormalizedPhoneNumber = value;
            }
        }

        /// <summary>
        /// converts a string to a PhoneNumber instance
        /// </summary>
        /// <param name="phoneNumber">The phone number as a string.</param>
        /// <returns>a phone number instance</returns>
        public static implicit operator PhoneNumber(string phoneNumber)
        {
            return new PhoneNumber(phoneNumber);
        }

        /// <summary>
        /// Implements the equal operator using the string representation of this class (<see cref="ToString"/>).
        /// </summary>
        /// <param name="left"> The left side object of the comparison. </param>
        /// <param name="right"> The right side object of the comparison. </param>
        /// <returns> true if both object do contain the same content </returns>
        public static bool operator ==(PhoneNumber left, PhoneNumber right)
        {
            return object.Equals(left, right);
        }

        /// <summary>
        /// Implements the not equal operator using the string representation of this class (<see cref="ToString"/>).
        /// </summary>
        /// <param name="left"> The left side object of the comparison. </param>
        /// <param name="right"> The right side object of the comparison. </param>
        /// <returns> true if both object do contain the same content </returns>
        public static bool operator !=(PhoneNumber left, PhoneNumber right)
        {
            return !object.Equals(left, right);
        }

        /// <summary>
        /// Returns a formatted string of the phone number if a qualified representation
        /// of the phone number is available. 
        /// Returns the value of the property <see cref="DenormalizedPhoneNumber"/> if no
        /// qualified properties are available.
        /// </summary>
        /// <returns>The formatted number.</returns>
        /// <example>+49 (1234) 456789</example>
        [AddAsProperty]
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(this.denormalizedPhoneNumber))
            {
                return this.denormalizedPhoneNumber;
            }

            // +49 (1234) 456789
            var result = string.Empty;
            result += (this.CountryCode == CountryCode.unspecified) ? string.Empty : "+" + (int)this.CountryCode + " ";
            result += (this.AreaCode == 0) ? string.Empty : "(" + this.AreaCode + ")";
            result += string.IsNullOrEmpty(this.Number) ? string.Empty : " " + this.Number;

            return result.Trim();
        }

        /// <summary>
        /// Compares the content of this instance to another instance of the <see cref="PhoneNumber"/> class based on the <see cref="ToString"/> method.
        /// </summary>
        /// <param name="obj"> The other instance. </param>
        /// <returns> true if the content does match </returns>
        public override bool Equals(object obj)
        {
            var other = obj as PhoneNumber;
            if (other == null)
            {
                return false;
            }

            return string.Compare(other.ToString(), this.ToString(), StringComparison.Ordinal) == 0;
        }

        /// <summary>
        /// Compares the content of this instance to another instance of the <see cref="PhoneNumber"/> class based on the <see cref="ToString"/> method.
        /// </summary>
        /// <param name="other"> The other instance. </param>
        /// <returns> true if the content does match </returns>
        public bool Equals(PhoneNumber other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            
            return other.ToString().Equals(this.ToString());
        }

        /// <summary>
        /// Generates a hash code based on the content strings
        /// </summary>
        /// <returns>
        /// the generated hash
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var result = this.CountryCode.GetHashCode();
                result = (result * 397) ^ this.AreaCode;
                result = (result * 397) ^ (this.Number != null ? this.Number.GetHashCode() : 0);
                result = (result * 397) ^ (this.denormalizedPhoneNumber != null ? this.denormalizedPhoneNumber.GetHashCode() : 0);
                return result;
            }
        }
    }
}