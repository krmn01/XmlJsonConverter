using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Xml;
using XmlJsonConverter.Core;
using Newtonsoft.Json;

namespace XmlJsonConverter.Core
{

    public class ConverterPageViewModel :BaseViewModel
    {
        public XmlDocument xmlDoc;
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
            switch (tmp)
            {
                case ".xml":
                    messageService.ShowMessage("Analyzing xml file");
                    xmlDoc = XMLConverter.loadXML(filePath);
                    xmlElements = XMLConverter.getXmlElements(xmlDoc);
                    break;

                case ".json":
                    messageService.ShowMessage("Analyzing json file");
                    break;

                default:
                    errorService.ShowError("File extension should be XML or JSON!");
                    break;
            }

            filePath = string.Empty;
            convertedFileName = string.Empty;

            OnPropertyChange(nameof(xmlElements));
            OnPropertyChange(nameof(filePath));
            OnPropertyChange(nameof(convertedFileName));
        }

       
        private void ConvertFile()
        {
            if (xmlDoc!=null){

                //getting unchecked elements
                List<XmlElementViewModel> uncheckedElementList = xmlElements.Where(e => !e.isChecked).ToList();

                foreach(var element in uncheckedElementList)
                {
                    XmlNodeList nodes = xmlDoc.GetElementsByTagName(element.xmlElementName);
                   
                    for(int i = 0; i < nodes.Count; i++)
                    {
                        nodes[i].ParentNode.RemoveChild(nodes[i]);
                        
                    }
                }

                string json = JsonConvert.SerializeXmlNode(xmlDoc);
                if (convertedFileName != string.Empty)
                {
                    File.WriteAllText(convertedFileName+".json", json);
                    
                }
                else
                {
                    File.WriteAllText("output.json", json);
                }
                messageService.ShowMessage("Conversion complete");
            } 
        }

    }

}