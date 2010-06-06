// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DgmlCategory.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the DgmlCategory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Statistic.DgmlContactsByCompany
{
    using System;
    using System.Drawing;
    using System.Xml.Serialization;

    /// <summary>
    /// The Category type for DGML.
    /// </summary>
    public sealed class DgmlCategory 
    {
        private static int instanceCounter;

        private static readonly Color[] backColors = new[]
            {
                Color.RoyalBlue, 
                Color.CornflowerBlue, 
                Color.Gold, 
                Color.Green, 
                Color.LightSeaGreen, 
                Color.MintCream, 
                Color.Firebrick
            };

        public DgmlCategory()
        {
            instanceCounter++;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DgmlCategory"/> class.
        /// </summary>
        /// <param name="label"> The label of the new category. </param>
        public DgmlCategory(string label)
        {
            instanceCounter++;
            this.Id = label;
            this.Label = label;
            this.Background = backColors[instanceCounter % backColors.Length].Name;
        }

        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public string Label { get; set; }

        [XmlAttribute]
        public string BasedOn { get; set; }

        [XmlAttribute]
        public string Background { get; set; }
    }
}
