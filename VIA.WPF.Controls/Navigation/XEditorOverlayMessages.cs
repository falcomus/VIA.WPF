// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XEditorOverlayMessages.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace VIA.WPF.Controls.Navigation;

#region ### Class ShowEditorOverlayMessage ###
/// <summary>
/// Requests that the current <see cref="VIA.WPF.Controls.XViewContainer" /> detail editor is displayed by the owning window overlay host.
/// </summary>
public sealed class ShowEditorOverlayMessage
{
    #region ### Fields ###
    private readonly Action closeAction;
    private readonly Func<bool> canCloseOnOverlayClick;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="ShowEditorOverlayMessage" /> class.
    /// </summary>
    /// <param name="owner">The source view container.</param>
    /// <param name="targetWindow">The window that should host the editor overlay.</param>
    /// <param name="header">The editor header content.</param>
    /// <param name="headerTemplate">The editor header template.</param>
    /// <param name="content">The editor content.</param>
    /// <param name="contentTemplate">The editor content template.</param>
    /// <param name="footer">The optional custom editor footer.</param>
    /// <param name="footerTemplate">The optional custom editor footer template.</param>
    /// <param name="validationSource">The validation source shown in the compact validation hint.</param>
    /// <param name="showValidationHint">Whether the compact validation hint is shown.</param>
    /// <param name="overlayBackground">The overlay background brush.</param>
    /// <param name="overlayOpacity">The overlay opacity.</param>
    /// <param name="overlayCornerRadius">The overlay corner radius.</param>
    /// <param name="detailBackground">The dialog background brush.</param>
    /// <param name="detailBorderBrush">The dialog border brush.</param>
    /// <param name="detailBorderThickness">The dialog border thickness.</param>
    /// <param name="detailCornerRadius">The dialog corner radius.</param>
    /// <param name="detailWidth">The dialog width.</param>
    /// <param name="detailMinWidth">The dialog minimum width.</param>
    /// <param name="detailMaxWidth">The dialog maximum width.</param>
    /// <param name="detailMinHeight">The dialog minimum height.</param>
    /// <param name="detailMaxHeight">The dialog maximum height.</param>
    /// <param name="detailMargin">The dialog margin.</param>
    /// <param name="detailPadding">The dialog content padding.</param>
    /// <param name="detailHeaderPadding">The dialog header padding.</param>
    /// <param name="detailFooterPadding">The dialog footer padding.</param>
    /// <param name="showCloseButton">Whether the built-in close button is shown.</param>
    /// <param name="isModal">Whether the editor is modal.</param>
    /// <param name="showDefaultFooter">Whether the default footer should be shown.</param>
    /// <param name="primaryCommand">The primary command.</param>
    /// <param name="primaryCommandParameter">The primary command parameter.</param>
    /// <param name="primaryText">The primary button text.</param>
    /// <param name="cancelText">The cancel button text.</param>
    /// <param name="closeAction">The action that closes the source detail editor.</param>
    /// <param name="canCloseOnOverlayClick">The predicate that decides whether overlay clicks may close the editor.</param>
    public ShowEditorOverlayMessage(
        VIA.WPF.Controls.XViewContainer owner,
        Window targetWindow,
        object? header,
        DataTemplate? headerTemplate,
        object? content,
        DataTemplate? contentTemplate,
        object? footer,
        DataTemplate? footerTemplate,
        object? validationSource,
        bool showValidationHint,
        Brush overlayBackground,
        double overlayOpacity,
        CornerRadius overlayCornerRadius,
        Brush? detailBackground,
        Brush? detailBorderBrush,
        Thickness detailBorderThickness,
        CornerRadius detailCornerRadius,
        double detailWidth,
        double detailMinWidth,
        double detailMaxWidth,
        double detailMinHeight,
        double detailMaxHeight,
        Thickness detailMargin,
        Thickness detailPadding,
        Thickness detailHeaderPadding,
        Thickness detailFooterPadding,
        bool showCloseButton,
        bool isModal,
        bool showDefaultFooter,
        ICommand? primaryCommand,
        object? primaryCommandParameter,
        string primaryText,
        string cancelText,
        Action closeAction,
        Func<bool> canCloseOnOverlayClick)
    {
        this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        this.TargetWindow = targetWindow ?? throw new ArgumentNullException(nameof(targetWindow));
        this.Header = header;
        this.HeaderTemplate = headerTemplate;
        this.Content = content;
        this.ContentTemplate = contentTemplate;
        this.Footer = footer;
        this.FooterTemplate = footerTemplate;
        this.ValidationSource = validationSource;
        this.ShowValidationHint = showValidationHint;
        this.OverlayBackground = overlayBackground;
        this.OverlayOpacity = overlayOpacity;
        this.OverlayCornerRadius = overlayCornerRadius;
        this.DetailBackground = detailBackground;
        this.DetailBorderBrush = detailBorderBrush;
        this.DetailBorderThickness = detailBorderThickness;
        this.DetailCornerRadius = detailCornerRadius;
        this.DetailWidth = detailWidth;
        this.DetailMinWidth = detailMinWidth;
        this.DetailMaxWidth = detailMaxWidth;
        this.DetailMinHeight = detailMinHeight;
        this.DetailMaxHeight = detailMaxHeight;
        this.DetailMargin = detailMargin;
        this.DetailPadding = detailPadding;
        this.DetailHeaderPadding = detailHeaderPadding;
        this.DetailFooterPadding = detailFooterPadding;
        this.ShowCloseButton = showCloseButton;
        this.IsModal = isModal;
        this.ShowDefaultFooter = showDefaultFooter;
        this.PrimaryCommand = primaryCommand;
        this.PrimaryCommandParameter = primaryCommandParameter;
        this.PrimaryText = primaryText;
        this.CancelText = cancelText;
        this.closeAction = closeAction ?? throw new ArgumentNullException(nameof(closeAction));
        this.canCloseOnOverlayClick = canCloseOnOverlayClick ?? throw new ArgumentNullException(nameof(canCloseOnOverlayClick));
        this.CloseCommand = new EditorOverlayCloseCommand(this);
        this.CloseCommandParameter = this;
    }
    #endregion

    #region ### Properties ###
    /// <summary>
    /// Gets the source view container.
    /// </summary>
    public VIA.WPF.Controls.XViewContainer Owner { get; }

    /// <summary>
    /// Gets the target window.
    /// </summary>
    public Window TargetWindow { get; }

    /// <summary>
    /// Gets or sets a value indicating whether a window overlay host handled the request.
    /// </summary>
    public bool Handled { get; set; }

    /// <summary>
    /// Gets the editor header content.
    /// </summary>
    public object? Header { get; }

    /// <summary>
    /// Gets the editor header template.
    /// </summary>
    public DataTemplate? HeaderTemplate { get; }

    /// <summary>
    /// Gets the editor content.
    /// </summary>
    public object? Content { get; }

    /// <summary>
    /// Gets the editor content template.
    /// </summary>
    public DataTemplate? ContentTemplate { get; }

    /// <summary>
    /// Gets the custom footer content.
    /// </summary>
    public object? Footer { get; }

    /// <summary>
    /// Gets the custom footer template.
    /// </summary>
    public DataTemplate? FooterTemplate { get; }

    /// <summary>
    /// Gets the validation source.
    /// </summary>
    public object? ValidationSource { get; }

    /// <summary>
    /// Gets a value indicating whether the validation hint is shown.
    /// </summary>
    public bool ShowValidationHint { get; }

    /// <summary>
    /// Gets the overlay background brush.
    /// </summary>
    public Brush OverlayBackground { get; }

    /// <summary>
    /// Gets the overlay opacity.
    /// </summary>
    public double OverlayOpacity { get; }

    /// <summary>
    /// Gets the overlay corner radius.
    /// </summary>
    public CornerRadius OverlayCornerRadius { get; }

    /// <summary>
    /// Gets the dialog background brush.
    /// </summary>
    public Brush? DetailBackground { get; }

    /// <summary>
    /// Gets the dialog border brush.
    /// </summary>
    public Brush? DetailBorderBrush { get; }

    /// <summary>
    /// Gets the dialog border thickness.
    /// </summary>
    public Thickness DetailBorderThickness { get; }

    /// <summary>
    /// Gets the dialog corner radius.
    /// </summary>
    public CornerRadius DetailCornerRadius { get; }

    /// <summary>
    /// Gets the dialog width.
    /// </summary>
    public double DetailWidth { get; }

    /// <summary>
    /// Gets the dialog minimum width.
    /// </summary>
    public double DetailMinWidth { get; }

    /// <summary>
    /// Gets the dialog maximum width.
    /// </summary>
    public double DetailMaxWidth { get; }

    /// <summary>
    /// Gets the dialog minimum height.
    /// </summary>
    public double DetailMinHeight { get; }

    /// <summary>
    /// Gets the dialog maximum height.
    /// </summary>
    public double DetailMaxHeight { get; }

    /// <summary>
    /// Gets the dialog margin.
    /// </summary>
    public Thickness DetailMargin { get; }

    /// <summary>
    /// Gets the dialog content padding.
    /// </summary>
    public Thickness DetailPadding { get; }

    /// <summary>
    /// Gets the dialog header padding.
    /// </summary>
    public Thickness DetailHeaderPadding { get; }

    /// <summary>
    /// Gets the dialog footer padding.
    /// </summary>
    public Thickness DetailFooterPadding { get; }

    /// <summary>
    /// Gets a value indicating whether the close button is shown.
    /// </summary>
    public bool ShowCloseButton { get; }

    /// <summary>
    /// Gets a value indicating whether the editor is modal.
    /// </summary>
    public bool IsModal { get; }

    /// <summary>
    /// Gets a value indicating whether the default footer should be shown.
    /// </summary>
    public bool ShowDefaultFooter { get; }

    /// <summary>
    /// Gets the primary command.
    /// </summary>
    public ICommand? PrimaryCommand { get; }

    /// <summary>
    /// Gets the primary command parameter.
    /// </summary>
    public object? PrimaryCommandParameter { get; }

    /// <summary>
    /// Gets the primary button text.
    /// </summary>
    public string PrimaryText { get; }

    /// <summary>
    /// Gets the cancel button text.
    /// </summary>
    public string CancelText { get; }

    /// <summary>
    /// Gets the command that closes the source editor.
    /// </summary>
    public ICommand CloseCommand { get; }

    /// <summary>
    /// Gets the command parameter used by <see cref="CloseCommand" />.
    /// </summary>
    public object? CloseCommandParameter { get; }

    /// <summary>
    /// Gets a value indicating whether the editor has header content.
    /// </summary>
    public bool HasHeader => HasMeaningfulContent(this.Header);

    /// <summary>
    /// Gets a value indicating whether the editor header row should be shown.
    /// </summary>
    public bool HasHeaderChrome => this.HasHeader || this.ShowCloseButton || this.ShowValidationHint;

    /// <summary>
    /// Gets a value indicating whether the editor has a custom footer.
    /// </summary>
    public bool HasFooter => HasMeaningfulContent(this.Footer);

    /// <summary>
    /// Gets a value indicating whether the primary command is available.
    /// </summary>
    public bool HasPrimaryCommand => this.PrimaryCommand is not null;

    /// <summary>
    /// Gets a value indicating whether an overlay click may close the editor now.
    /// </summary>
    public bool CanCloseOnOverlayClick => this.canCloseOnOverlayClick();
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Requests that the source editor closes.
    /// </summary>
    public void Close()
    {
        this.closeAction();
    }
    #endregion

    #region ### Private Classes ###
    /// <summary>
    /// Provides a small command wrapper around the owner message close action.
    /// </summary>
    private sealed class EditorOverlayCloseCommand : ICommand
    {
        #region ### Fields ###
        private readonly ShowEditorOverlayMessage message;
        #endregion

        #region ### Constructors ###
        /// <summary>
        /// Initializes a new instance of the <see cref="EditorOverlayCloseCommand" /> class.
        /// </summary>
        /// <param name="message">The owning message.</param>
        public EditorOverlayCloseCommand(ShowEditorOverlayMessage message)
        {
            this.message = message ?? throw new ArgumentNullException(nameof(message));
        }
        #endregion

        #region ### Events ###
        /// <inheritdoc />
        public event EventHandler? CanExecuteChanged
        {
            add { }
            remove { }
        }
        #endregion

        #region ### Public Methods ###
        /// <inheritdoc />
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        /// <inheritdoc />
        public void Execute(object? parameter)
        {
            this.message.Close();
        }
        #endregion
    }
    #endregion

    #region ### Private Methods ###
    private static bool HasMeaningfulContent(object? value)
    {
        return value switch
        {
            null => false,
            string text => !string.IsNullOrWhiteSpace(text),
            _ => true,
        };
    }
    #endregion
}
#endregion

#region ### Class HideEditorOverlayMessage ###
/// <summary>
/// Requests that a previously shown window editor overlay is hidden.
/// </summary>
public sealed class HideEditorOverlayMessage
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="HideEditorOverlayMessage" /> class.
    /// </summary>
    /// <param name="owner">The source view container.</param>
    /// <param name="targetWindow">The optional window that currently hosts the overlay.</param>
    public HideEditorOverlayMessage(VIA.WPF.Controls.XViewContainer owner, Window? targetWindow)
    {
        this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        this.TargetWindow = targetWindow;
    }
    #endregion

    #region ### Properties ###
    /// <summary>
    /// Gets the source view container.
    /// </summary>
    public VIA.WPF.Controls.XViewContainer Owner { get; }

    /// <summary>
    /// Gets the optional target window.
    /// </summary>
    public Window? TargetWindow { get; }

    /// <summary>
    /// Gets or sets a value indicating whether a window overlay host handled the request.
    /// </summary>
    public bool Handled { get; set; }
    #endregion
}
#endregion
