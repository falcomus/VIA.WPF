// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XCheckGroupDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XCheckGroupDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XCheckGroup showcase page.
/// </summary>
public sealed class XCheckGroupDemoViewModel : DemoPageViewModel
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XCheckGroupDemoViewModel"/> class.
    /// </summary>
    public XCheckGroupDemoViewModel()
    {
        NotificationOptions.Add(new CheckGroupOption("Email", "Send important updates to the mailbox."));
        NotificationOptions.Add(new CheckGroupOption("Desktop", "Show local toast notifications."));
        NotificationOptions.Add(new CheckGroupOption("SMS", "Use only for urgent alerts."));
        NotificationOptions.Add(new CheckGroupOption("Weekly report", "Add a compact status digest every Monday."));

        SelectedNotificationOptions.Add(NotificationOptions[0]);
        SelectedNotificationOptions.Add(NotificationOptions[1]);

        SelectedNotificationOptions.CollectionChanged += OnSelectedNotificationOptionsChanged;
    }
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XCheckGroup";

    /// <inheritdoc/>
    public override string Description => "Demonstrates titled multi-selection groups with descriptions, variants, shared sizing and MVVM-backed selected items.";

    /// <summary>
    /// Gets the selectable notification options used by the MVVM sample.
    /// </summary>
    public ObservableCollection<CheckGroupOption> NotificationOptions { get; } = new();

    /// <summary>
    /// Gets the selected notification options used by the MVVM sample.
    /// </summary>
    public ObservableCollection<object> SelectedNotificationOptions { get; } = new();

    /// <summary>
    /// Gets a short summary of the current MVVM selection.
    /// </summary>
    public string NotificationSummary
    {
        get
        {
            if (SelectedNotificationOptions.Count == 0)
            {
                return "No notification channels are selected.";
            }

            string selectedOptions = string.Join(", ", SelectedNotificationOptions.OfType<CheckGroupOption>().Select(option => option.Name));
            return $"{SelectedNotificationOptions.Count} of {NotificationOptions.Count} channels selected: {selectedOptions}.";
        }
    }

    /// <inheritdoc/>
    public override string XamlCode => """
<!-- Descriptive multi-selection group -->
<via:XCheckGroup Header="Notification channels" ItemSpacing="8" ItemsPadding="12">
    <via:XCheckGroupItem Content="Email" Description="Send important updates to the mailbox." IsChecked="True" Variant="Primary" />
    <via:XCheckGroupItem Content="Desktop" Description="Show local toast notifications." IsChecked="True" Variant="Info" />
    <via:XCheckGroupItem Content="SMS" Description="Use only for urgent alerts." Variant="Warning" />
</via:XCheckGroup>

<!-- Compact horizontal filters -->
<via:XCheckGroup Orientation="Horizontal" ShowTitle="False" ItemsPadding="12,5" ItemSpacing="24">
    <via:XCheckGroupItem Content="Open" IsChecked="True" />
    <via:XCheckGroupItem Content="Assigned" />
    <via:XCheckGroupItem Content="Overdue" Variant="Danger" />
</via:XCheckGroup>

<!-- MVVM selected items -->
<via:XCheckGroup
    Title="Bound notification options"
    ItemsSource="{Binding NotificationOptions}"
    SelectedItems="{Binding SelectedNotificationOptions}"
    ItemSpacing="8">
    <via:XCheckGroup.ItemTemplate>
        <DataTemplate>
            <StackPanel>
                <TextBlock FontWeight="SemiBold" Text="{Binding Name}" />
                <TextBlock Text="{Binding Description}" TextWrapping="Wrap" />
            </StackPanel>
        </DataTemplate>
    </via:XCheckGroup.ItemTemplate>
</via:XCheckGroup>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
public ObservableCollection<CheckGroupOption> NotificationOptions { get; } = new();
public ObservableCollection<object> SelectedNotificationOptions { get; } = new();

public XCheckGroupDemoViewModel()
{
    NotificationOptions.Add(new CheckGroupOption("Email", "Send important updates to the mailbox."));
    NotificationOptions.Add(new CheckGroupOption("Desktop", "Show local toast notifications."));
    NotificationOptions.Add(new CheckGroupOption("SMS", "Use only for urgent alerts."));

    SelectedNotificationOptions.Add(NotificationOptions[0]);
    SelectedNotificationOptions.CollectionChanged += (_, _) => OnPropertyChanged(nameof(NotificationSummary));
}

public string NotificationSummary => $"{SelectedNotificationOptions.Count} channels selected.";

public sealed record CheckGroupOption(string Name, string Description);
""";
    #endregion

    #region ### Private Methods ###
    private void OnSelectedNotificationOptionsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(NotificationSummary));
    }
    #endregion
}
#endregion

#region ### Record CheckGroupOption ###
/// <summary>
/// Represents one option used by the XCheckGroup MVVM sample.
/// </summary>
/// <param name="Name">The display name.</param>
/// <param name="Description">The secondary description.</param>
public sealed record CheckGroupOption(string Name, string Description);
#endregion
