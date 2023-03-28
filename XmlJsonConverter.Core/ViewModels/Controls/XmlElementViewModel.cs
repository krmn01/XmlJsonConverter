using System.Xml;

namespace XmlJsonConverter.Core
{
    public class XmlElementViewModel
    {
        /*
        public XmlElement element { get; set; }

        public XmlElementViewModel(XmlElement element)
        {
            this.element = element;
        }
        public string xmlElementName { get {
                return element!=null ? element.Name : string.Empty;
            } }

        */
        public string xmlElementName { get; set; }
        public string parentName { get; set; }
        public bool isChecked { get; set; }

        /// <summary>
        /// preparedToShowName is used to display xml as tree
        /// </summary>
        public string preparedToShowName { get
            {
                string tmp = "";
                for (int i = 0; i < parentId; i++) tmp+="---";
                tmp += xmlElementName;
                return tmp;
            } }
        public int parentId { get; set; }
        public XmlElementViewModel(string element, string parent, int parentId)
        {
            this.xmlElementName = element;
            this.parentName = parent;
            this.parentId = parentId;
            this.isChecked = true;
        }



    }
}
