// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DgmlDirectedGraph.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the DirectedGraph type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Statistic.Dgml
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    using Sem.Sync.SyncBase.DetailData;

    [XmlRoot(ElementName = "DirectedGraph", Namespace = "http://schemas.microsoft.com/vs/2009/dgml")]
    public class Graph
    {
        public Graph()
        {
            this.Nodes = new List<Node>();
        }

        public Graph(List<StdElement> elements)
        {
            this.Nodes = new List<Node>(from element in elements select new Node(element));
        }

        [XmlAttribute()]
        public GraphDirection GraphDirection { get; set; }

        [XmlAttribute()]
        public Layout Layout { get; set; }

        [XmlArrayItem(ElementName = "Node")]
        public List<Node> Nodes { get; set; }

        [XmlArrayItem(ElementName = "Link")]
        public List<Link> Links { get; set; }

        [XmlArrayItem(ElementName = "Category")]
        public List<Category> Categories { get; set; }
    }
}
