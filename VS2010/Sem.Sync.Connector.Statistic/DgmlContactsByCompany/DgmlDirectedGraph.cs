// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DgmlDirectedGraph.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the DirectedGraph type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Statistic.DgmlContactsByCompany
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    using Sem.Sync.SyncBase.DetailData;

    [XmlRoot(ElementName = "DirectedGraph", Namespace = "http://schemas.microsoft.com/vs/2009/dgml")]
    public class DgmlDirectedGraph
    {
        public DgmlDirectedGraph()
        {
            this.Nodes = new List<DgmlNode>();
        }

        public DgmlDirectedGraph(List<StdElement> elements)
        {
            this.Nodes = new List<DgmlNode>(from element in elements select new DgmlNode(element));
        }

        [XmlAttribute()]
        public DgmlGraphDirection GraphDirection { get; set; }

        [XmlAttribute()]
        public DgmlLayout Layout { get; set; }

        [XmlArrayItem(ElementName = "Node")]
        public List<DgmlNode> Nodes { get; set; }

        [XmlArrayItem(ElementName = "Link")]
        public List<DgmlLink> Links { get; set; }

        [XmlArrayItem(ElementName = "Category")]
        public List<DgmlCategory> Categories { get; set; }
    }
}
