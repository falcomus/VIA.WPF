// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XNavigationTabControl.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace VIA.WPF.Controls;

#region ### Class XNavigationTabControl ###
/// <summary>
/// Represents a tab based navigation control without selected tab content.
/// </summary>
public class XNavigationTabControl : XTabControl
{
    #region ### Constructors ###

    /// <summary>
    /// Initializes static members of the <see cref="XNavigationTabControl"/> class.
    /// </summary>
    static XNavigationTabControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XNavigationTabControl),
            new FrameworkPropertyMetadata(typeof(XTabControl)));

        ShowBorderProperty.OverrideMetadata(
            typeof(XNavigationTabControl),
            new FrameworkPropertyMetadata(false));

        ShowTabContentProperty.OverrideMetadata(
            typeof(XNavigationTabControl),
            new FrameworkPropertyMetadata(
                false,
                null,
                CoerceShowTabContent));

        HeaderAppearanceProperty.OverrideMetadata(
            typeof(XNavigationTabControl),
            new FrameworkPropertyMetadata(XTabHeaderAppearance.Underlined));

        FontSizeProperty.OverrideMetadata(
            typeof(XNavigationTabControl),
            new FrameworkPropertyMetadata(19d));

        FontWeightProperty.OverrideMetadata(
            typeof(XNavigationTabControl),
            new FrameworkPropertyMetadata(FontWeights.SemiBold));
    }

    #endregion

    #region ### Protected Methods ###

    /// <inheritdoc/>
    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return item is XNavigationTabItem;
    }

    /// <inheritdoc/>
    protected override DependencyObject GetContainerForItemOverride()
    {
        return new XNavigationTabItem();
    }

    /// <inheritdoc/>
    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
        base.PrepareContainerForItemOverride(element, item);

        if (element is not XNavigationTabItem tabItem)
        {
            return;
        }

        if (!ReferenceEquals(element, item))
        {
            object localHeaderValue = tabItem.ReadLocalValue(HeaderedContentControl.HeaderProperty);

            if (localHeaderValue == DependencyProperty.UnsetValue || ReferenceEquals(tabItem.Header, item))
            {
                BindingOperations.SetBinding(
                    tabItem,
                    HeaderedContentControl.HeaderProperty,
                    new Binding("Title")
                    {
                        Source = item,
                        Mode = BindingMode.OneWay,
                        FallbackValue = item?.ToString(),
                        TargetNullValue = item?.ToString(),
                    });
            }
        }

        if (tabItem.ReadLocalValue(FontSizeProperty) == DependencyProperty.UnsetValue)
        {
            BindingOperations.SetBinding(
                tabItem,
                FontSizeProperty,
                new Binding(nameof(FontSize))
                {
                    Source = this,
                    Mode = BindingMode.OneWay,
                });
        }

        if (tabItem.ReadLocalValue(FontWeightProperty) == DependencyProperty.UnsetValue)
        {
            BindingOperations.SetBinding(
                tabItem,
                FontWeightProperty,
                new Binding(nameof(FontWeight))
                {
                    Source = this,
                    Mode = BindingMode.OneWay,
                });
        }
    }

    /// <inheritdoc/>
    protected override void ClearContainerForItemOverride(DependencyObject element, object item)
    {
        if (element is XNavigationTabItem tabItem)
        {
            ClearBindingIfOwnedByThis(tabItem, HeaderedContentControl.HeaderProperty);
            ClearBindingIfOwnedByThis(tabItem, FontSizeProperty);
            ClearBindingIfOwnedByThis(tabItem, FontWeightProperty);
        }

        base.ClearContainerForItemOverride(element, item);
    }

    #endregion

    #region ### Private Methods ###

    /// <summary>
    /// Coerces the tab content visibility to disabled for navigation tabs.
    /// </summary>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <param name="baseValue">The base value.</param>
    /// <returns>Always <see langword="false"/>.</returns>
    [SuppressMessage(
        "Performance",
        "CA1859:Use concrete types when possible",
        Justification = "The WPF CoerceValueCallback delegate requires this exact signature.")]
    private static object CoerceShowTabContent(DependencyObject dependencyObject, object baseValue)
    {
        return false;
    }


    /// <summary>
    /// Clears a binding if it was assigned by this control.
    /// </summary>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <param name="dependencyProperty">The dependency property.</param>
    private static void ClearBindingIfOwnedByThis(DependencyObject dependencyObject, DependencyProperty dependencyProperty)
    {
        BindingExpression? bindingExpression = BindingOperations.GetBindingExpression(
            dependencyObject,
            dependencyProperty);

        if (bindingExpression?.ParentBinding.Source is XNavigationTabControl)
        {
            BindingOperations.ClearBinding(dependencyObject, dependencyProperty);
        }
    }

    #endregion
}
#endregion
