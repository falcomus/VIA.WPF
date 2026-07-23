// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XContentState.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Controls;

/// <summary>
/// Defines the state displayed by an <see cref="XContentStatePresenter"/>.
/// </summary>
public enum XContentState
{
    /// <summary>Displays the regular content.</summary>
    Content,

    /// <summary>Displays progress feedback.</summary>
    Loading,

    /// <summary>Displays an empty state.</summary>
    Empty,

    /// <summary>Displays an error state.</summary>
    Error,

    /// <summary>Displays an offline state.</summary>
    Offline
}
