using System;
using System.Windows.Input;

namespace AutoProjectWPF.ViewModel
{
    class ActionViewModel : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;
        public string DisplayName { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public ActionViewModel(string DisplayName, Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.DisplayName = DisplayName;
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
