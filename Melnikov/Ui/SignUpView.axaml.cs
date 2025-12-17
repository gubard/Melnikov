using Avalonia.Controls;
using Avalonia.Input;

namespace Melnikov.Ui;

public partial class SignUpView : UserControl
{
    public SignUpView()
    {
        InitializeComponent();
    }

    private void LoginTextBoxOnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
        {
            return;
        }

        EmailTextBox.Focus();
        e.Handled = true;
    }

    private void EmailTextBoxOnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
        {
            return;
        }

        PasswordTextBox.Focus();
        e.Handled = true;
    }

    private void PasswordTextBoxOnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
        {
            return;
        }

        RepeatPasswordTextBox.Focus();
        e.Handled = true;
    }
}
