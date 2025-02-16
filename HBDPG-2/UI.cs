using System.Diagnostics.CodeAnalysis;
using Terminal.Gui;

// namespace

public class MainWindow : Window
{
    public MainWindow()
    {
        Title = "HBDPG-2 (Ctrl+Q to quit)";

        ColorScheme = new ColorScheme
        {
            Normal = Application.Driver.MakeAttribute(Color.White, Color.Black),
            Focus = Application.Driver.MakeAttribute(Color.Black, Color.Gray),
            HotNormal = Application.Driver.MakeAttribute(Color.Cyan, Color.Black),
            HotFocus = Application.Driver.MakeAttribute(Color.Black, Color.Cyan)
        };

        InitializeUI();
        SetupEvents();
    }

    [MemberNotNull(nameof(_appDescription), nameof(_passphrase1Label), nameof(_passphrase1Input),
        nameof(_passphrase2Label), nameof(_passphrase2Input), nameof(_passwordLengthLabel),
        nameof(_passwordLengthInput), nameof(_generateButton), nameof(_resultLabel), nameof(_resultField),
        nameof(_showPasswordCheckbox), nameof(_entropyLabel), nameof(_elapsedTimeLabel))]
    private void InitializeUI()
    {
        _appDescription = new()
        {
            Text = "HBDPG-2\n(Hashing-based Deterministic Password Generator - 2nd Gen)" +
                "\nApp version: 0.1.0\nCore version: 1.0-beta" +
                "\n\n(C) 2025 Piotr Kniaz. MIT license",
            TextAlignment = TextAlignment.Centered,
            Width = Dim.Fill()
        };

        _passphrase1Label = new()
        {
            Text = "Passphrase 1:",
            X = 1,
            Y = Pos.Bottom(_appDescription) + 2
        };

        _passphrase1Input = new("")
        {
            Secret = true,
            X = Pos.Right(_passphrase1Label) + 1,
            Y = Pos.Bottom(_appDescription) + 2,
            Width = Dim.Fill() - 1
        };

        _passphrase2Label = new()
        {
            Text = "Passphrase 2:",
            X = 1,
            Y = Pos.Bottom(_passphrase1Label) + 1
        };

        _passphrase2Input = new("")
        {
            Secret = true,
            X = Pos.Right(_passphrase2Label) + 1,
            Y = Pos.Top(_passphrase2Label),
            Width = Dim.Fill() - 1
        };

        _passwordLengthLabel = new()
        {
            Text = "Password length:",
            X = 1,
            Y = Pos.Bottom(_passphrase2Label) + 1
        };

        _passwordLengthInput = new("32")
        {
            X = Pos.Right(_passwordLengthLabel) + 1,
            Y = Pos.Top(_passwordLengthLabel),
            Width = Dim.Fill() - 1
        };

        _generateButton = new()
        {
            Text = "Generate",
            X = Pos.Center(),
            Y = Pos.Bottom(_passwordLengthLabel) + 1,
            IsDefault = true
            // Border = new Border() { BorderStyle = BorderStyle.Double }
        };

        // _generateLabel = new()
        // {
        //     Text = "Generating...",
        //     Visible = false,
        //     X = Pos.Center(),
        //     Y = Pos.Bottom(_passwordLengthLabel) + 1,
        // };

        _resultLabel = new()
        {
            Text = "Result:",
            Visible = false,
            X = Pos.Center(),
            Y = Pos.Bottom(_generateButton) + 2
        };

        _resultField = new("")
        {
            ReadOnly = true,
            Secret = true,
            Visible = false,
            TextAlignment = TextAlignment.Right,
            X = Pos.Center(),
            Y = Pos.Bottom(_resultLabel),
            Width = Dim.Fill()
        };

        _showPasswordCheckbox = new()
        {
            Text = "Show password",
            Checked = false,
            Visible = false,
            X = Pos.Center(),
            Y = Pos.Bottom(_resultField)
        };

        _entropyLabel = new()
        {
            Text = "Entropy: 0.00 bits",
            Visible = false,
            X = 1,
            Y = Pos.Bottom(_showPasswordCheckbox)
        };

        _elapsedTimeLabel = new()
        {
            Text = "Elapsed time: 0.000 s",
            Visible = false,
            X = 1,
            Y = Pos.Bottom(_entropyLabel)
        };

        Add(_appDescription, _passphrase1Label, _passphrase1Input, _passphrase2Label, _passphrase2Input,
            _passwordLengthLabel, _passwordLengthInput, _generateButton, _resultLabel, _resultField,
            _showPasswordCheckbox, _entropyLabel, _elapsedTimeLabel);
    }

    private void SetupEvents()
    {
        _passwordLengthInput.TextChanging += OnPasswordLengthChanged;
        _generateButton.Clicked += Generate;
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
            MessageBox.ErrorQuery("Passphrase is too short", "Passphrases must be at least 8 characters long.", "Ok");
        }
        else if (_passwordLength < 16 || _passwordLength > 64)
        {
            MessageBox.ErrorQuery("Invalid password length", $"Number must be between 16 and 64.", "Ok");
        }
        else
        {
            Result result = Core.Generate(_passphrase1, _passphrase2, _passwordLength);
            
            if (Clipboard.TrySetClipboardData(result.Password))
            {
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
            }
        }
    }

    private Label _appDescription;
    private Label _passphrase1Label;
    private TextField _passphrase1Input;
    private Label _passphrase2Label;
    private TextField _passphrase2Input;
    private Label _passwordLengthLabel;
    private TextField _passwordLengthInput;
    private Button _generateButton;
    // private Label _generateLabel;
    private Label _resultLabel;
    private TextField _resultField;
    private CheckBox _showPasswordCheckbox;
    private Label _entropyLabel;
    private Label _elapsedTimeLabel;

    private string _passphrase1 = string.Empty;
    private string _passphrase2 = string.Empty;
    private int _passwordLength;
}