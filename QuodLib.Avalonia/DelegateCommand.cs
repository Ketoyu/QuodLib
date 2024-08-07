﻿using System.Windows.Input;

namespace QuodLib.Avalonia {
    /// <summary>
    /// Accepts a delegate and uses that for the command.
    /// </summary>
    public class DelegateCommand : ICommand {
        private readonly Func<object?, bool>? _canExecute;
        private readonly Action<object?> _execute;

        public DelegateCommand(Action<object?> execute, Func<object?, bool> canExecute) {
            _execute = execute;
            _canExecute = canExecute;
        }

        public DelegateCommand(Action<object?> execute) {
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
            => _canExecute?.Invoke(parameter) ?? true;

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, 
        ///     this object can be set to null.
        /// </param>
        public void Execute(object? parameter)
            => _execute.Invoke(parameter);

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler? CanExecuteChanged;
    }
}
