using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Xml;


namespace XmlJsonConverter.Core
{
    public class XMLConverter
    {
        public static XmlDocument loadXML(string path)
        {
            XmlDocument document = new XmlDocument();
            try
            {
                document.PreserveWhitespace = true;
            }
            catch(System.IO.FileNotFoundException)
{
                document.LoadXml("<?xml version=\"1.0\"?> \n" +
                "<books xmlns=\"http://www.contoso.com/books\"> \n" +
                "  <book genre=\"novel\" ISBN=\"1-861001-57-8\" publicationdate=\"1823-01-28\"> \n" +
                "    <title>Pride And Prejudice</title> \n" +
                "    <price>24.95</price> \n" +
                "  </book> \n" +
                "  <book genre=\"novel\" ISBN=\"1-861002-30-1\" publicationdate=\"1985-01-01\"> \n" +
                "    <title>The Handmaid's Tale</title> \n" +
                "    <price>29.95</price> \n" +
                "  </book> \n" +
                "</books>");
            }
            document.Load(path);
            return document;
        }

        public static ObservableCollection<XmlElementViewModel> getXmlElements(XmlDocument document)
        {
            ObservableCollection<XmlElementViewModel> elements = new ObservableCollection<XmlElementViewModel>();

            TraverseNodes(document.ChildNodes, elements);

            return elements;
        }

        private static void TraverseNodes(XmlNodeList nodes, ObservableCollection<XmlElementViewModel> elements)
        {
            foreach (XmlNode node in nodes)
            {
                // If the node is an XmlElement and is not already in the collection, add it
                if (node is XmlElement elem && !elements.Contains(new XmlElementViewModel(elem)))
                {
                    elements.Add(new XmlElementViewModel(elem));
                }

                // Recursively traverse any child nodes
                if (node.HasChildNodes)
                {
                    TraverseNodes(node.ChildNodes, elements);
                }
            }
        }


    }
}
