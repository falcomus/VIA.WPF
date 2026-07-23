// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XClipboardService.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Text;
using System.Windows;

namespace VIA.WPF.Services;

#region ### Class XClipboardService ###
/// <summary>
/// Provides safe clipboard operations for WPF applications.
/// </summary>
public sealed class XClipboardService : IXClipboardService
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public bool TrySetText(string? text)
    {
        try
        {
            Clipboard.SetText(text ?? string.Empty);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <inheritdoc />
    public bool TryGetText(out string? text)
    {
        try
        {
            if (Clipboard.ContainsText())
            {
                text = Clipboard.GetText();
                return true;
            }
        }
        catch (Exception)
        {
            // Clipboard can be locked by another process.
        }

        text = null;
        return false;
    }

    /// <inheritdoc />
    public bool TrySetCsv(IEnumerable<IEnumerable<object?>> rows)
    {
        ArgumentNullException.ThrowIfNull(rows);

        string csv = CreateCsv(rows);
        return this.TrySetText(csv);
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Creates CSV text from the specified rows.
    /// </summary>
    /// <param name="rows">The rows.</param>
    /// <returns>The CSV text.</returns>
    private static string CreateCsv(IEnumerable<IEnumerable<object?>> rows)
    {
        StringBuilder builder = new();

        foreach (IEnumerable<object?> row in rows)
        {
            string line = string.Join(";", row.Select(EscapeCsvValue));
            builder.AppendLine(line);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Escapes a CSV value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The escaped CSV value.</returns>
    private static string EscapeCsvValue(object? value)
    {
        string text = value switch
        {
            null => string.Empty,
            IFormattable formattable => formattable.ToString(null, CultureInfo.CurrentCulture),
            _ => value.ToString() ?? string.Empty
        };

        if (!text.Contains(';') && !text.Contains('"') && !text.Contains('\n') && !text.Contains('\r'))
        {
            return text;
        }

        return $"\"{text.Replace("\"", "\"\"")}\"";
    }
    #endregion
}
#endregion
