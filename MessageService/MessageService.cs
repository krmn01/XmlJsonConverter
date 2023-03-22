using System.Windows;

namespace XmlJsonConverter
{
    public class MessageService :IMessageService
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
