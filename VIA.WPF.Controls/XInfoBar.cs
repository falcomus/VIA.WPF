// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XInfoBar.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VIA.WPF.Controls;

#region ### Class XInfoBar ###
/// <summary>
/// Displays concise contextual information with optional actions and dismissal.
/// </summary>
public class XInfoBar : ContentControl
{
    #region ### Dependency Properties ###
    public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
        nameof(IsOpen), typeof(bool), typeof(XInfoBar), new FrameworkPropertyMetadata(true));

    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title), typeof(string), typeof(XInfoBar), new FrameworkPropertyMetadata(string.Empty));

    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
        nameof(Message), typeof(string), typeof(XInfoBar), new FrameworkPropertyMetadata(string.Empty));

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon), typeof(object), typeof(XInfoBar), new FrameworkPropertyMetadata(null));

    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
        nameof(Variant), typeof(XControlVariant), typeof(XInfoBar), new FrameworkPropertyMetadata(XControlVariant.Info));

    public static readonly DependencyProperty IsClosableProperty = DependencyProperty.Register(
        nameof(IsClosable), typeof(bool), typeof(XInfoBar), new FrameworkPropertyMetadata(false));
    #endregion

    #region ### Private Fields ###
    private readonly ICommand closeCommand;
    #endregion

    #region ### Constructors ###
    static XInfoBar()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(XInfoBar), new FrameworkPropertyMetadata(typeof(XInfoBar)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XInfoBar"/> class.
    /// </summary>
    public XInfoBar()
    {
        this.closeCommand = new CloseInfoBarCommand(this);
    }
    #endregion

    #region ### Public Properties ###
    public bool IsOpen
    {
        get => (bool)this.GetValue(IsOpenProperty);
        set => this.SetValue(IsOpenProperty, value);
    }

    public string Title
    {
        get => (string)this.GetValue(TitleProperty);
        set => this.SetValue(TitleProperty, value);
    }

    public string Message
    {
        get => (string)this.GetValue(MessageProperty);
        set => this.SetValue(MessageProperty, value);
    }

    public object? Icon
    {
        get => this.GetValue(IconProperty);
        set => this.SetValue(IconProperty, value);
    }

    public XControlVariant Variant
    {
        get => (XControlVariant)this.GetValue(VariantProperty);
        set => this.SetValue(VariantProperty, value);
    }

    public bool IsClosable
    {
        get => (bool)this.GetValue(IsClosableProperty);
        set => this.SetValue(IsClosableProperty, value);
    }

    /// <summary>Gets the command used by the template to close the information bar.</summary>
    public ICommand CloseCommand => this.closeCommand;
    #endregion

    #region ### Protected Methods ###
    /// <inheritdoc />
    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.Property == IsOpenProperty || e.Property == IsClosableProperty)
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
    #endregion

    #region ### Nested Types ###
    private sealed class CloseInfoBarCommand(XInfoBar owner) : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter) => owner.IsOpen && owner.IsClosable;

        public void Execute(object? parameter)
        {
            owner.SetCurrentValue(IsOpenProperty, false);
        }
    }
    #endregion
}
#endregion
