using System.Windows.Input;

namespace QuodLib.Avalonia {
    public class DelegateCommandAsync : ICommand {
        private Func<object?, bool>? _canExecute;
        private Func<object?, Task> _execute;
        private bool _isExecuting;

        public bool IsExecuting {
            get => _isExecuting;
            set {
                _isExecuting = value;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }

        public DelegateCommandAsync(Func<object?, Task> execute, Func<object?, bool> canExecute) {
            _execute = execute;
            _canExecute = canExecute;
        }

        public DelegateCommandAsync(Func<object?, Task> execute) {
            _execute = execute;
            _canExecute = null;
        }

        public bool CanExecute(object? parameter)
            => !IsExecuting && (_canExecute?.Invoke(parameter) ?? true);

        public async void Execute(object? parameter) {
            IsExecuting = true;
            await _execute.Invoke(parameter);
            IsExecuting = false;
        }

        public void OnCanExecuteChanged() {
            if (IsExecuting)
                return;

            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public event EventHandler? CanExecuteChanged;
    }
}
