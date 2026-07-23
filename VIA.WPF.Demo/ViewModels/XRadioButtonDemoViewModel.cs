// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XRadioButtonDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XRadioButtonDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XRadioButton showcase page.
/// </summary>
public sealed class XRadioButtonDemoViewModel : DemoPageViewModel
{
    #region ### Private Fields ###
    private bool _isDailyDigest;
    private bool _isWeeklyDigest = true;
    private bool _isMonthlyDigest;
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the demo page title.
    /// </summary>
    public override string Title => "XRadioButton";

    /// <summary>
    /// Gets the demo page description.
    /// </summary>
    public override string Description => "Demonstrates grouped single-choice options with the VIA.WPF size model, disabled states and MVVM-backed selection.";

    /// <summary>
    /// Gets or sets a value indicating whether the daily digest option is selected.
    /// </summary>
    public bool IsDailyDigest
    {
        get => _isDailyDigest;
        set
        {
            if (SetProperty(ref _isDailyDigest, value))
            {
                OnPropertyChanged(nameof(DigestSummary));
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the weekly digest option is selected.
    /// </summary>
    public bool IsWeeklyDigest
    {
        get => _isWeeklyDigest;
        set
        {
            if (SetProperty(ref _isWeeklyDigest, value))
            {
                OnPropertyChanged(nameof(DigestSummary));
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the monthly digest option is selected.
    /// </summary>
    public bool IsMonthlyDigest
    {
        get => _isMonthlyDigest;
        set
        {
            if (SetProperty(ref _isMonthlyDigest, value))
            {
                OnPropertyChanged(nameof(DigestSummary));
            }
        }
    }

    /// <summary>
    /// Gets a short summary for the live binding sample.
    /// </summary>
    public string DigestSummary
    {
        get
        {
            if (IsDailyDigest)
            {
                return "Digest frequency: daily.";
            }

            if (IsMonthlyDigest)
            {
                return "Digest frequency: monthly.";
            }

            return "Digest frequency: weekly.";
        }
    }

    /// <summary>
    /// Gets the XAML code shown on the demo page.
    /// </summary>
    public override string XamlCode => """
<!-- Grouped selection -->
<via:XRadioButton Content="List layout" GroupName="LayoutMode" IsChecked="True" />
<via:XRadioButton Content="Grid layout" GroupName="LayoutMode" />
<via:XRadioButton Content="Compact cards" GroupName="LayoutMode" />

<!-- Sizes -->
<via:XRadioButton Content="Small option" GroupName="SizeSample" Size="Small" />
<via:XRadioButton Content="Medium option" GroupName="SizeSample" Size="Medium" />
<via:XRadioButton Content="Large option" GroupName="SizeSample" Size="Large" />

<!-- Disabled state -->
<via:XRadioButton Content="Disabled checked" GroupName="DisabledSample" IsChecked="True" IsEnabled="False" />

<!-- MVVM binding -->
<via:XRadioButton
    Content="Weekly"
    GroupName="DigestFrequency"
    IsChecked="{Binding IsWeeklyDigest, Mode=TwoWay}" />
""";

    /// <summary>
    /// Gets the C# code shown on the demo page.
    /// </summary>
    public override string CSharpCode => """
private bool _isWeeklyDigest = true;

public bool IsWeeklyDigest
{
    get => _isWeeklyDigest;
    set
    {
        if (SetProperty(ref _isWeeklyDigest, value))
        {
            OnPropertyChanged(nameof(DigestSummary));
        }
    }
}

public string DigestSummary => IsWeeklyDigest
    ? "Digest frequency: weekly."
    : "Another digest frequency is selected.";
""";
    #endregion
}
#endregion
