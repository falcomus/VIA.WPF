// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DemoRuntimeSmokeTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using VIA.WPF.Demo;
using VIA.WPF.Demo.Views;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Smoke;

#region ### Class DemoRuntimeSmokeTests ###
/// <summary>
/// Guards the demo shell against deferred BAML and template errors that compilation cannot detect.
/// </summary>
public sealed class DemoRuntimeSmokeTests
{
    #region ### Tests ###
    [Fact]
    public void MainWindow_ShouldShowAndCompleteInitialLayout()
    {
        WpfTestHelper.Run(
            () =>
            {
                MainWindow window = new();
                window.Show();
                window.Dispatcher.Invoke(static () => { }, DispatcherPriority.Render);
                window.Close();
            });
    }

    [Fact]
    public void GettingStartedView_ShouldLoad()
    {
        WpfTestHelper.Run(
            () =>
            {
                try
                {
                    _ = new XGettingStartedView();
                }
                catch (XamlParseException exception)
                {
                    throw new InvalidOperationException(
                        $"XAML startup failure at {exception.BaseUri}, line {exception.LineNumber}:{exception.LinePosition}.",
                        exception);
                }
            });
    }
    #endregion
}
#endregion
