using Avalonia.Controls;
using Avalonia.Input;

namespace Melnikov.Ui;

public partial class SignInView : UserControl
{
    public SignInView()
    {
        InitializeComponent();
        Loaded += (_, _) => LoginOrEmailTextBox.Focus();
    }

    private void LoginOrEmailTextBoxOnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
        {
            return;
        }

        PasswordTextBox.Focus();
        e.Handled = true;
    }
}
