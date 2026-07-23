// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationLocalizationSettingsTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Resources;
using VIA.WPF.MVVM;

namespace VIA.WPF.Tests.MVVM.Validation;

#region ### Class XValidationLocalizationSettingsTests ###
/// <summary>
/// Tests snapshot and restore support for global validation localization settings.
/// </summary>
public sealed class XValidationLocalizationSettingsTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that localization settings can be captured and restored.
    /// </summary>
    [Fact]
    public void CaptureAndRestore_ShouldRestorePreviousSettings()
    {
        XValidationLocalizationSettings originalSettings = XValidationLocalization.Capture();
        TestResourceManager temporaryResourceManager = new("Temporary");

        try
        {
            XValidationLocalization.ResourceManager = temporaryResourceManager;
            XValidationLocalization.Culture = CultureInfo.InvariantCulture;
            XValidationLocalization.ThrowOnMissingResource = true;

            XValidationLocalizationSettings capturedSettings = XValidationLocalization.Capture();

            XValidationLocalization.ResourceManager = null;
            XValidationLocalization.Culture = new CultureInfo("de-DE");
            XValidationLocalization.ThrowOnMissingResource = false;

            XValidationLocalization.Restore(capturedSettings);

            Assert.Same(temporaryResourceManager, XValidationLocalization.ResourceManager);
            Assert.Equal(CultureInfo.InvariantCulture, XValidationLocalization.Culture);
            Assert.True(XValidationLocalization.ThrowOnMissingResource);
        }
        finally
        {
            XValidationLocalization.Restore(originalSettings);
        }
    }
    #endregion

    #region ### Class TestResourceManager ###
    private sealed class TestResourceManager : ResourceManager
    {
        #region ### Fields ###
        private readonly string value;
        #endregion

        #region ### Constructors ###
        public TestResourceManager(string value)
        {
            this.value = value;
        }
        #endregion

        #region ### Public Methods ###
        public override string? GetString(string name, CultureInfo? culture)
        {
            return this.value;
        }
        #endregion
    }
    #endregion
}
#endregion
