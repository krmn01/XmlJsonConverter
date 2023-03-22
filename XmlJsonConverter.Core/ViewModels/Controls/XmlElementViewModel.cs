using System.Xml;

namespace XmlJsonConverter.Core
{
    public class XmlElementViewModel
    {
        public XmlElement element { get; set; }

        public XmlElementViewModel(XmlElement element)
        {
            this.element = element;
        }
        public string xmlElementName { get {
                return element!=null ? element.Name : string.Empty;
            } }

    }
}
