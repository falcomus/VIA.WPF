using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

/// <summary>Generic editor for a set of named application color tokens.</summary>
public class XColorSchemeEditor : Control
{
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        nameof(ItemsSource), typeof(IEnumerable), typeof(XColorSchemeEditor), new FrameworkPropertyMetadata(null));
    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
        nameof(SelectedItem), typeof(XColorToken), typeof(XColorSchemeEditor), new FrameworkPropertyMetadata(null));

    static XColorSchemeEditor() => DefaultStyleKeyProperty.OverrideMetadata(typeof(XColorSchemeEditor), new FrameworkPropertyMetadata(typeof(XColorSchemeEditor)));

    public IEnumerable? ItemsSource { get => (IEnumerable?)GetValue(ItemsSourceProperty); set => SetValue(ItemsSourceProperty, value); }
    public XColorToken? SelectedItem { get => (XColorToken?)GetValue(SelectedItemProperty); set => SetValue(SelectedItemProperty, value); }
}
