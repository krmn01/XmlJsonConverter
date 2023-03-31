using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlJsonConverter.Core
{
    public class JsonElementViewModel
    {
        public string jsonElementName { get; set; }
        public string parentName { get; set; }
        public bool isChecked { get; set; }

        public JsonElementViewModel(string jsonElementName, string parentName)
        {
            this.jsonElementName = jsonElementName;
            this.parentName = parentName;
            this.isChecked = true;
        }
    }
}
