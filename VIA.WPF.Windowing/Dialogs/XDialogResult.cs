// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDialogResult.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Windowing;

#region ### Struct XDialogResult ###
/// <summary>
/// Represents the normalized result of a modal WPF dialog.
/// </summary>
public readonly record struct XDialogResult
{
    #region ### Constructors ###
    private XDialogResult(XDialogOutcome outcome, bool? nativeResult)
    {
        this.Outcome = outcome;
        this.NativeResult = nativeResult;
    }
    #endregion

    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the result representing an accepted dialog.
    /// </summary>
    public static XDialogResult Accepted { get; } = new(XDialogOutcome.Accepted, true);

    /// <summary>
    /// Gets the result representing a dialog that was not accepted.
    /// </summary>
    public static XDialogResult NotAccepted { get; } = new(XDialogOutcome.NotAccepted, false);

    /// <summary>
    /// Gets the result representing a dialog without a boolean result.
    /// </summary>
    public static XDialogResult NoResult { get; } = new(XDialogOutcome.NoResult, null);
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the normalized dialog outcome.
    /// </summary>
    public XDialogOutcome Outcome { get; }

    /// <summary>
    /// Gets the original nullable WPF dialog result.
    /// </summary>
    public bool? NativeResult { get; }

    /// <summary>
    /// Gets a value indicating whether the dialog was accepted.
    /// </summary>
    public bool IsAccepted => this.Outcome == XDialogOutcome.Accepted;
    #endregion

    #region ### Internal Methods ###
    /// <summary>
    /// Creates a normalized result from the nullable WPF dialog result.
    /// </summary>
    /// <param name="nativeResult">The native WPF dialog result.</param>
    /// <returns>The normalized result.</returns>
    internal static XDialogResult FromNativeResult(bool? nativeResult)
    {
        return nativeResult switch
        {
            true => Accepted,
            false => NotAccepted,
            null => NoResult
        };
    }
    #endregion
}
#endregion