// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FreezableExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace VIA.WPF.Extensions;

#region ### Class FreezableExtensions ###
/// <summary>
/// Provides convenience methods for WPF freezable objects.
/// </summary>
public static class FreezableExtensions
{
    #region ### Public Methods ###
    /// <summary>
    /// Freezes the specified freezable if freezing is possible and returns the same instance.
    /// </summary>
    /// <typeparam name="T">The freezable type.</typeparam>
    /// <param name="freezable">The freezable object.</param>
    /// <returns>The same freezable object.</returns>
    public static T FreezeIfPossible<T>(this T freezable)
        where T : Freezable
    {
        ArgumentNullException.ThrowIfNull(freezable);

        if (freezable.CanFreeze && !freezable.IsFrozen)
        {
            freezable.Freeze();
        }

        return freezable;
    }

    /// <summary>
    /// Clones the specified freezable using current values and freezes the clone when possible.
    /// </summary>
    /// <typeparam name="T">The freezable type.</typeparam>
    /// <param name="freezable">The freezable object.</param>
    /// <returns>The frozen clone.</returns>
    public static T CloneCurrentValueFrozen<T>(this T freezable)
        where T : Freezable
    {
        ArgumentNullException.ThrowIfNull(freezable);

        T clone = (T)freezable.CloneCurrentValue();

        return clone.FreezeIfPossible();
    }
    #endregion
}
#endregion
