using Terminal.Gui;

// namespace

public partial class MainWindow : Window
{
    public MainWindow()
    {
        Title = "HBDPG-2 (Ctrl+Q to quit)";
        Colors.Error = new ColorScheme {
            Normal = Application.Driver.MakeAttribute(Color.White, Color.Red),
            Focus = Application.Driver.MakeAttribute(Color.Black, Color.Gray),
            HotNormal = Application.Driver.MakeAttribute(Color.White, Color.Red),
            HotFocus = Application.Driver.MakeAttribute(Color.Black, Color.Gray)
        };

        ColorScheme = new ColorScheme
        {
            Normal = Application.Driver.MakeAttribute(Color.White, Color.Black),
            Focus = Application.Driver.MakeAttribute(Color.Black, Color.Gray),
            HotNormal = Application.Driver.MakeAttribute(Color.White, Color.Black),
            HotFocus = Application.Driver.MakeAttribute(Color.Black, Color.Gray)
        };

        InitializeUI();
        SetupEvents();
    }

    private void InitializeUI()
    {
        if (Clipboard.IsSupported)
        {
            _clearButton.Text = "Clear fields and clipboard";
        }

        Add(_appName, _appDescription, _passphrase1Label, _passphrase1Input, _passphrase2Label, _passphrase2Input,
            _passwordLengthLabel, _passwordLengthInput, _generateButton, _clearButton, _resultLabel, _resultField,
            _showPasswordCheckbox, _entropyLabel, _elapsedTimeLabel);
    }

    private void SetupEvents()
    {
        _passwordLengthInput.TextChanging += OnPasswordLengthChanged;
        _generateButton.Clicked += Generate;
        _clearButton.Clicked += ClearFieldsAndClipboard;
        _showPasswordCheckbox.Toggled += ShowPassword;
    }

    private void OnPasswordLengthChanged(TextChangingEventArgs args)
    {
        if (args.NewText.Length > 2)
        {
            args.Cancel = true;
            return;
        }

        foreach (char c in args.NewText.Select(v => (char)v))
        {
            if (!char.IsDigit(c))
            {
                args.Cancel = true;
                return;
            }
        }
    }

    private void ShowPassword(bool newState)
    {
        if (_showPasswordCheckbox.Checked)
        {
            _resultField.Secret = false;
        }
        else
        {
            _resultField.Secret = true;
        }
    }

    private void Generate()
    {
        _passphrase1 = _passphrase1Input.Text.ToString() ?? string.Empty;
        _passphrase2 = _passphrase2Input.Text.ToString() ?? string.Empty;

        if (int.TryParse(_passwordLengthInput.Text.ToString(), out int number))
        {
            _passwordLength = number;
        }

        if (_passphrase1.Length < 8 || _passphrase2.Length < 8)
        {
            MessageBox.ErrorQuery("Passphrase is too short", "Passphrases must be at least 8 characters long.", "OK");
        }
        else if (_passwordLength < 16 || _passwordLength > 64)
        {
            MessageBox.ErrorQuery("Invalid password length", $"Number must be between 16 and 64.", "OK");
        }
        else
        {
            Result result = Core.Generate(_passphrase1, _passphrase2, _passwordLength);

            _clearButton.Visible = true;

            _resultLabel.Visible = true;
            _resultField.Text = result.Password;
            _resultField.Width = _passwordLength;
            _resultField.X = Pos.Center();
            _resultField.Visible = true;

            _showPasswordCheckbox.Visible = true;

            _entropyLabel.Text = $"Entropy: {result.Entropy:f2} bits";
            _entropyLabel.Visible = true;

            _elapsedTimeLabel.Text = $"Elapsed time: {result.ElapsedTime:f3} s";
            _elapsedTimeLabel.Visible = true;

            if (Clipboard.TrySetClipboardData(result.Password))
            {
                _resultField.CanFocus = false;
            }
            else
            {
                
            }
        }
    }

    private void ClearFieldsAndClipboard()
    {
        if (Clipboard.TryGetClipboardData(out string data) && data == _resultField.Text)
        {
            Clipboard.TrySetClipboardData(string.Empty);
        }

        _passphrase1 = string.Empty;
        _passphrase2 = string.Empty;
        _passwordLength = 0;

        _passphrase1Input.Text = string.Empty;
        _passphrase2Input.Text = string.Empty;
        _passwordLengthInput.Text = "32";

        _resultField.Text = string.Empty;
        _entropyLabel.Text = "Entropy: 0.00 bits";
        _elapsedTimeLabel.Text = "Elapsed time: 0.000 s";

        _resultLabel.Visible = false;
        _resultField.Visible = false;
        _showPasswordCheckbox.Visible = false;
        _entropyLabel.Visible = false;
        _elapsedTimeLabel.Visible = false;

        _passphrase1Input.SetFocus();
        _clearButton.Visible = false;
    }
}