// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalizationDemoResources.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Resources;

namespace VIA.WPF.Demo.Resources;

#region ### Class LocalizationDemoResources ###
/// <summary>
/// Provides access to the application-owned resource files used by the localization demo.
/// </summary>
public static class LocalizationDemoResources
{
    #region ### Fields ###
    private static readonly ResourceManager ResourceManagerInstance = new(
        "VIA.WPF.Demo.Resources.LocalizationDemoStrings",
        typeof(LocalizationDemoResources).Assembly);
    #endregion

    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the resource manager used by the localization demo.
    /// </summary>
    public static ResourceManager ResourceManager => ResourceManagerInstance;
    #endregion
}
#endregion
