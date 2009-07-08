namespace Sem.Sync.FilesystemConnector
{
    using System.Xml.Serialization;

    public class ColumnDefinition
    {
        [XmlAttribute]
        public string Title { get; set; }
        
        [XmlAttribute]
        public string Selector { get; set; }

        [XmlAttribute]
        public string FormatString { get; set; }
        
        public ColumnDefinition(){}

        public ColumnDefinition(string selector)
        {
            this.Title = selector;
            this.Selector = selector;
        }
        
        public ColumnDefinition(string selector, string formatString) : this(selector)
        {
            this.FormatString = formatString;
        }
    }
}