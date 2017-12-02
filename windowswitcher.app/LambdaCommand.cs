using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace windowswitcher.app
{
    public class LambdaCommand : ICommand
    {
        private Action<object> action;
        private Func<object, bool> predicate;

        public LambdaCommand(Action<object> _action, Func<object, bool> _predicate = null)
        {
            action = _action ?? throw new ArgumentNullException("_action");
            predicate = _predicate ?? new Func<object, bool>(_ => true);
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) => predicate(parameter);

        public void Execute(object parameter) => action(parameter);
    }
}
