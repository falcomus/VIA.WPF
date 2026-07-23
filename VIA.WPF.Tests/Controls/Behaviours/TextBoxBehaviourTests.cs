// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBoxBehaviourTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using VIA.WPF.Behaviors;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Behaviours;

#region ### Class TextBoxBehaviourTests ###
/// <summary>
/// Tests text box related VIA.WPF behaviors.
/// </summary>
public sealed class TextBoxBehaviourTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that <see cref="TextBoxCommitOnEnterBehavior" /> updates the text binding source when Enter is pressed.
    /// </summary>
    [Fact]
    public void TextBoxCommitOnEnterBehavior_ShouldUpdateSourceOnEnter()
    {
        WpfTestHelper.Run(
            () =>
            {
                TextBoxBindingSource source = new()
                {
                    Value = "Old"
                };
                TextBox textBox = new();
                textBox.SetBinding(
                    TextBox.TextProperty,
                    new Binding(nameof(TextBoxBindingSource.Value))
                    {
                        Source = source,
                        UpdateSourceTrigger = UpdateSourceTrigger.Explicit
                    });

                TextBoxCommitOnEnterBehavior.SetIsEnabled(textBox, true);
                textBox.Text = "New";

                using HwndSource inputSource = CreateInputSource();
                KeyEventArgs args = CreateKeyEventArgs(inputSource, Key.Enter);

                textBox.RaiseEvent(args);

                Assert.Equal("New", source.Value);
                Assert.True(args.Handled);
            });
    }

    /// <summary>
    /// Verifies that <see cref="TextBoxCommitOnEnterBehavior" /> does not commit multiline text boxes.
    /// </summary>
    [Fact]
    public void TextBoxCommitOnEnterBehavior_ShouldIgnoreEnterWhenTextBoxAcceptsReturn()
    {
        WpfTestHelper.Run(
            () =>
            {
                TextBoxBindingSource source = new()
                {
                    Value = "Old"
                };
                TextBox textBox = new()
                {
                    AcceptsReturn = true
                };
                textBox.SetBinding(
                    TextBox.TextProperty,
                    new Binding(nameof(TextBoxBindingSource.Value))
                    {
                        Source = source,
                        UpdateSourceTrigger = UpdateSourceTrigger.Explicit
                    });

                TextBoxCommitOnEnterBehavior.SetIsEnabled(textBox, true);
                textBox.Text = "New";

                using HwndSource inputSource = CreateInputSource();
                KeyEventArgs args = CreateKeyEventArgs(inputSource, Key.Enter);

                textBox.RaiseEvent(args);

                Assert.Equal("Old", source.Value);
            });
    }

    /// <summary>
    /// Verifies that <see cref="SelectAllTextBoxBehavior" /> selects all text when keyboard focus is received.
    /// </summary>
    [Fact]
    public void SelectAllTextBoxBehavior_ShouldSelectAllTextOnKeyboardFocus()
    {
        WpfTestHelper.Run(
            () =>
            {
                TextBox textBox = new()
                {
                    Text = "abcdef"
                };
                textBox.Select(2, 2);

                SelectAllTextBoxBehavior.SetIsEnabled(textBox, true);

                KeyboardFocusChangedEventArgs args = new(Keyboard.PrimaryDevice, 0, textBox, textBox)
                {
                    RoutedEvent = Keyboard.GotKeyboardFocusEvent
                };

                textBox.RaiseEvent(args);

                Assert.Equal(0, textBox.SelectionStart);
                Assert.Equal(textBox.Text.Length, textBox.SelectionLength);
            });
    }

    /// <summary>
    /// Verifies that focus behaviors can process their dispatcher callbacks without requiring a loaded window.
    /// </summary>
    [Fact]
    public void FocusBehaviors_ShouldProcessDispatcherCallbacksWithoutErrors()
    {
        WpfTestHelper.Run(
            () =>
            {
                Button loadedButton = new()
                {
                    Focusable = true
                };
                Button visibleButton = new()
                {
                    Focusable = true
                };

                FocusOnLoadedBehavior.SetIsEnabled(loadedButton, true);
                FocusOnVisibleBehavior.SetIsEnabled(visibleButton, true);

                loadedButton.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
                WpfTestHelper.DoEvents();

                Assert.True(FocusOnLoadedBehavior.GetIsEnabled(loadedButton));
                Assert.True(FocusOnVisibleBehavior.GetIsEnabled(visibleButton));
            });
    }
    #endregion

    #region ### Private Methods ###
    private static HwndSource CreateInputSource()
    {
        return new HwndSource(new HwndSourceParameters("VIA.WPF.Tests")
        {
            Width = 1,
            Height = 1
        });
    }

    private static KeyEventArgs CreateKeyEventArgs(PresentationSource source, Key key)
    {
        return new KeyEventArgs(Keyboard.PrimaryDevice, source, 0, key)
        {
            RoutedEvent = Keyboard.KeyDownEvent
        };
    }
    #endregion

    #region ### Private Classes ###
    private sealed class TextBoxBindingSource : DependencyObject
    {
        #region ### Public Fields ###
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(string),
            typeof(TextBoxBindingSource),
            new PropertyMetadata(string.Empty));
        #endregion

        #region ### Public Properties ###
        public string? Value
        {
            get => (string?)this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }
        #endregion
    }
    #endregion
}
#endregion
