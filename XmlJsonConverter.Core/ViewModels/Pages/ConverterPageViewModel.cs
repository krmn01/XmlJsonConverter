using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Xml;
using XmlJsonConverter.Core;

namespace XmlJsonConverter.Core
{

    public class ConverterPageViewModel :BaseViewModel
    {
        public ObservableCollection<XmlElementViewModel> xmlElements { get; set; } = new ObservableCollection<XmlElementViewModel>();
        public string filePath { get; set; }
        public string convertedFileName { get; set; }

        public bool correctExtension { get; set; }

        public ICommand ConvertFileCommand { get; set;}

        private readonly IErrorService errorService;
        private readonly IMessageService messageService;

        public ConverterPageViewModel(IErrorService err, IMessageService msg)
        {
            xmlElements = new ObservableCollection<XmlElementViewModel>();
            this.errorService = err;
            this.messageService = msg;
            ConvertFileCommand = new RelayCommand(ConvertFile);
        }

       
        private void ConvertFile()
        {
            string tmp = Path.GetExtension(filePath);
            switch (tmp)
            {
                case ".xml":
                    messageService.ShowMessage("Converting xml file to json");
                    XmlDocument doc = XMLConverter.loadXML(filePath);
                    xmlElements = XMLConverter.getXmlElements(doc);
                    break;

                case ".json":
                    messageService.ShowMessage("Converting json file to xml");
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

    }

}