// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDataGrid.Input.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace VIA.WPF.Controls;

public partial class XDataGrid
{
    #region ### Private Methods (Keyboard Input) ###
    private bool TryExecuteKeyboardShortcut(KeyEventArgs e)
    {
        if (e.Handled || Keyboard.Modifiers != ModifierKeys.None)
        {
            return false;
        }

        if (this._currentPopup?.IsOpen == true)
        {
            return false;
        }

        if (e.OriginalSource is DependencyObject source && IsInteractiveKeyboardSource(source))
        {
            return false;
        }

        if (this.IsCurrentCellEditing())
        {
            return false;
        }

        Key key = e.Key == Key.System ? e.SystemKey : e.Key;

        if (key == Key.Return && this.EditOnEnterKey)
        {
            return this.TryExecuteSelectedItemCommand(this.EditItemCommand);
        }

        bool insertPressed = key == Key.Insert || key == Key.Add || key == Key.OemPlus;

        if (insertPressed && this.NewOnInsertKey)
        {
            return this.TryExecuteNewItemCommand();
        }

        if (key == Key.Delete && this.DeleteOnDeleteKey)
        {
            return this.TryExecuteSelectedItemCommand(this.DeleteItemCommand);
        }

        return false;
    }

    private bool TryExecuteSelectedItemCommand(ICommand? command)
    {
        if (command is null)
        {
            return false;
        }

        object? item = this.GetKeyboardCommandItem();
        if (item is null)
        {
            return false;
        }

        this.SelectedItem = item;

        if (!command.CanExecute(item))
        {
            return false;
        }

        command.Execute(item);

        this.RestoreKeyboardFocus();

        return true;
    }

    private bool TryExecuteNewItemCommand()
    {
        if (this.NewItemCommand is null || !this.NewItemCommand.CanExecute(null))
        {
            return false;
        }

        this.NewItemCommand.Execute(null);

        this.RestoreKeyboardFocus();

        return true;
    }

    private object? GetKeyboardCommandItem()
    {
        object? item = this.SelectedItem;

        if (item is null || ReferenceEquals(item, CollectionView.NewItemPlaceholder))
        {
            item = this.CurrentItem;
        }

        if (item is null || ReferenceEquals(item, CollectionView.NewItemPlaceholder))
        {
            return null;
        }

        return item;
    }

    private static bool IsInteractiveKeyboardSource(DependencyObject source)
    {
        return FindVisualParent<ButtonBase>(source) is not null ||
            FindVisualParent<TextBoxBase>(source) is not null ||
            FindVisualParent<ComboBox>(source) is not null ||
            FindVisualParent<DataGridColumnHeader>(source) is not null ||
            FindVisualParent<ScrollBar>(source) is not null;
    }

    private bool IsCurrentCellEditing()
    {
        if (this.CurrentCell.Item is null ||
            ReferenceEquals(this.CurrentCell.Item, CollectionView.NewItemPlaceholder) ||
            this.CurrentCell.Column is null)
        {
            return false;
        }

        FrameworkElement? cellContent = this.CurrentCell.Column.GetCellContent(this.CurrentCell.Item);
        DataGridCell? cell = FindVisualParent<DataGridCell>(cellContent);

        return cell?.IsEditing == true;
    }
    #endregion

    #region ### Private Methods (Mouse Input) ###
    private void TryExecuteEditOnRowDoubleClick(MouseButtonEventArgs e)
    {
        if (e.Handled || !this.EditOnRowDoubleClick || e.ChangedButton != MouseButton.Left)
        {
            return;
        }

        if (this.EditItemCommand is null)
        {
            return;
        }

        if (e.OriginalSource is not DependencyObject source || IsInteractiveDoubleClickSource(source))
        {
            return;
        }

        DataGridRow? row = FindVisualParent<DataGridRow>(source);
        if (row?.DataContext is null || ReferenceEquals(row.DataContext, CollectionView.NewItemPlaceholder))
        {
            return;
        }

        object item = row.DataContext;
        this.SelectedItem = item;
        e.Handled = true;

        this.Dispatcher.BeginInvoke(
            new Action(() => this.TryExecuteEditItemCommand(item, restoreKeyboardFocus: true)),
            DispatcherPriority.ContextIdle);
    }

    private void TryExecuteEditItemCommand(object item, bool restoreKeyboardFocus)
    {
        if (this.EditItemCommand is null || !this.EditItemCommand.CanExecute(item))
        {
            return;
        }

        this.EditItemCommand.Execute(item);

        if (restoreKeyboardFocus)
        {
            this.RestoreKeyboardFocus();
        }
    }

    private static bool IsInteractiveDoubleClickSource(DependencyObject source)
    {
        return FindVisualParent<ButtonBase>(source) is not null ||
            FindVisualParent<TextBoxBase>(source) is not null ||
            FindVisualParent<ComboBox>(source) is not null ||
            FindVisualParent<DataGridColumnHeader>(source) is not null ||
            FindVisualParent<ScrollBar>(source) is not null;
    }
    #endregion

    #region ### Private Methods (Focus) ###
    private void RestoreKeyboardFocus()
    {
        this.Dispatcher.BeginInvoke(
            new Action(() =>
            {
                if (!this.IsVisible || !this.IsEnabled || !this.Focusable)
                {
                    return;
                }

                this.Focus();

                object? selectedItem = this.SelectedItem;
                if (selectedItem is not null &&
                    !ReferenceEquals(selectedItem, CollectionView.NewItemPlaceholder) &&
                    this.Items.Contains(selectedItem))
                {
                    this.ScrollIntoView(selectedItem);
                }

                Keyboard.Focus(this);
            }),
            DispatcherPriority.ApplicationIdle);
    }
    #endregion
}
