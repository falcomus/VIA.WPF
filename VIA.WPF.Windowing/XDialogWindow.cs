using System.Windows;

namespace VIA.WPF.Windowing;

/// <summary>
/// Provides the visual defaults for a modal application dialog.
/// Dialog presentation, ownership and dim overlays are handled by <see cref="XDialogService"/>.
/// </summary>
public class XDialogWindow : XWindow
{
    static XDialogWindow()
    {
        UseWindowShadowProperty.OverrideMetadata(
            typeof(XDialogWindow),
            new FrameworkPropertyMetadata(true));
    }
}
