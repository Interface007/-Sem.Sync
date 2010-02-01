//-----------------------------------------------------------------------
// <copyright file="PersonName.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.DetailData
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// This class represents the name of a person in a normalized form. This way 
    /// you can access different parts of the name like the academic title.
    /// </summary>
    [Serializable]
    public class PersonName
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonName"/> class.
        /// </summary>
        public PersonName()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonName"/> class.
        /// </summary>
        /// <param name="name"> The name to interpret. </param>
        public PersonName(string name)
        {
            var match = Regex.Match(
                name, @"^(?<last>[a-zA-Z-]+)( \((?<aka>[a-zA-Z.,-/ ]*)\))?,[ ]*(?<first>\w*)( (?<second>.*))?");
            if (match.Groups.Count > 0 && !string.IsNullOrEmpty(match.Groups[0].ToString()))
            {
                this.FirstName = match.Groups["first"].ToString();
                this.MiddleName = match.Groups["second"].ToString();
                this.LastName = match.Groups["last"].ToString();
                this.AcademicTitle = match.Groups["aka"].ToString();
                return;
            }
            
            var namePartsX = name.Split(' ');
            if (namePartsX.Length == 2)
            {
                this.FirstName = namePartsX[0];
                this.LastName = namePartsX[1];
                return;
            }

            if (namePartsX.Length == 3)
            {
                this.FirstName = namePartsX[0];
                this.MiddleName = namePartsX[1];
                this.LastName = namePartsX[2];
                return;
            }
        }

        /// <summary>
        /// Gets or sets the academic title of the person (like Dr.)
        /// </summary>
        public string AcademicTitle { get; set; }

        /// <summary>
        /// Gets or sets the first name of the person. This is normally the name good friends of 
        /// this person use when communicating privately.
        /// </summary>
        /// <example>
        /// In case of the name "Sven-Peter Hans Emmentaler" this property should get 
        /// the part "Sven-Peter".
        /// </example>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the middle name(s) of the person. Some persons do have more than one 
        /// "private" name. This property should be filled with the middle names
        /// separated by a space.
        /// </summary>
        /// <example>
        /// In case of the name "Sven-Peter Hans Maria Emmentaler" this property should get 
        /// the part "Hans Maria", because Sven-Peter is seen as one single part of the name.
        /// </example>
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the official family name (in case of wester europe).
        /// </summary>
        /// <example>
        /// In case of the name "Sven-Peter Hans Maria Emmentaler" this property should get 
        /// the part "Emmentaler", because Sven-Peter is seen as one single part of the name.
        /// </example>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the official family name (in case of wester europe).
        /// </summary>
        /// <example>
        /// In case of the name "Klaus Maria von Emmentaler" this property should get 
        /// the part "von", because this is a commonly used prefix for names.
        /// </example>
        public string LastNamePrefix { get; set; }

        /// <summary>
        /// Gets or sets the suffix to the name.
        /// </summary>
        /// <example>
        /// In case of the name "Klaus Maria von Emmentaler Sen." this property should get 
        /// the part "Sen.", because this is a commonly used suffix for names describing 
        /// that this is the son of a father with the same name.
        /// </example>
        public string Suffix { get; set; }

        /// <summary>
        /// Gets or sets the former LastName of the person. This entity does only support one 
        /// former name.
        /// </summary>
        public string FormerName { get; set; }

        /// <summary>
        /// converts a string implicit ro a PersonName
        /// </summary>
        /// <param name="newName"> The new name. </param>
        /// <returns> the new initialized person name class </returns>
        public static implicit operator PersonName(string newName)
        {
            return new PersonName(newName);
        }
        
        /// <summary>
        /// Overrides the inherited ToString method from object to represent a meaningful name
        /// </summary>
        /// <returns>the full name of the person</returns>
        public override string ToString()
        {
            var name = this.LastName;
            name += string.IsNullOrEmpty(this.AcademicTitle) ? string.Empty : " (" + this.AcademicTitle + ")";
            name += ((name.Length > 0) ? ", " : string.Empty) + this.FirstName + " ";
            name += this.MiddleName + " ";

            return name.Replace("()", string.Empty).Replace("  ", " ").Trim();
        }
    }
}