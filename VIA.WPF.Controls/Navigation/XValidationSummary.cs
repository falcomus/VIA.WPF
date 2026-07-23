// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationSummary.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using VIA.WPF.MVVM;

namespace VIA.WPF.Controls;

#region ### Class XValidationSummary ###
/// <summary>
/// Displays validation messages for an <see cref="INotifyDataErrorInfo"/> source.
/// </summary>
public class XValidationSummary : ItemsControl
{
    #region ### Fields ###
    private readonly ObservableCollection<XValidationError> messages = [];
    #endregion

    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Source"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
        nameof(Source),
        typeof(object),
        typeof(XValidationSummary),
        new FrameworkPropertyMetadata(null, OnSourceChanged));

    /// <summary>
    /// Identifies the <see cref="Header"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(string),
        typeof(XValidationSummary),
        new FrameworkPropertyMetadata("Validation messages"));

    /// <summary>
    /// Identifies the <see cref="IsCollapsible"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsCollapsibleProperty = DependencyProperty.Register(
        nameof(IsCollapsible),
        typeof(bool),
        typeof(XValidationSummary),
        new FrameworkPropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="IsExpanded"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
        nameof(IsExpanded),
        typeof(bool),
        typeof(XValidationSummary),
        new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    /// <summary>
    /// Identifies the <see cref="IncludeWarnings"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IncludeWarningsProperty = DependencyProperty.Register(
        nameof(IncludeWarnings),
        typeof(bool),
        typeof(XValidationSummary),
        new FrameworkPropertyMetadata(true, OnFilterChanged));

    /// <summary>
    /// Identifies the <see cref="IncludeInformation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IncludeInformationProperty = DependencyProperty.Register(
        nameof(IncludeInformation),
        typeof(bool),
        typeof(XValidationSummary),
        new FrameworkPropertyMetadata(false, OnFilterChanged));

    private static readonly DependencyPropertyKey HasMessagesPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(HasMessages),
        typeof(bool),
        typeof(XValidationSummary),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the read-only <see cref="HasMessages"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasMessagesProperty = HasMessagesPropertyKey.DependencyProperty;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XValidationSummary"/> class.
    /// </summary>
    static XValidationSummary()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(XValidationSummary), new FrameworkPropertyMetadata(typeof(XValidationSummary)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XValidationSummary"/> class.
    /// </summary>
    public XValidationSummary()
    {
        XValidationAdornerHelper.SuppressDefaultErrorTemplate(this);
        this.SetCurrentValue(ItemsSourceProperty, this.messages);
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the validation source.
    /// </summary>
    public object? Source
    {
        get => this.GetValue(SourceProperty);
        set => this.SetValue(SourceProperty, value);
    }

    /// <summary>
    /// Gets or sets the summary header.
    /// </summary>
    public string? Header
    {
        get => (string?)this.GetValue(HeaderProperty);
        set => this.SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the summary can be collapsed.
    /// </summary>
    public bool IsCollapsible
    {
        get => (bool)this.GetValue(IsCollapsibleProperty);
        set => this.SetValue(IsCollapsibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the summary content is expanded.
    /// </summary>
    public bool IsExpanded
    {
        get => (bool)this.GetValue(IsExpandedProperty);
        set => this.SetValue(IsExpandedProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether warnings are displayed.
    /// </summary>
    public bool IncludeWarnings
    {
        get => (bool)this.GetValue(IncludeWarningsProperty);
        set => this.SetValue(IncludeWarningsProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether informational messages are displayed.
    /// </summary>
    public bool IncludeInformation
    {
        get => (bool)this.GetValue(IncludeInformationProperty);
        set => this.SetValue(IncludeInformationProperty, value);
    }

    /// <summary>
    /// Gets a value indicating whether the summary contains messages.
    /// </summary>
    public bool HasMessages => (bool)this.GetValue(HasMessagesProperty);
    #endregion

    #region ### Private Methods ###
    private static void OnSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        XValidationSummary summary = (XValidationSummary)dependencyObject;
        summary.DetachSource(e.OldValue);
        summary.AttachSource(e.NewValue);
        summary.RefreshMessages();
    }

    private static void OnFilterChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        ((XValidationSummary)dependencyObject).RefreshMessages();
    }

    private void AttachSource(object? source)
    {
        if (source is INotifyDataErrorInfo notifyDataErrorInfo)
        {
            WeakEventManager<INotifyDataErrorInfo, DataErrorsChangedEventArgs>.AddHandler(
                notifyDataErrorInfo,
                nameof(INotifyDataErrorInfo.ErrorsChanged),
                this.OnSourceErrorsChanged);
        }

        if (source is INotifyPropertyChanged notifyPropertyChanged)
        {
            WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.AddHandler(
                notifyPropertyChanged,
                nameof(INotifyPropertyChanged.PropertyChanged),
                this.OnSourcePropertyChanged);
        }
    }

    private void DetachSource(object? source)
    {
        if (source is INotifyDataErrorInfo notifyDataErrorInfo)
        {
            WeakEventManager<INotifyDataErrorInfo, DataErrorsChangedEventArgs>.RemoveHandler(
                notifyDataErrorInfo,
                nameof(INotifyDataErrorInfo.ErrorsChanged),
                this.OnSourceErrorsChanged);
        }

        if (source is INotifyPropertyChanged notifyPropertyChanged)
        {
            WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.RemoveHandler(
                notifyPropertyChanged,
                nameof(INotifyPropertyChanged.PropertyChanged),
                this.OnSourcePropertyChanged);
        }
    }

    private void OnSourceErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        this.RefreshMessages();
    }

    private void OnSourcePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.PropertyName) ||
            e.PropertyName is XValidatableObject.ValidationMessagesPropertyName
                or XValidatableObject.ValidationErrorsPropertyName
                or XValidatableObject.HasErrorsPropertyName
                or XValidatableObject.IsValidPropertyName)
        {
            this.RefreshMessages();
        }
    }

    private void RefreshMessages()
    {
        IEnumerable<XValidationError> sourceMessages = this.GetSourceMessages()
            .Where(this.ShouldDisplayMessage)
            .ToArray();

        this.messages.Clear();

        foreach (XValidationError message in sourceMessages)
        {
            this.messages.Add(message);
        }

        this.SetValue(HasMessagesPropertyKey, this.messages.Count > 0);
    }

    private IEnumerable<XValidationError> GetSourceMessages()
    {
        return this.Source switch
        {
            null => [],
            XValidatableObject validatableObject => validatableObject.ValidationMessages,
            IEnumerable<XValidationError> validationMessages => validationMessages,
            INotifyDataErrorInfo notifyDataErrorInfo => GetNotifyDataErrorMessages(notifyDataErrorInfo),
            _ => []
        };
    }

    private bool ShouldDisplayMessage(XValidationError message)
    {
        return message.Severity switch
        {
            XValidationSeverity.Error => true,
            XValidationSeverity.Warning => this.IncludeWarnings,
            XValidationSeverity.Information => this.IncludeInformation,
            _ => true
        };
    }

    private static IEnumerable<XValidationError> GetNotifyDataErrorMessages(INotifyDataErrorInfo notifyDataErrorInfo)
    {
        IEnumerable errors = notifyDataErrorInfo.GetErrors(null);

        foreach (object? error in errors)
        {
            if (error is XValidationError validationError)
            {
                yield return validationError;
                continue;
            }

            string message = error?.ToString() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(message))
            {
                yield return new XValidationError(XValidationText.Text(message), XValidationSeverity.Error);
            }
        }
    }
    #endregion
}
#endregion
