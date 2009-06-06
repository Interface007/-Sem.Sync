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

    public class PhoneNumber
    {
        public PhoneNumber()
        {
        }

        private string denormalizedPhoneNumber = string.Empty;

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

        public CountryCode CountryCode { get; set; }
        public int AreaCode { get; set; }
        public string Number { get; set; }

        public PhoneNumber(string phoneNumber)
        {
            this.DenormalizedPhoneNumber = phoneNumber;
        }

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