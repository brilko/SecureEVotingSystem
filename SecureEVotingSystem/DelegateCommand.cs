using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SecureEVotingSystem {
    class DelegateCommand : ICommand {
        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute = null) {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        private readonly Func<object, bool> canExecute;
        public bool CanExecute(object parameter)
            => canExecute == null || canExecute(parameter);

        private readonly Action<object> execute;
        public void Execute(object parameter)
            => execute(parameter);
    }
}
