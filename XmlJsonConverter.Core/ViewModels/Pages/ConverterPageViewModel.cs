using Microsoft.Win32;
using System.ComponentModel;
using System.Windows.Input;
using XmlJsonConverter.Core;

namespace XmlJsonConverter.Core
{

    public class ConverterPageViewModel :BaseViewModel
    {
        public string filePath { get; set; }
        public string convertedFileName { get; set; }

        public bool correctExtension { get; set; }

        public ICommand ConvertFileCommand { get; set;}

        private readonly IErrorService errorService;       

        public ConverterPageViewModel(IErrorService err)
        {
            this.errorService = err;
            ConvertFileCommand = new RelayCommand(ConvertFile);
        }

       
        private void ConvertFile()
        {
            string tmp = Path.GetExtension(filePath);
            switch (tmp)
            {
                case ".xml":
                    errorService.ShowError("Converting!");
                    break;

                case ".json":
                    break;

                default:
                    errorService.ShowError(tmp);
                    break;
            }

            //filePath = string.Empty;
            convertedFileName = string.Empty;

            OnPropertyChange(nameof(filePath));
            OnPropertyChange(nameof(convertedFileName));
        }

    }

}