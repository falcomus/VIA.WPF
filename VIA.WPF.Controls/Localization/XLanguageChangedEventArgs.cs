// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XLanguageChangedEventArgs.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;

namespace VIA.WPF.Localization;

#region ### Class XLanguageChangedEventArgs ###
/// <summary>
/// Provides data for a VIA.WPF application language change.
/// </summary>
public sealed class XLanguageChangedEventArgs : EventArgs
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XLanguageChangedEventArgs"/> class.
    /// </summary>
    /// <param name="previousCulture">The previous UI culture.</param>
    /// <param name="currentCulture">The current UI culture.</param>
    public XLanguageChangedEventArgs(CultureInfo previousCulture, CultureInfo currentCulture)
    {
        ArgumentNullException.ThrowIfNull(previousCulture);
        ArgumentNullException.ThrowIfNull(currentCulture);

        this.PreviousCulture = previousCulture;
        this.CurrentCulture = currentCulture;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the previous UI culture.
    /// </summary>
    public CultureInfo PreviousCulture { get; }

    /// <summary>
    /// Gets the current UI culture.
    /// </summary>
    public CultureInfo CurrentCulture { get; }
    #endregion
}
#endregion
