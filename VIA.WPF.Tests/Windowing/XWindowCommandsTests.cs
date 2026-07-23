// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XWindowCommandsTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Input;
using VIA.WPF.Windowing;

namespace VIA.WPF.Tests.Windowing;

#region ### Class XWindowCommandsTests ###
/// <summary>
/// Provides tests for the <see cref="XWindowCommands" /> class.
/// </summary>
public sealed class XWindowCommandsTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that all standard window commands are exposed as routed UI commands.
    /// </summary>
    [Fact]
    public void Commands_ShouldExposeExpectedRoutedUICommands()
    {
        AssertCommand(XWindowCommands.Minimize, nameof(XWindowCommands.Minimize), "Minimize");
        AssertCommand(XWindowCommands.MaximizeRestore, nameof(XWindowCommands.MaximizeRestore), "MaximizeRestore");
        AssertCommand(XWindowCommands.Close, nameof(XWindowCommands.Close), "Close");
        AssertCommand(XWindowCommands.ToggleThemeMode, nameof(XWindowCommands.ToggleThemeMode), "ToggleThemeMode");
    }

    /// <summary>
    /// Verifies that the command instances are stable singletons.
    /// </summary>
    [Fact]
    public void Commands_ShouldReturnStableInstances()
    {
        Assert.Same(XWindowCommands.Minimize, XWindowCommands.Minimize);
        Assert.Same(XWindowCommands.MaximizeRestore, XWindowCommands.MaximizeRestore);
        Assert.Same(XWindowCommands.Close, XWindowCommands.Close);
        Assert.Same(XWindowCommands.ToggleThemeMode, XWindowCommands.ToggleThemeMode);
    }
    #endregion

    #region ### Private Methods ###
    private static void AssertCommand(RoutedUICommand command, string expectedName, string expectedText)
    {
        Assert.Equal(expectedName, command.Name);
        Assert.Equal(expectedText, command.Text);
        Assert.Equal(typeof(XWindowCommands), command.OwnerType);
    }
    #endregion
}
#endregion
