using System.ComponentModel;

namespace XmlJsonConverter.Core
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged = (sender,args) => { };

        protected void OnPropertyChange(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
