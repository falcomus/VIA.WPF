// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XSearchableColumn.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.MVVM;

#region ### Class XSearchableColumn ###
/// <summary>
/// Represents a searchable page column with a technical property name and a display name.
/// </summary>
public sealed class XSearchableColumn : XObservableObject
{
    #region ### Fields ###
    private string propertyName;
    private string? displayName;
    private XValidationText? displayText;
    private bool isEnabled = true;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XSearchableColumn"/> class.
    /// </summary>
    /// <param name="propertyName">The technical property name.</param>
    public XSearchableColumn(string propertyName)
        : this(propertyName, propertyName)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XSearchableColumn"/> class.
    /// </summary>
    /// <param name="propertyName">The technical property name.</param>
    /// <param name="displayName">The display name.</param>
    public XSearchableColumn(string propertyName, string displayName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);

        this.propertyName = propertyName;
        this.displayName = string.IsNullOrWhiteSpace(displayName) ? propertyName : displayName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XSearchableColumn"/> class.
    /// </summary>
    /// <param name="propertyName">The technical property name.</param>
    /// <param name="displayText">The localizable display text.</param>
    public XSearchableColumn(string propertyName, XValidationText displayText)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
        ArgumentNullException.ThrowIfNull(displayText);

        this.propertyName = propertyName;
        this.displayText = displayText;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the technical property name used for searching.
    /// </summary>
    public string PropertyName
    {
        get => this.propertyName;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            this.SetProperty(ref this.propertyName, value);
            this.OnPropertyChanged(nameof(this.DisplayName));
        }
    }

    /// <summary>
    /// Gets or sets the display name shown to users.
    /// </summary>
    public string DisplayName
    {
        get => this.displayText?.Resolve() ?? this.displayName ?? this.PropertyName;
        set
        {
            this.displayText = null;
            this.SetProperty(ref this.displayName, string.IsNullOrWhiteSpace(value) ? this.PropertyName : value);
        }
    }

    /// <summary>
    /// Gets or sets the optional localizable display text.
    /// </summary>
    public XValidationText? DisplayText
    {
        get => this.displayText;
        set
        {
            if (this.SetProperty(ref this.displayText, value))
            {
                this.OnPropertyChanged(nameof(this.DisplayName));
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this column participates in search.
    /// </summary>
    public bool IsEnabled
    {
        get => this.isEnabled;
        set => this.SetProperty(ref this.isEnabled, value);
    }
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public override string ToString()
    {
        return this.DisplayName;
    }
    #endregion
}
#endregion
