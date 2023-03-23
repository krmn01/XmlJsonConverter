using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
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

            //int currentElementId = 0;
            TraverseNodes(document.ChildNodes, elements, "", "", 0, -1);

            //deleting root element because we dont want to give a chance to delete it using checkboxes
            elements.RemoveAt(0);   
                

            return elements;
        }

        private static void TraverseNodes(XmlNodeList nodes, ObservableCollection<XmlElementViewModel> elements, string currentName,string parentName, int currentId, int parentId)
        {
            foreach (XmlNode node in nodes)
            {
                if (node is XmlElement elem)
                {
                    var name = elem.Name;
                    // Check if an element with the same name already exists in the collection
                    var elementExists = elements.Any(e => e.xmlElementName == name);
                    if (!elementExists)
                    {
                        // If not, add the element to the collection
                        var element = new XmlElementViewModel(elem.Name, parentName, parentId);
                        
                        elements.Add(element);

                        // Traverse child nodes recursively
                        if (node.HasChildNodes)
                        {
                            currentName = element.xmlElementName;
                            currentId += 1;
                            TraverseNodes(node.ChildNodes, elements, currentName,element.xmlElementName,currentId,currentId-1);
                        }
                    }
                }
            }

        }


    }
}
