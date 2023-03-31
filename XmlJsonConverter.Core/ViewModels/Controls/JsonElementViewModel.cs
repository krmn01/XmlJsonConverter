using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlJsonConverter.Core
{
    public class JsonElementViewModel
    {
        public string jsonElementName { get; set; }
        public string path { get; set; }

        public string parentName { get; set; }
        public bool isChecked { get; set; }

        public string preparedToShowName{ get{
                /* if (jsonElementName.StartsWith("@") || jsonElementName.StartsWith("#"))
                 {
                     return "\t\t" + jsonElementName;
                 }
                 else return jsonElementName;
                */
                int index = path.IndexOf(jsonElementName);
                if (index != -1)
                {
                    string partBeforeSubstring = path.Substring(0, index);
                    int charsBeforeSubstring = partBeforeSubstring.Length;
                    string spaces = new string(' ', charsBeforeSubstring);

                    return spaces + jsonElementName;
                }
                else return jsonElementName;



            } }

        public JsonElementViewModel(string jsonElementName, string path, string parentName)
        {
            this.jsonElementName = jsonElementName;
            this.path = path;
            this.isChecked = true;
            this.parentName = parentName;
        }
    }
}
