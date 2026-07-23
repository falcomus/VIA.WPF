// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IconTestExtension.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Markup;

namespace VIA.WPF.Icons;

#region ### Class IconTestExtension ###
/// <summary>
/// Provides a small test markup extension for XAML namespace discovery.
/// </summary>
[MarkupExtensionReturnType(typeof(string))]
public sealed class IconTestExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return "IconTest1";
    }
    #endregion
}
#endregion