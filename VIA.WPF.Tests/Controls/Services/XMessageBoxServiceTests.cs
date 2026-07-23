// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XMessageBoxServiceTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using VIA.WPF.Services;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Services;

#region ### Class XMessageBoxServiceTests ###
/// <summary>
/// Provides non-invasive tests for the message box service.
/// </summary>
public sealed class XMessageBoxServiceTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that the service starts without an owner window.
    /// </summary>
    [Fact]
    public void Constructor_ShouldHaveNoOwner()
    {
        XMessageBoxService service = new();

        Assert.Null(service.Owner);
    }

    /// <summary>
    /// Ensures that the optional owner window can be assigned and cleared.
    /// </summary>
    [Fact]
    public void Owner_ShouldRoundtripAssignedWindow()
    {
        WpfTestHelper.Run(
            () =>
            {
                XMessageBoxService service = new();
                Window owner = new();

                service.Owner = owner;

                Assert.Same(owner, service.Owner);

                service.Owner = null;

                Assert.Null(service.Owner);
            });
    }
    #endregion
}
#endregion
