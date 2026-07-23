// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationHintPopup.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using VIA.WPF.MVVM;

namespace VIA.WPF.Controls;

#region ### Class XValidationHintPopup ###
/// <summary>
/// Displays a compact validation state indicator and exposes current validation messages in a popup.
/// </summary>
/// <remarks>
/// The popup can read messages from <see cref="XValidatableObject"/>, an <see cref="IEnumerable{T}"/> of
/// <see cref="XValidationError"/> or a foreign <see cref="INotifyDataErrorInfo"/> implementation.
/// VIA.WPF validation sources keep severity information, while foreign WPF errors are defensively wrapped as
/// <see cref="XValidationSeverity.Error"/> messages.
/// </remarks>
public class XValidationHintPopup : ItemsControl
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
        typeof(XValidationHintPopup),
        new FrameworkPropertyMetadata(null, OnSourceChanged));

    /// <summary>
    /// Identifies the <see cref="Header"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(string),
        typeof(XValidationHintPopup),
        new FrameworkPropertyMetadata("Validation"));

    /// <summary>
    /// Identifies the <see cref="IncludeWarnings"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IncludeWarningsProperty = DependencyProperty.Register(
        nameof(IncludeWarnings),
        typeof(bool),
        typeof(XValidationHintPopup),
        new FrameworkPropertyMetadata(true, OnFilterChanged));

    /// <summary>
    /// Identifies the <see cref="IncludeInformation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IncludeInformationProperty = DependencyProperty.Register(
        nameof(IncludeInformation),
        typeof(bool),
        typeof(XValidationHintPopup),
        new FrameworkPropertyMetadata(false, OnFilterChanged));

    /// <summary>
    /// Identifies the <see cref="ShowWhenValid"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowWhenValidProperty = DependencyProperty.Register(
        nameof(ShowWhenValid),
        typeof(bool),
        typeof(XValidationHintPopup),
        new FrameworkPropertyMetadata(false));

    private static readonly DependencyPropertyKey HasMessagesPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(HasMessages),
        typeof(bool),
        typeof(XValidationHintPopup),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the read-only <see cref="HasMessages"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasMessagesProperty = HasMessagesPropertyKey.DependencyProperty;

    private static readonly DependencyPropertyKey HasErrorsPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(HasErrors),
        typeof(bool),
        typeof(XValidationHintPopup),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the read-only <see cref="HasErrors"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasErrorsProperty = HasErrorsPropertyKey.DependencyProperty;

    private static readonly DependencyPropertyKey HasWarningsPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(HasWarnings),
        typeof(bool),
        typeof(XValidationHintPopup),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the read-only <see cref="HasWarnings"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasWarningsProperty = HasWarningsPropertyKey.DependencyProperty;

    private static readonly DependencyPropertyKey HasInformationPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(HasInformation),
        typeof(bool),
        typeof(XValidationHintPopup),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the read-only <see cref="HasInformation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasInformationProperty = HasInformationPropertyKey.DependencyProperty;

    private static readonly DependencyPropertyKey MessageCountPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(MessageCount),
        typeof(int),
        typeof(XValidationHintPopup),
        new FrameworkPropertyMetadata(0));

    /// <summary>
    /// Identifies the read-only <see cref="MessageCount"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MessageCountProperty = MessageCountPropertyKey.DependencyProperty;

    private static readonly DependencyPropertyKey ErrorCountPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(ErrorCount),
        typeof(int),
        typeof(XValidationHintPopup),
        new FrameworkPropertyMetadata(0));

    /// <summary>
    /// Identifies the read-only <see cref="ErrorCount"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ErrorCountProperty = ErrorCountPropertyKey.DependencyProperty;

    private static readonly DependencyPropertyKey WarningCountPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(WarningCount),
        typeof(int),
        typeof(XValidationHintPopup),
        new FrameworkPropertyMetadata(0));

    /// <summary>
    /// Identifies the read-only <see cref="WarningCount"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty WarningCountProperty = WarningCountPropertyKey.DependencyProperty;

    private static readonly DependencyPropertyKey InformationCountPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(InformationCount),
        typeof(int),
        typeof(XValidationHintPopup),
        new FrameworkPropertyMetadata(0));

    /// <summary>
    /// Identifies the read-only <see cref="InformationCount"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty InformationCountProperty = InformationCountPropertyKey.DependencyProperty;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XValidationHintPopup"/> class.
    /// </summary>
    static XValidationHintPopup()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(XValidationHintPopup), new FrameworkPropertyMetadata(typeof(XValidationHintPopup)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XValidationHintPopup"/> class.
    /// </summary>
    public XValidationHintPopup()
    {
        this.SetCurrentValue(ItemsSourceProperty, this.messages);
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the validation source.
    /// </summary>
    /// <remarks>
    /// Supported sources are <see cref="XValidatableObject"/>, <see cref="IEnumerable{T}"/> of
    /// <see cref="XValidationError"/> and <see cref="INotifyDataErrorInfo"/>. Sources implementing
    /// <see cref="INotifyPropertyChanged"/> or <see cref="INotifyDataErrorInfo"/> are observed with weak events.
    /// </remarks>
    public object? Source
    {
        get => this.GetValue(SourceProperty);
        set => this.SetValue(SourceProperty, value);
    }

    /// <summary>
    /// Gets or sets the popup header.
    /// </summary>
    public string? Header
    {
        get => (string?)this.GetValue(HeaderProperty);
        set => this.SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether warnings are displayed.
    /// </summary>
    /// <remarks>
    /// Warnings are an VIA.WPF validation extension. Standard WPF <see cref="INotifyDataErrorInfo"/> errors are treated as errors.
    /// </remarks>
    public bool IncludeWarnings
    {
        get => (bool)this.GetValue(IncludeWarningsProperty);
        set => this.SetValue(IncludeWarningsProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether informational messages are displayed.
    /// </summary>
    /// <remarks>
    /// Informational messages are an VIA.WPF validation extension. They are hidden by default.
    /// </remarks>
    public bool IncludeInformation
    {
        get => (bool)this.GetValue(IncludeInformationProperty);
        set => this.SetValue(IncludeInformationProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the indicator is shown when no validation messages are available.
    /// </summary>
    public bool ShowWhenValid
    {
        get => (bool)this.GetValue(ShowWhenValidProperty);
        set => this.SetValue(ShowWhenValidProperty, value);
    }

    /// <summary>
    /// Gets a value indicating whether the popup contains messages.
    /// </summary>
    public bool HasMessages => (bool)this.GetValue(HasMessagesProperty);

    /// <summary>
    /// Gets a value indicating whether the popup contains error messages.
    /// </summary>
    public bool HasErrors => (bool)this.GetValue(HasErrorsProperty);

    /// <summary>
    /// Gets a value indicating whether the popup contains warning messages.
    /// </summary>
    public bool HasWarnings => (bool)this.GetValue(HasWarningsProperty);

    /// <summary>
    /// Gets a value indicating whether the popup contains informational messages.
    /// </summary>
    public bool HasInformation => (bool)this.GetValue(HasInformationProperty);

    /// <summary>
    /// Gets the number of displayed validation messages.
    /// </summary>
    public int MessageCount => (int)this.GetValue(MessageCountProperty);

    /// <summary>
    /// Gets the number of displayed validation errors.
    /// </summary>
    public int ErrorCount => (int)this.GetValue(ErrorCountProperty);

    /// <summary>
    /// Gets the number of displayed validation warnings.
    /// </summary>
    public int WarningCount => (int)this.GetValue(WarningCountProperty);

    /// <summary>
    /// Gets the number of displayed informational validation messages.
    /// </summary>
    public int InformationCount => (int)this.GetValue(InformationCountProperty);
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (this.GetTemplateChild("PART_Popup") is Popup popup)
        {
            popup.CustomPopupPlacementCallback = PlacePopupRightAligned;
        }
    }
    #endregion

    #region ### Private Methods ###
    private static CustomPopupPlacement[] PlacePopupRightAligned(Size popupSize, Size targetSize, Point offset)
    {
        return
        [
            new CustomPopupPlacement(
                new Point(targetSize.Width - popupSize.Width, targetSize.Height + 6d),
                PopupPrimaryAxis.Horizontal)
        ];
    }

    private static void OnSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        XValidationHintPopup popup = (XValidationHintPopup)dependencyObject;
        popup.DetachSource(e.OldValue);
        popup.AttachSource(e.NewValue);
        popup.RefreshMessages();
    }

    private static void OnFilterChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        ((XValidationHintPopup)dependencyObject).RefreshMessages();
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
        XValidationError[] sourceMessages = [.. this.GetSourceMessages().Where(this.ShouldDisplayMessage)];

        this.messages.Clear();

        foreach (XValidationError message in sourceMessages)
        {
            this.messages.Add(message);
        }

        int errorCount = 0;
        int warningCount = 0;
        int informationCount = 0;

        foreach (XValidationError message in sourceMessages)
        {
            switch (message.Severity)
            {
                case XValidationSeverity.Error:
                    errorCount++;
                    break;

                case XValidationSeverity.Warning:
                    warningCount++;
                    break;

                case XValidationSeverity.Information:
                    informationCount++;
                    break;
            }
        }

        this.SetValue(MessageCountPropertyKey, sourceMessages.Length);
        this.SetValue(ErrorCountPropertyKey, errorCount);
        this.SetValue(WarningCountPropertyKey, warningCount);
        this.SetValue(InformationCountPropertyKey, informationCount);
        this.SetValue(HasMessagesPropertyKey, sourceMessages.Length > 0);
        this.SetValue(HasErrorsPropertyKey, errorCount > 0);
        this.SetValue(HasWarningsPropertyKey, warningCount > 0);
        this.SetValue(HasInformationPropertyKey, informationCount > 0);
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
