// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XFormGrid.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Controls;

#region ### Class XFormGrid ###
/// <summary>
/// Represents a dense two-column form grid using VIA.WPF spacing defaults.
/// </summary>
/// <remarks>
/// Children use standard Grid row and column placement. The default columns are an auto-sized label column and a
/// stretching editor column.
/// </remarks>
public class XFormGrid : XGrid
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XFormGrid"/> class.
    /// </summary>
    public XFormGrid()
    {
        this.Columns = "Auto,*";
        this.ColumnSpacing = 12d;
        this.RowSpacing = 8d;
    }
    #endregion
}
#endregion
