// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XControlThemeManager.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace VIA.WPF.Controls;

#region ### Class XControlThemeManager ###
/// <summary>
/// Provides initialization for global VIA.WPF resource dictionaries.
/// </summary>
internal static class XControlThemeManager
{
    #region ### Private Fields ###
    private static bool _isInitialized;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Ensures that global VIA.WPF resource dictionaries are merged into the current application resources.
    /// </summary>
    public static void EnsureGlobalResources()
    {
        if (_isInitialized)
        {
            return;
        }

        if (Application.Current is null)
        {
            return;
        }

        MergeDictionary("/VIA.WPF.Controls;component/Themes/XScrollBar.xaml");

        _isInitialized = true;
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Merges the specified resource dictionary if it has not already been added.
    /// </summary>
    /// <param name="source">The pack URI source.</param>
    private static void MergeDictionary(string source)
    {
        Uri uri = new(source, UriKind.Relative);

        bool exists = Application.Current!.Resources.MergedDictionaries.Any(dictionary => dictionary.Source == uri);
        if (exists)
        {
            return;
        }

        Application.Current.Resources.MergedDictionaries.Add(
            new ResourceDictionary
            {
                Source = uri
            });
    }
    #endregion
}
#endregion