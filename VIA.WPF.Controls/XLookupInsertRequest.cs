// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XLookupInsertRequest.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Controls;

#region ### Class XLookupInsertRequest ###
/// <summary>
/// Represents a request to insert a new lookup item from editable lookup text.
/// </summary>
public sealed class XLookupInsertRequest
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XLookupInsertRequest"/> class.
    /// </summary>
    /// <param name="text">The requested lookup text.</param>
    /// <param name="sourceControl">The source lookup combo box.</param>
    public XLookupInsertRequest(string text, XLookupComboBox sourceControl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        ArgumentNullException.ThrowIfNull(sourceControl);

        this.Text = text.Trim();
        this.SourceControl = sourceControl;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the requested lookup text.
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// Gets the source lookup combo box that raised the request.
    /// </summary>
    public XLookupComboBox SourceControl { get; }
    #endregion
}
#endregion
