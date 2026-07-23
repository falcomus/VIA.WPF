// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IXClipboardService.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Services;

#region ### Interface IXClipboardService ###
/// <summary>
/// Provides clipboard operations that can be mocked by view models and tests.
/// </summary>
public interface IXClipboardService
{
    #region ### Public Methods ###
    /// <summary>
    /// Tries to write text to the clipboard.
    /// </summary>
    /// <param name="text">The text value.</param>
    /// <returns><c>true</c> if the clipboard operation succeeded; otherwise, <c>false</c>.</returns>
    bool TrySetText(string? text);

    /// <summary>
    /// Tries to read text from the clipboard.
    /// </summary>
    /// <param name="text">The clipboard text.</param>
    /// <returns><c>true</c> if the clipboard operation succeeded; otherwise, <c>false</c>.</returns>
    bool TryGetText(out string? text);

    /// <summary>
    /// Tries to write CSV text to the clipboard.
    /// </summary>
    /// <param name="rows">The CSV rows.</param>
    /// <returns><c>true</c> if the clipboard operation succeeded; otherwise, <c>false</c>.</returns>
    bool TrySetCsv(IEnumerable<IEnumerable<object?>> rows);
    #endregion
}
#endregion
