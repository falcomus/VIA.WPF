// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XamlServiceProviderStub.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Markup;

namespace VIA.WPF.Tests.Helpers;

#region ### Class XamlServiceProviderStub ###
/// <summary>
/// Provides a minimal XAML service provider for markup extension tests.
/// </summary>
internal sealed class XamlServiceProviderStub : IServiceProvider, IProvideValueTarget
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XamlServiceProviderStub"/> class.
    /// </summary>
    /// <param name="targetObject">The target object exposed through <see cref="IProvideValueTarget"/>.</param>
    /// <param name="targetProperty">The target property exposed through <see cref="IProvideValueTarget"/>.</param>
    public XamlServiceProviderStub(object? targetObject = null, object? targetProperty = null)
    {
        this.TargetObject = targetObject;
        this.TargetProperty = targetProperty;
    }
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc />
    public object? TargetObject { get; }

    /// <inheritdoc />
    public object? TargetProperty { get; }
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public object? GetService(Type serviceType)
    {
        return serviceType == typeof(IProvideValueTarget)
            ? this
            : null;
    }
    #endregion
}
#endregion
