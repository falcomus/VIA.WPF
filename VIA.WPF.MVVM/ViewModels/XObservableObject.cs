// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XObservableObject.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;

namespace VIA.WPF.MVVM;

#region ### Class XObservableObject ###
/// <summary>
/// Provides the base observable object for VIA.WPF MVVM types.
/// </summary>
public abstract class XObservableObject : ObservableObject
{
    #region ### Protected Methods ###
    /// <summary>
    /// Raises property change notifications for multiple properties.
    /// </summary>
    /// <param name="propertyNames">The property names.</param>
    protected void OnPropertiesChanged(params string[] propertyNames)
    {
        foreach (string propertyName in propertyNames.Where(propertyName => !string.IsNullOrWhiteSpace(propertyName)))
        {
            this.OnPropertyChanged(propertyName);
        }
    }
    #endregion
}
#endregion
