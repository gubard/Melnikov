using Avalonia.Controls;
using Avalonia.Input;

namespace Melnikov.Ui;

public partial class SignInView : UserControl
{
    public SignInView()
    {
        InitializeComponent();
    }

    private void LoginOrEmailTextBoxOnKeyDown(object? sender, KeyEventArgs e)
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

        SignInButton.Command?.Execute(SignInButton.CommandParameter);
    }
}
