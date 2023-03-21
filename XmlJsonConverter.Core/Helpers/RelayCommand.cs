using System.Windows;
using System.Windows.Input;

namespace XmlJsonConverter.Core
{
    public class RelayCommand : ICommand
    {
        private Action mAction;
        private Func<object, RoutedEventArgs> mFunc;
        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action action)
        {
            mAction = action;
        }

        public RelayCommand(Func<object, RoutedEventArgs> func)
        {
            mFunc = func;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            mAction?.Invoke();
            mFunc?.Invoke(parameter);
        }
    }
}
