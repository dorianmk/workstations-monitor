using AdminClientApp.Entry;
using System;
using System.Windows.Input;

namespace AdminClientApp.ViewModels.Common
{
    public class RelayCommand : ICommand
    {
        private Predicate<object> _canExecute;
        private Action<object> _execute;


        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged(bool useAppDispatcher = false)
        {
            if (useAppDispatcher)
                App.Current.Dispatcher.Invoke(() => RaiseCanExecuteChanged(false));
            else
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
