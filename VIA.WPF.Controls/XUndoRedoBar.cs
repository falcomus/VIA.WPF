using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VIA.WPF.Controls;

/// <summary>
/// Provides compact, command-driven undo and redo actions for workbench toolbars.
/// </summary>
public class XUndoRedoBar : Control
{
    public static readonly DependencyProperty UndoCommandProperty =
        DependencyProperty.Register(nameof(UndoCommand), typeof(ICommand), typeof(XUndoRedoBar));

    public static readonly DependencyProperty RedoCommandProperty =
        DependencyProperty.Register(nameof(RedoCommand), typeof(ICommand), typeof(XUndoRedoBar));

    public static readonly DependencyProperty UndoToolTipProperty =
        DependencyProperty.Register(nameof(UndoToolTip), typeof(string), typeof(XUndoRedoBar), new PropertyMetadata("Undo (Ctrl+Z)"));

    public static readonly DependencyProperty RedoToolTipProperty =
        DependencyProperty.Register(nameof(RedoToolTip), typeof(string), typeof(XUndoRedoBar), new PropertyMetadata("Redo (Ctrl+Y)"));

    static XUndoRedoBar()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(XUndoRedoBar), new FrameworkPropertyMetadata(typeof(XUndoRedoBar)));
    }

    public ICommand? UndoCommand { get => (ICommand?)GetValue(UndoCommandProperty); set => SetValue(UndoCommandProperty, value); }

    public ICommand? RedoCommand { get => (ICommand?)GetValue(RedoCommandProperty); set => SetValue(RedoCommandProperty, value); }

    public string UndoToolTip { get => (string)GetValue(UndoToolTipProperty); set => SetValue(UndoToolTipProperty, value); }

    public string RedoToolTip { get => (string)GetValue(RedoToolTipProperty); set => SetValue(RedoToolTipProperty, value); }
}
