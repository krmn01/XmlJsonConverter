using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Xml;
using XmlJsonConverter.Core;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;
using Newtonsoft.Json.Linq;

namespace XmlJsonConverter.Core
{

    public class ConverterPageViewModel :BaseViewModel
    {
        public XmlDocument xmlDoc;
        public JObject jsonDoc;

        public ObservableCollection<JsonElementViewModel> jsonElements { get; set; } = new ObservableCollection<JsonElementViewModel>();
        public ObservableCollection<XmlElementViewModel> xmlElements { get; set; } = new ObservableCollection<XmlElementViewModel>();
        public string filePath { get; set; }
        public string convertedFileName { get; set; }

        public bool correctExtension { get; set; }

        public ICommand ConvertFileCommand { get; set;}
        public ICommand AnalyzeFileCommand { get; set; }

        private readonly IErrorService errorService;
        private readonly IMessageService messageService;

        public ConverterPageViewModel(IErrorService err, IMessageService msg)
        {
            xmlElements = new ObservableCollection<XmlElementViewModel>();
            this.errorService = err;
            this.messageService = msg;
            ConvertFileCommand = new RelayCommand(ConvertFile);
            AnalyzeFileCommand = new RelayCommand(AnlyzeFile);
        }


        private void AnlyzeFile()
        {
            string tmp = Path.GetExtension(filePath);
            if (xmlElements != null) xmlElements = new ObservableCollection<XmlElementViewModel>();
            else if (jsonElements != null) jsonElements = new ObservableCollection<JsonElementViewModel>();

            switch (tmp)
            {
                case ".xml":
                    messageService.ShowMessage("Analyzing xml file");
                    xmlDoc = XMLConverter.loadXML(filePath);
                    xmlElements = XMLConverter.getXmlElements(xmlDoc);
                    break;

                case ".json":
                    messageService.ShowMessage("Analyzing json file");
                    string json = File.ReadAllText(filePath);
                    jsonDoc = JObject.Parse(json);
                    jsonElements = JsonConverter.getJsonElements(jsonDoc);
                   
                    break;

                default:
                    errorService.ShowError("File extension should be XML or JSON!");
                    break;
            }

            filePath = string.Empty;
            convertedFileName = string.Empty;

            OnPropertyChange(nameof(xmlElements));
            OnPropertyChange(nameof(jsonElements));
            OnPropertyChange(nameof(filePath));
            OnPropertyChange(nameof(convertedFileName));
        }

        private void deleteUncheckedNodes(List<XmlElementViewModel> uncheckedElementList, XmlNodeList docNodes)
        {
            foreach (XmlNode node in docNodes)
            {
                foreach(var uncheckedElement in uncheckedElementList){

                    if (node is XmlElement elementNode)
                    {
                        if (uncheckedElement.xmlElementName.Equals(elementNode.Name))
                        { 
                            elementNode.RemoveAll();
                            elementNode.ParentNode.RemoveChild(elementNode);
                        }
                        if (node.HasChildNodes)
                        {
                            deleteUncheckedNodes(uncheckedElementList, node.ChildNodes);
                        }
                    }
                }   
                       
            }
        }



        private JObject deleteUncheckedNodes(List<JsonElementViewModel> uncheckedElementList, JObject js)
        {
            foreach(var element in uncheckedElementList){
             
                foreach (var property in js.DescendantsAndSelf().OfType<JProperty>().Where(p => p.Name == element.jsonElementName).ToList())
                {
                    property.Remove();
                }
            }
            return js;
        }

       
        private void ConvertFile()
        {
            if (xmlDoc!=null){

                //getting unchecked elements
                List<XmlElementViewModel> uncheckedElementList = xmlElements.Where(e => !e.isChecked).ToList();
                XmlNodeList docNodes = xmlDoc.DocumentElement.ChildNodes;

                deleteUncheckedNodes(uncheckedElementList, docNodes);
                
                string json = JsonConvert.SerializeXmlNode(xmlDoc);
                JObject js = JObject.Parse(json);
                
                ///deleting all occurances of #whitespace
                foreach (var property in js.DescendantsAndSelf().OfType<JProperty>().Where(p => p.Name == "#whitespace").ToList())
                {
                    property.Remove();
                }
                js.Property("?xml")?.Remove();
                
                json = JsonConvert.SerializeObject(js, Newtonsoft.Json.Formatting.None);

                if (convertedFileName != string.Empty)
                {
                    File.WriteAllText(convertedFileName+".json", json);
                    
                }
                else
                {
                    File.WriteAllText("output.json", json);
                }
                xmlDoc = null;
                xmlElements.Clear();
                messageService.ShowMessage("Conversion complete");
            }else if(jsonDoc != null)
            {
                List<JsonElementViewModel> uncheckedElementList = jsonElements.Where(e => !e.isChecked).ToList();
                jsonDoc = deleteUncheckedNodes(uncheckedElementList, jsonDoc);

                XmlDocument xmlTmp = new XmlDocument();
                XmlElement root = xmlTmp.CreateElement("root");
                xmlTmp.AppendChild(root);

                foreach (JProperty prop in jsonDoc.Properties())
                {
                    if (prop.Value.Type == JTokenType.Object)
                    {
                        XmlElement elem = xmlTmp.CreateElement(prop.Name);
                        root.AppendChild(elem);
                        JsonConverter.ConvertObjectToJson(elem, (JObject)prop.Value);
                    }
                    else if (prop.Value.Type == JTokenType.Array)
                    {
                        XmlElement elem = xmlTmp.CreateElement(prop.Name);
                        root.AppendChild(elem);
                        JsonConverter.ConvertArrayToJson(elem, (JArray)prop.Value);
                    }
                    else
                    {
                        XmlElement elem = xmlTmp.CreateElement(prop.Name);
                        elem.InnerText = prop.Value.ToString();
                        root.AppendChild(elem);
                    }
                }


                if (convertedFileName != string.Empty)
                {
                    xmlTmp.Save(convertedFileName + ".xml");

                }
                else
                {
                    xmlTmp.Save("output.xml");
                }
                jsonDoc = null;
                jsonElements.Clear();
                messageService.ShowMessage("Conversion complete");
            }
        }

    }

}