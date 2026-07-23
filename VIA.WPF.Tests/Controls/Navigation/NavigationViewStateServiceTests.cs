// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigationViewStateServiceTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.Navigation;

namespace VIA.WPF.Tests.Controls.Navigation;

#region ### Class NavigationViewStateServiceTests ###
/// <summary>
/// Provides tests for <see cref="NavigationViewStateService" />.
/// </summary>
public sealed class NavigationViewStateServiceTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that page preference keys are formatted deterministically.
    /// </summary>
    [Fact]
    public void CreatePageKey_ShouldFormatSectionAndPageValues()
    {
        string key = NavigationViewStateService.CreatePageKey("Section", "Page");

        Assert.Equal("Page|Section|Page", key);
    }

    /// <summary>
    /// Ensures that null values are converted to the stable None key part.
    /// </summary>
    [Fact]
    public void CreatePageKey_ShouldUseNoneForNullValues()
    {
        string key = NavigationViewStateService.CreatePageKey(null, null);

        Assert.Equal("Page|None|None", key);
    }

    /// <summary>
    /// Ensures that workspace preference keys are formatted deterministically.
    /// </summary>
    [Fact]
    public void CreateWorkspaceKey_ShouldFormatSectionValue()
    {
        string key = NavigationViewStateService.CreateWorkspaceKey("Section");

        Assert.Equal("Workspace|Section", key);
    }

    /// <summary>
    /// Ensures that empty or missing keys return the supplied default value.
    /// </summary>
    [Fact]
    public void GetRememberViewStateOrDefault_ShouldReturnDefaultForEmptyOrMissingKeys()
    {
        bool emptyDefault = NavigationViewStateService.Current.GetRememberViewStateOrDefault(string.Empty, true);
        bool whitespaceDefault = NavigationViewStateService.Current.GetRememberViewStateOrDefault("   ", false);
        bool missingDefault = NavigationViewStateService.Current.GetRememberViewStateOrDefault(CreateUniqueKey(), true);

        Assert.True(emptyDefault);
        Assert.False(whitespaceDefault);
        Assert.True(missingDefault);
    }

    /// <summary>
    /// Ensures that stored values override the supplied default value.
    /// </summary>
    [Fact]
    public void SetRememberViewState_ShouldStoreValueForKey()
    {
        string key = CreateUniqueKey();

        NavigationViewStateService.Current.SetRememberViewState(key, true);
        bool storedTrue = NavigationViewStateService.Current.GetRememberViewStateOrDefault(key, false);

        NavigationViewStateService.Current.SetRememberViewState(key, false);
        bool storedFalse = NavigationViewStateService.Current.GetRememberViewStateOrDefault(key, true);

        Assert.True(storedTrue);
        Assert.False(storedFalse);
    }

    /// <summary>
    /// Ensures that storing an empty key is ignored.
    /// </summary>
    [Fact]
    public void SetRememberViewState_ShouldIgnoreEmptyKeys()
    {
        NavigationViewStateService.Current.SetRememberViewState(string.Empty, true);

        bool result = NavigationViewStateService.Current.GetRememberViewStateOrDefault(string.Empty, false);

        Assert.False(result);
    }
    #endregion

    #region ### Private Methods ###
    private static string CreateUniqueKey()
    {
        return $"Test|{Guid.NewGuid():N}";
    }
    #endregion
}
#endregion
