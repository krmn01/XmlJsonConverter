using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace XmlJsonConverter.Core
{
    public class JsonConverter
    {

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
