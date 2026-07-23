// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockupProjectViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace VIA.WPF.Mockup.Wpf.ViewModels;

/// <summary>
/// Provides the initial presentation state for the reusable Project page.
/// </summary>
public sealed partial class MockupProjectViewModel : ObservableObject
{
    public MockupProjectViewModel()
    {
        ProjectTargets = ["Desktop", "Mobile", "Both"];
        DevicePresets = ["Desktop (1920 × 1080)", "Desktop (1440 × 900)", "Phone (390 × 844)"];
        LayoutModes = ["Grid", "Free", "Responsive"];
        SnapSpacings = ["4 px", "8 px", "12 px", "16 px"];
        SaveBehaviors = ["Prompt on Save", "Save immediately", "Save copy first"];
        ExportTargets = ["Windows (Desktop)", "Mobile presentation", "Image package"];

        Screens =
        [
            CreateScreen("Home Screen", "Desktop", null, true, "Updated: Today, 9:41 AM", "Main landing screen showing featured categories and best sellers.", "Desktop (1920 × 1080)", "May 1, 2026, 10:12 AM", 12, 37, 99, 235),
            CreateScreen("Login Screen", "Desktop", "Draft", false, "Updated: May 7, 2026", "Authentication screen with account recovery entry points.", "Desktop (1440 × 900)", "May 2, 2026, 08:35 AM", 8, 96, 165, 250),
            CreateScreen("Product Details", "Desktop", null, false, "Updated: May 6, 2026", "Product information, images and purchasing actions.", "Desktop (1920 × 1080)", "May 3, 2026, 11:20 AM", 18, 16, 185, 129),
            CreateScreen("Checkout", "Desktop", null, false, "Updated: May 6, 2026", "Checkout flow with delivery, payment and summary areas.", "Desktop (1920 × 1080)", "May 4, 2026, 01:45 PM", 22, 139, 92, 246),
            CreateScreen("Settings", "Desktop", null, false, "Updated: May 5, 2026", "Application settings and preferences overview.", "Desktop (1440 × 900)", "May 4, 2026, 03:10 PM", 15, 245, 158, 11),
            CreateScreen("Report View", "Desktop", null, false, "Updated: May 5, 2026", "Dashboard report with charts and data summaries.", "Desktop (1920 × 1080)", "May 5, 2026, 09:05 AM", 19, 14, 165, 233)
        ];

        SelectedScreen = Screens[0];
    }

    public IReadOnlyList<string> ProjectTargets { get; }

    public IReadOnlyList<string> DevicePresets { get; }

    public IReadOnlyList<string> LayoutModes { get; }

    public IReadOnlyList<string> SnapSpacings { get; }

    public IReadOnlyList<string> SaveBehaviors { get; }

    public IReadOnlyList<string> ExportTargets { get; }

    public ObservableCollection<MockupScreenListItem> Screens { get; }

    [ObservableProperty]
    public partial MockupScreenListItem? SelectedScreen { get; set; }

    [ObservableProperty]
    public partial string ProjectName { get; set; } = "flink E-Commerce";

    [ObservableProperty]
    public partial string Description { get; set; } = "E-Commerce application for browsing products, adding items to cart, and completing purchases with multiple payment options.";

    [ObservableProperty]
    public partial string AuthorTeam { get; set; } = "VIA Software Team";

    [ObservableProperty]
    public partial string Version { get; set; } = "1.0.0";

    [ObservableProperty]
    public partial string ProjectPath { get; set; } = @"C:\VIA_Projects\flinkECommerce\flinkECommerce.fproj";

    [ObservableProperty]
    public partial string AssetFolder { get; set; } = @"C:\VIA_Projects\flinkECommerce\Assets";

    [ObservableProperty]
    public partial string Notes { get; set; } = "Initial build includes the core shopping flow and admin reports. Integrate API endpoints and finalize the payment provider before release.";

    [ObservableProperty]
    public partial string SelectedTarget { get; set; } = "Desktop";

    [ObservableProperty]
    public partial string SelectedResolution { get; set; } = "Desktop (1920 × 1080)";

    [ObservableProperty]
    public partial string StartupScreen { get; set; } = "Home Screen";

    [ObservableProperty]
    public partial string BaseLayoutMode { get; set; } = "Grid";

    [ObservableProperty]
    public partial string SnapSpacing { get; set; } = "8 px";

    [ObservableProperty]
    public partial string SaveBehavior { get; set; } = "Prompt on Save";

    [ObservableProperty]
    public partial string ExportTarget { get; set; } = "Windows (Desktop)";

    [ObservableProperty]
    public partial string NamingPrefix { get; set; } = "flk_";

    [ObservableProperty]
    public partial bool IsAutosaveEnabled { get; set; } = true;

    [ObservableProperty]
    public partial double Zoom { get; set; } = 83d;

    [ObservableProperty]
    public partial string ScreenSearchText { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string StatusText { get; set; } = "Up to date";

    [ObservableProperty]
    public partial string LastSavedText { get; set; } = "Last saved: Today, 9:41 AM";

    [RelayCommand]
    public void NewScreen()
    {
        MockupScreenListItem item = CreateScreen(
            $"New Screen {Screens.Count + 1}",
            SelectedTarget,
            "Draft",
            false,
            "Updated: Just now",
            "New screen draft.",
            SelectedResolution,
            "Just now",
            0,
            59,
            130,
            246);

        Screens.Insert(0, item);
        SelectedScreen = item;
        StatusText = "New screen created";
    }

    [RelayCommand]
    public void Save()
    {
        StatusText = "Saved";
        LastSavedText = "Last saved: Just now";
    }

    [RelayCommand]
    public void OpenDesigner()
    {
        StatusText = SelectedScreen is null
            ? "Select a screen first"
            : $"Designer requested for {SelectedScreen.Name}";
    }

    [RelayCommand]
    public void OpenProject() => StatusText = "Open project requested";

    [RelayCommand]
    public void SaveAs() => StatusText = "Save As requested";

    [RelayCommand]
    public void RenameProject() => StatusText = "Rename project requested";

    [RelayCommand]
    public void DeleteProject() => StatusText = "Delete project requested";

    private static MockupScreenListItem CreateScreen(
        string name,
        string target,
        string? state,
        bool isMain,
        string updatedText,
        string description,
        string resolution,
        string createdText,
        int controlCount,
        byte red,
        byte green,
        byte blue)
    {
        SolidColorBrush brush = new(Color.FromRgb(red, green, blue));
        brush.Freeze();

        return new MockupScreenListItem
        {
            Name = name,
            Target = target,
            State = state,
            IsMain = isMain,
            UpdatedText = updatedText,
            Description = description,
            Resolution = resolution,
            CreatedText = createdText,
            UsedControlsCount = controlCount,
            AccentBrush = brush
        };
    }
}
