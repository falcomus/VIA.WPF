// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDataGrid.NestedTypes.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Input;

namespace VIA.WPF.Controls;

public partial class XDataGrid
{
    #region ### Nested Types ###
    private sealed class DelegateCommand(Action execute, Func<bool>? canExecute = null) : ICommand
    {
        #region ### Public Events ###
        /// <inheritdoc />
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        #endregion

        #region ### Public Methods ###
        /// <inheritdoc />
        public bool CanExecute(object? parameter)
        {
            return canExecute?.Invoke() ?? true;
        }

        /// <inheritdoc />
        public void Execute(object? parameter)
        {
            if (this.CanExecute(parameter))
            {
                execute();
            }
        }
        #endregion
    }

    private sealed class ObjectValueComparer : IEqualityComparer<object?>
    {
        #region ### Public Methods ###
        public new bool Equals(object? x, object? y)
        {
            return string.Equals(x?.ToString(), y?.ToString(), StringComparison.CurrentCulture);
        }

        public int GetHashCode(object? obj)
        {
            return (obj?.ToString() ?? string.Empty).GetHashCode(StringComparison.CurrentCulture);
        }
        #endregion
    }
    #endregion
}
