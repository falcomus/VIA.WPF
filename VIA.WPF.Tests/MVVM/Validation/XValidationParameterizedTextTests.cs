// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationParameterizedTextTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Resources;
using VIA.WPF.MVVM;

namespace VIA.WPF.Tests.MVVM.Validation;

#region ### Class XValidationParameterizedTextTests ###
/// <summary>
/// Tests parameterized validation text resolution.
/// </summary>
public sealed class XValidationParameterizedTextTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that literal validation texts are formatted with arguments.
    /// </summary>
    [Fact]
    public void Text_ShouldFormatArguments()
    {
        XValidationText text = XValidationText.Text("Maximum length is {0}.", 50);

        Assert.Equal("Maximum length is 50.", text.Resolve(CultureInfo.InvariantCulture));
        Assert.Equal(50, Assert.Single(text.Arguments));
    }

    /// <summary>
    /// Verifies that fallback texts are formatted with arguments when no resource can be resolved.
    /// </summary>
    [Fact]
    public void Resource_ShouldFormatFallbackArguments()
    {
        PreserveLocalizationSettings(() =>
        {
            XValidationLocalization.ResourceManager = null;
            XValidationLocalization.ThrowOnMissingResource = false;

            XValidationText text = XValidationText.Resource("NameMaxLength", "Name may contain at most {0} characters.", 50);

            Assert.Equal("Name may contain at most 50 characters.", text.Resolve(CultureInfo.InvariantCulture));
        });
    }

    /// <summary>
    /// Verifies that resolved resource texts are formatted with arguments.
    /// </summary>
    [Fact]
    public void Resource_ShouldFormatResolvedResourceArguments()
    {
        PreserveLocalizationSettings(() =>
        {
            XValidationLocalization.ResourceManager = new TestResourceManager(
                new Dictionary<string, string>
                {
                    ["NameMaxLength"] = "Name may contain at most {0} characters."
                });

            XValidationText text = XValidationText.Resource("NameMaxLength", 50);

            Assert.Equal("Name may contain at most 50 characters.", text.Resolve(CultureInfo.InvariantCulture));
        });
    }

    /// <summary>
    /// Verifies that context rule overloads preserve formatting arguments.
    /// </summary>
    [Fact]
    public void ContextRuleOverload_ShouldStoreFormattingArguments()
    {
        XValidationContext context = new(new object());

        context.MaxLength("123456", 5, "Name", "NameMaxLength", 5);

        XValidationError error = Assert.Single(context.Messages);
        Assert.Equal("Name", error.PropertyNames.Single());
        Assert.Equal("NameMaxLength", error.Text.ResourceKey);
        Assert.Equal(5, Assert.Single(error.Text.Arguments));
    }
    #endregion

    #region ### Private Methods ###
    private static void PreserveLocalizationSettings(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        XValidationLocalizationSettings settings = XValidationLocalization.Capture();

        try
        {
            action();
        }
        finally
        {
            XValidationLocalization.Restore(settings);
        }
    }
    #endregion

    #region ### Class TestResourceManager ###
    private sealed class TestResourceManager : ResourceManager
    {
        #region ### Fields ###
        private readonly IReadOnlyDictionary<string, string> values;
        #endregion

        #region ### Constructors ###
        public TestResourceManager(IReadOnlyDictionary<string, string> values)
        {
            this.values = values;
        }
        #endregion

        #region ### Public Methods ###
        public override string? GetString(string name, CultureInfo? culture)
        {
            return this.values.TryGetValue(name, out string? value) ? value : null;
        }
        #endregion
    }
    #endregion
}
#endregion
