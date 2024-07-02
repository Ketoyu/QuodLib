using System.Windows.Input;

namespace QuodLib.Avalonia {
    public class DelegateCommand : ICommand {
        private Func<object?, bool>? _canExecute;
        private Action<object?> _execute;

        public DelegateCommand(Action<object?> execute, Func<object?, bool> canExecute) {
            _execute = execute;
            _canExecute = canExecute;
        }

        public DelegateCommand(Action<object?> execute) {
            _execute = execute;
            _canExecute = null;
        }

        public bool CanExecute(object? parameter)
            => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object? parameter)
            => _execute.Invoke(parameter);

        public event EventHandler? CanExecuteChanged;
    }
}
