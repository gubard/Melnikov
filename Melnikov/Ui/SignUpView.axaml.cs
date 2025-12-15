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
    }

    private void EmailTextBoxOnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
        {
            return;
        }

        PasswordTextBox.Focus();
    }

    private void PasswordTextBoxOnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
        {
            return;
        }

        RepeatPasswordTextBox.Focus();
    }

    private void RepeatPasswordTextBoxOnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
        {
            return;
        }

        SignUpButton.Command?.Execute(SignUpButton.CommandParameter);
    }
}
