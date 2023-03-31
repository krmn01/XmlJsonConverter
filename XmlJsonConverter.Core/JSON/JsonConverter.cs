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
            JArray arr = new JArray(doc.Values());
            ObservableCollection<JsonElementViewModel> tmp = new ObservableCollection<JsonElementViewModel>();


            foreach (JProperty prop in doc.DescendantsAndSelf().OfType<JProperty>())
            {
                if (prop.Value is JValue || prop.Value is JArray)
                {
                    JObject parent = (JObject)prop.Parent;
                    string parentName = parent.Properties().FirstOrDefault()?.Name;
                    string propName = prop.Name;

                    if (!tmp.Contains(new JsonElementViewModel(propName, parentName)))
                    {
                        tmp.Add(new JsonElementViewModel(propName, parentName));
                    }
                }
            }

            return tmp;
        }
    }
}
