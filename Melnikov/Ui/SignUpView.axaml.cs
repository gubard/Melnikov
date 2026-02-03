using Avalonia.Controls;
using Avalonia.Input;
using Inanna.Helpers;

namespace Melnikov.Ui;

public partial class SignUpView : UserControl
{
    public SignUpView()
    {
        InitializeComponent();
        Loaded += (_, _) => LoginTextBox.FocusCaretIndex();
    }

    private void LoginTextBoxOnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
        {
            return;
        }

        EmailTextBox.FocusCaretIndex();
        e.Handled = true;
    }

    private void EmailTextBoxOnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
        {
            return;
        }

        PasswordTextBox.FocusCaretIndex();
        e.Handled = true;
    }

    private void PasswordTextBoxOnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
        {
            return;
        }

        RepeatPasswordTextBox.FocusCaretIndex();
        e.Handled = true;
    }
}
