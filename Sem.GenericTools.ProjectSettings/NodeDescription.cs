namespace Sem.GenericTools.ProjectSettings
{
    using System;
    using System.Xml;

    public class NodeDescription
    {
        public string XPathSelector { get; set; }
        public Func<XmlDocument, string, XmlNode>[] DefaultContent { get; set; }

        public string ProcessedSelector(string parameter)
    {
            return 
                XPathSelector.Contains("{0}") 
                ? string.Format(this.XPathSelector, parameter) 
                : this.XPathSelector;
    }

        public NodeDescription()
        {
        }

        public NodeDescription(string xPathSelector, params Func<XmlDocument, string, XmlNode>[] defaultContent)
        {
            this.DefaultContent = defaultContent;
            this.XPathSelector = xPathSelector;
        }
    }
}
