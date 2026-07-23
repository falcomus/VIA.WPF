// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XClipboardServiceTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using VIA.WPF.Services;

namespace VIA.WPF.Tests.Controls.Services;

#region ### Class XClipboardServiceTests ###
/// <summary>
/// Provides tests for clipboard service helper behavior that does not require touching the operating-system clipboard.
/// </summary>
public sealed class XClipboardServiceTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that CSV creation rejects a null row sequence before using the clipboard.
    /// </summary>
    [Fact]
    public void TrySetCsv_ShouldRejectNullRows()
    {
        XClipboardService service = new();

        Assert.Throws<ArgumentNullException>(() => service.TrySetCsv(null!));
    }

    /// <summary>
    /// Ensures that CSV creation uses semicolons, line breaks and escaped values.
    /// </summary>
    [Fact]
    public void CreateCsv_ShouldEscapeSpecialValues()
    {
        List<List<object?>> rows =
        [
            ["Alpha", "Beta;Gamma", "Quote \"Value\""],
            [null, "Line\nBreak", 42]
        ];
        MethodInfo method = GetPrivateStaticMethod("CreateCsv");
        object?[] arguments = [rows];

        string csv = Assert.IsType<string>(method.Invoke(null, arguments));

        Assert.Equal(
            $"Alpha;\"Beta;Gamma\";\"Quote \"\"Value\"\"\"{Environment.NewLine};\"Line\nBreak\";42{Environment.NewLine}",
            csv);
    }

    /// <summary>
    /// Ensures that plain CSV values are not quoted unnecessarily.
    /// </summary>
    [Fact]
    public void EscapeCsvValue_ShouldReturnPlainValuesUnquoted()
    {
        MethodInfo method = GetPrivateStaticMethod("EscapeCsvValue");
        object?[] arguments = ["Plain"];

        string text = Assert.IsType<string>(method.Invoke(null, arguments));

        Assert.Equal("Plain", text);
    }

    /// <summary>
    /// Ensures that null CSV values are rendered as empty cells.
    /// </summary>
    [Fact]
    public void EscapeCsvValue_ShouldRenderNullAsEmptyText()
    {
        MethodInfo method = GetPrivateStaticMethod("EscapeCsvValue");
        object?[] arguments = [null];

        string text = Assert.IsType<string>(method.Invoke(null, arguments));

        Assert.Equal(string.Empty, text);
    }
    #endregion

    #region ### Private Methods ###
    private static MethodInfo GetPrivateStaticMethod(string name)
    {
        MethodInfo? method = typeof(XClipboardService).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static);

        Assert.NotNull(method);
        return method;
    }
    #endregion
}
#endregion
