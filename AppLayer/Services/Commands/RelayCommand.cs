using System;
using System.Windows.Input;

namespace AppCore.Services.Commands
{
    public class RelayCommand : ICommand
    {
        private Action<object> _execute;
        private Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }

        }
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this._execute = execute;
            this._canExecute = canExecute;

        }
        public void Execute(object parameter)
        {
            this._execute(parameter);
        }
        public bool CanExecute(object parameter)
        {
            return this._canExecute == null || this._canExecute(parameter);
        }

        //{
        //    return this.execute != null || this.canExecute(parameter);
        //}

    }
}
