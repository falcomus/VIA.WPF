// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XLookupInsertRequestEventArgs.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace VIA.WPF.Controls;

#region ### Class XLookupInsertRequestEventArgs ###
/// <summary>
/// Provides event data for lookup insert requests.
/// </summary>
public sealed class XLookupInsertRequestEventArgs : RoutedEventArgs
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XLookupInsertRequestEventArgs"/> class.
    /// </summary>
    /// <param name="routedEvent">The routed event.</param>
    /// <param name="source">The event source.</param>
    /// <param name="request">The insert request.</param>
    public XLookupInsertRequestEventArgs(RoutedEvent routedEvent, object source, XLookupInsertRequest request)
        : base(routedEvent, source)
    {
        ArgumentNullException.ThrowIfNull(request);
        this.Request = request;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the insert request.
    /// </summary>
    public XLookupInsertRequest Request { get; }

    /// <summary>
    /// Gets the requested lookup text.
    /// </summary>
    public string Text => this.Request.Text;
    #endregion
}
#endregion
