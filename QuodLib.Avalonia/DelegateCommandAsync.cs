using System.Windows.Input;

namespace QuodLib.Avalonia {
    /// <summary>
    /// Accepts an async delegate and uses that for the command.
    /// </summary>
    public class DelegateCommandAsync : ICommand {
        private readonly Func<object?, bool>? _canExecute;
        private readonly Func<object?, Task> _execute;
        private bool _isExecuting;

        /// <summary>
        /// Occurs when the async command encounters an <see cref="Exception"/>.
        /// </summary>
        public Action<Exception>? OnException { get; init; }

        /// <summary>
        /// Whether the async command is currently in the process of executing.
        /// </summary>
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

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, 
        ///     this object can be set to null.
        /// </param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object? parameter)
            => !IsExecuting && (_canExecute?.Invoke(parameter) ?? true);

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, 
        ///     this object can be set to null.
        /// </param>
        public async void Execute(object? parameter) {
            IsExecuting = true;
            try {
                await _execute.Invoke(parameter);
            } catch (Exception ex) {
                OnException?.Invoke(ex);
            }
            IsExecuting = false;
        }

        /// <summary>
        /// Fires <see cref="CanExecuteChanged"/>, unless the command is already in the process of executing.
        /// </summary>
        /// <remarks>
        /// See also <seealso cref="IsExecuting"/>.
        /// </remarks>
        /// 
        public void OnCanExecuteChanged() {
            if (IsExecuting)
                return;

            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler? CanExecuteChanged;
    }
}
