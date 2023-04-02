using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace XmlJsonConverter.Core
{
    public class JsonConverter
    {

        public static void ConvertObjectToJson(XmlElement parentElem, JObject jsonObj)
        {
            foreach (JProperty prop in jsonObj.Properties())
            {
                if (prop.Name.StartsWith("@"))
                {
                    // Handle attribute
                    parentElem.SetAttribute(prop.Name.Substring(1), prop.Value.ToString());
                }
                else if (prop.Value.Type == JTokenType.Object)
                {
                    // Handle nested objects
                    XmlElement elem = parentElem.OwnerDocument.CreateElement(prop.Name);
                    parentElem.AppendChild(elem);
                    ConvertObjectToJson(elem, (JObject)prop.Value);
                }
                else if (prop.Value.Type == JTokenType.Array)
                {
                    // Handle arrays
                    XmlElement elem = parentElem.OwnerDocument.CreateElement(prop.Name);
                    parentElem.AppendChild(elem);
                    ConvertArrayToJson(elem, (JArray)prop.Value);
                }
                else
                {
                    // Handle other types
                    XmlElement elem = parentElem.OwnerDocument.CreateElement(prop.Name);
                    elem.InnerText = prop.Value.ToString();
                    parentElem.AppendChild(elem);
                }
            }

            // Remove empty elements
            XmlNodeList emptyNodes = parentElem.SelectNodes("*[not(node())]");
            foreach (XmlNode emptyNode in emptyNodes)
            {
                emptyNode.ParentNode.RemoveChild(emptyNode);
            }
        }

        public static void ConvertArrayToJson(XmlElement parentElem, JArray jsonArr)
        {
            foreach (JToken token in jsonArr)
            {
                if (token.Type == JTokenType.Object)
                {
                    // Handle objects
                    XmlElement elem = parentElem.OwnerDocument.CreateElement(parentElem.Name);
                    parentElem.ParentNode.AppendChild(elem);
                    ConvertObjectToJson(elem, (JObject)token);
                }
                else if (token.Type == JTokenType.Array)
                {
                    // Handle nested arrays
                    ConvertArrayToJson(parentElem, (JArray)token);
                }
                else
                {
                    // Handle other types
                    XmlElement elem = parentElem.OwnerDocument.CreateElement(parentElem.Name);
                    elem.InnerText = token.ToString();
                    parentElem.ParentNode.AppendChild(elem);
                }
            }

            // Remove empty elements
            XmlNodeList emptyNodes = parentElem.SelectNodes("*[not(node())]");
            foreach (XmlNode emptyNode in emptyNodes)
            {
                emptyNode.ParentNode.RemoveChild(emptyNode);
            }
        }

        public static ObservableCollection<JsonElementViewModel> getJsonElements(JObject doc)
        {
            HashSet<(string,string)> addedElements = new HashSet<(string,string)>();
            ObservableCollection<JsonElementViewModel> tmp = new ObservableCollection<JsonElementViewModel>();


            foreach (JProperty prop in doc.DescendantsAndSelf().OfType<JProperty>())
            {
                JToken token = prop.Value;
                string path = token.Path;
                if (prop.Value is JValue || prop.Value is JArray)
                {
                    JObject parent = (JObject)prop.Parent;
                    string parentName = parent.Properties().FirstOrDefault()?.Name;
                    string propName = prop.Name;

                    (string, string) elementName = (propName, parentName);
                    if (!addedElements.Contains(elementName))
                    {
                        tmp.Add(new JsonElementViewModel(propName, path,parentName));
                        addedElements.Add(elementName);
                    }
                }
            }

            return tmp;
        }
    }
}
