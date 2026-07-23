// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConverterMarkupExtensionTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Markup;
using VIA.WPF.Converters;

namespace VIA.WPF.Tests.Controls.Converters;

#region ### Class ConverterMarkupExtensionTests ###
/// <summary>
/// Tests converter markup extensions.
/// </summary>
public sealed class ConverterMarkupExtensionTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that value-converter markup extensions return their shared converter instances.
    /// </summary>
    /// <param name="extension">The markup extension.</param>
    /// <param name="expectedInstance">The expected shared converter instance.</param>
    [Theory]
    [MemberData(nameof(GetConverterExtensions))]
    public void ProvideValue_ShouldReturnSharedConverterInstance(MarkupExtension extension, object expectedInstance)
    {
        object result = extension.ProvideValue(NullServiceProvider.Instance);

        Assert.Same(expectedInstance, result);
    }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Gets converter markup extension test data.
    /// </summary>
    /// <returns>The converter markup extension test data.</returns>
    public static TheoryData<MarkupExtension, object> GetConverterExtensions()
    {
        return new TheoryData<MarkupExtension, object>
        {
            { new AllTrueToVisibilityExtension(), AllTrueToVisibilityConverter.Instance },
            { new AnyTrueToVisibilityExtension(), AnyTrueToVisibilityConverter.Instance },
            { new BooleanAndExtension(), BooleanAndConverter.Instance },
            { new BooleanOrExtension(), BooleanOrConverter.Instance },
            { new BooleanToGridLengthExtension(), BooleanToGridLengthConverter.Instance },
            { new BooleanToOpacityExtension(), BooleanToOpacityConverter.Instance },
            { new BooleanToThicknessExtension(), BooleanToThicknessConverter.Instance },
            { new BooleanToVisibilityExtension(), VIA.WPF.Converters.BooleanToVisibilityConverter.Instance },
            { new BrushOpacityExtension(), BrushOpacityConverter.Instance },
            { new CollectionEmptyToVisibilityExtension(), CollectionEmptyToVisibilityConverter.Instance },
            { new CollectionNotEmptyToVisibilityExtension(), CollectionNotEmptyToVisibilityConverter.Instance },
            { new ColorToBrushExtension(), ColorToBrushConverter.Instance },
            { new CountToVisibilityExtension(), CountToVisibilityConverter.Instance },
            { new EnumToBooleanExtension(), EnumToBooleanConverter.Instance },
            { new EnumToVisibilityExtension(), EnumToVisibilityConverter.Instance },
            { new EqualityToBooleanExtension(), EqualityToBooleanConverter.Instance },
            { new EqualityToVisibilityExtension(), EqualityToVisibilityConverter.Instance },
            { new InverseBooleanExtension(), InverseBooleanConverter.Instance },
            { new InverseBooleanToVisibilityExtension(), InverseBooleanToVisibilityConverter.Instance },
            { new MultiplyExtension(), MultiplyConverter.Instance },
            { new NotNullToBooleanExtension(), NotNullToBooleanConverter.Instance },
            { new NullToBooleanExtension(), NullToBooleanConverter.Instance },
            { new NullToVisibilityExtension(), NullToVisibilityConverter.Instance },
            { new NumberGreaterThanToVisibilityExtension(), NumberGreaterThanToVisibilityConverter.Instance },
            { new ObjectReferenceEqualsExtension(), ObjectReferenceEqualsConverter.Instance },
            { new StringNullOrEmptyToVisibilityExtension(), StringNullOrEmptyToVisibilityConverter.Instance },
            { new StringNullOrWhiteSpaceToVisibilityExtension(), StringNullOrWhiteSpaceToVisibilityConverter.Instance },
            { new TreeLevelToThicknessExtension(), TreeLevelToThicknessConverter.Instance }
        };
    }
    #endregion

    #region ### Private Classes ###
    private sealed class NullServiceProvider : IServiceProvider
    {
        #region ### Public Static Properties ###
        /// <summary>
        /// Gets the shared null service provider instance.
        /// </summary>
        public static NullServiceProvider Instance { get; } = new();
        #endregion

        #region ### Public Methods ###
        /// <inheritdoc />
        public object? GetService(Type serviceType)
        {
            return null;
        }
        #endregion
    }
    #endregion
}
#endregion
