using Avalonia.Controls;
using Avalonia.Input;
using Inanna.Helpers;

namespace Melnikov.Ui;

public sealed partial class SignInView : UserControl
{
    public SignInView()
    {
        InitializeComponent();
        Loaded += (_, _) => LoginOrEmailTextBox.FocusCaretIndex();
    }

    private void LoginOrEmailTextBoxOnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
        {
            return;
        }

        PasswordTextBox.FocusCaretIndex();
        e.Handled = true;
    }
}
