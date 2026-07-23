// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XExpanderDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XExpanderDemoViewModel ###
/// <summary>
/// Represents the demo page view model for <c>XExpander</c>.
/// </summary>
public sealed class XExpanderDemoViewModel : DemoPageViewModel
{
    #region ### Private Fields ###
    private bool _detailsExpanded = true;
    private bool _generalSectionExpanded = true;
    private bool _securitySectionExpanded;
    private bool _notificationsSectionExpanded;
    private bool _isSynchronizingAccordion;
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XExpander";

    /// <inheritdoc/>
    public override string Description => "Demonstrates a themed expander with Size, HeaderIcon, ExpandDirection, ShowIndicator and two-way IsExpanded support.";

    /// <summary>
    /// Gets or sets a value indicating whether the details sample is expanded.
    /// </summary>
    public bool DetailsExpanded
    {
        get => _detailsExpanded;
        set
        {
            if (SetProperty(ref _detailsExpanded, value))
            {
                OnPropertyChanged(nameof(DetailsSummary));
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the general accordion section is expanded.
    /// </summary>
    public bool GeneralSectionExpanded
    {
        get => _generalSectionExpanded;
        set
        {
            if (SetProperty(ref _generalSectionExpanded, value))
            {
                if (value)
                {
                    CloseOtherAccordionSections(nameof(GeneralSectionExpanded));
                }

                OnPropertyChanged(nameof(AccordionSummary));
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the security accordion section is expanded.
    /// </summary>
    public bool SecuritySectionExpanded
    {
        get => _securitySectionExpanded;
        set
        {
            if (SetProperty(ref _securitySectionExpanded, value))
            {
                if (value)
                {
                    CloseOtherAccordionSections(nameof(SecuritySectionExpanded));
                }

                OnPropertyChanged(nameof(AccordionSummary));
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the notifications accordion section is expanded.
    /// </summary>
    public bool NotificationsSectionExpanded
    {
        get => _notificationsSectionExpanded;
        set
        {
            if (SetProperty(ref _notificationsSectionExpanded, value))
            {
                if (value)
                {
                    CloseOtherAccordionSections(nameof(NotificationsSectionExpanded));
                }

                OnPropertyChanged(nameof(AccordionSummary));
            }
        }
    }

    /// <summary>
    /// Gets a short summary for the details binding sample.
    /// </summary>
    public string DetailsSummary => DetailsExpanded
        ? "The detail panel is open."
        : "The detail panel is collapsed.";

    /// <summary>
    /// Gets a short summary for the accordion sample.
    /// </summary>
    public string AccordionSummary => GetOpenAccordionSection() is { } sectionName
        ? $"Open section: {sectionName}."
        : "No accordion section is open.";

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XExpander
    Header="General settings"
    HeaderIcon="{via:MaterialIcon Kind=Cog}"
    Size="Small"
    IsExpanded="True">
    <via:XStackPanel Spacing="8">
        <via:XCheckBox Content="Enable notifications" />
        <via:XCheckBox Content="Start automatically" />
    </via:XStackPanel>
</via:XExpander>

<via:XExpander
    Header="Project details"
    HeaderIcon="{via:MaterialIcon Kind=TextBoxSearchOutline}"
    IsExpanded="{Binding DetailsExpanded, Mode=TwoWay}">
    <TextBlock Text="The expanded state is bound to the view model." />
</via:XExpander>

<via:XExpander
    Header="Expand up"
    HeaderIcon="{via:MaterialIcon Kind=MenuUp}"
    ExpandDirection="Up"
    IsExpanded="True">
    <TextBlock Text="Useful in bottom panels, popups and tool windows." />
</via:XExpander>

<via:XExpander
    Header="Expand right"
    HeaderIcon="{via:MaterialIcon Kind=ChevronRight}"
    ExpandDirection="Right"
    IsExpanded="True">
    <TextBlock Text="Useful in inspector panels." />
</via:XExpander>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
private bool _detailsExpanded = true;

public bool DetailsExpanded
{
    get => _detailsExpanded;
    set
    {
        if (SetProperty(ref _detailsExpanded, value))
        {
            OnPropertyChanged(nameof(DetailsSummary));
        }
    }
}

public string DetailsSummary => DetailsExpanded
    ? "The detail panel is open."
    : "The detail panel is collapsed.";

XExpander sidePanel = new()
{
    Header = "Inspector",
    ExpandDirection = XExpandDirection.Right,
    IsExpanded = true,
};
""";
    #endregion

    #region ### Private Methods ###
    private void CloseOtherAccordionSections(string activePropertyName)
    {
        if (_isSynchronizingAccordion)
        {
            return;
        }

        _isSynchronizingAccordion = true;

        try
        {
            if (activePropertyName != nameof(GeneralSectionExpanded))
            {
                GeneralSectionExpanded = false;
            }

            if (activePropertyName != nameof(SecuritySectionExpanded))
            {
                SecuritySectionExpanded = false;
            }

            if (activePropertyName != nameof(NotificationsSectionExpanded))
            {
                NotificationsSectionExpanded = false;
            }
        }
        finally
        {
            _isSynchronizingAccordion = false;
        }
    }

    private string? GetOpenAccordionSection()
    {
        if (GeneralSectionExpanded)
        {
            return "General";
        }

        if (SecuritySectionExpanded)
        {
            return "Security";
        }

        if (NotificationsSectionExpanded)
        {
            return "Notifications";
        }

        return null;
    }
    #endregion
}
#endregion
