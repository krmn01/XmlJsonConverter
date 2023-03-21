using System.Windows;

namespace XmlJsonConverter
{
    public class ErrorService :IErrorService
    {
        public void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
