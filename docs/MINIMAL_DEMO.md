# VIA.WPF in 5 minutes

This document describes the intended minimal demo for VIA.WPF.

## Goal

Show the smallest useful VIA.WPF setup:

- theme resources
- one button
- one text input
- validation
- one small view model

## App.xaml

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="/VIA.WPF.Themes;component/Themes/Generic.xaml" />
            <ResourceDictionary Source="/VIA.WPF.Controls;component/Themes/Generic.xaml" />
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

The exact resource setup may vary depending on package structure and application bootstrapping.

## MainWindow.xaml

```xml
<via:XStackPanel
    xmlns:via="http://schemas.via.dev/wpf"
    Margin="24"
    Spacing="12">

    <via:XTextBox
        Header="Name"
        Placeholder="Enter name"
        Text="{Binding Name, UpdateSourceTrigger=PropertyChanged}" />

    <via:XValidationHintPopup ValidationSource="{Binding}" />

    <via:XButton
        Content="Save"
        Variant="Primary"
        Command="{Binding SaveCommand}" />

</via:XStackPanel>
```

## ViewModel sketch

```csharp
public sealed class CustomerEditorViewModel : XValidatableObject
{
    private string? name;

    public string? Name
    {
        get => this.name;
        set => this.SetProperty(ref this.name, value);
    }

    protected override Task ValidateCoreAsync(XValidationContext context, CancellationToken cancellationToken)
    {
        context.Required(this.Name, nameof(this.Name), XValidationText.Text("Name is required."));
        return Task.CompletedTask;
    }
}
```

## Next step

A real minimal sample application should be added later as a separate small demo, independent of the large control showcase.
