using Terminal.Gui;

// namespace

public class MainWindow : Window
{
    public MainWindow()
    {
        Title = "HBDPG-2 (Ctrl+Q to quit)";

        Label appDescription = new()
        {
            Text = "HBDPG-2\n(Hashing-based Deterministic Password Generator - 2nd Gen)",
            TextAlignment = TextAlignment.Centered,
            Width = Dim.Fill()
        };

        // Application.Top.ColorScheme = new ColorScheme
        // {
        //     Normal = Application.Driver.MakeAttribute(Color.White, Color.Black),
        //     Focus = Application.Driver.MakeAttribute(Color.Black, Color.Gray),
        //     HotNormal = Application.Driver.MakeAttribute(Color.Cyan, Color.Black),
        //     HotFocus = Application.Driver.MakeAttribute(Color.Black, Color.Cyan)
        // };

        Label passphrase1Label = new()
        {
            Text = "Passphrase 1:",
            X = 1,
            Y = Pos.Bottom(appDescription) + 1
        };

        TextField passphrase1Input = new("")
        {
            Secret = true,
            // Position text field adjacent to the label
            X = Pos.Right(passphrase1Label) + 1,
            Y = Pos.Bottom(appDescription) + 1,

            // Fill remaining horizontal space
            Width = Dim.Fill() - 1
        };

        Label passphrase2Label = new()
        {
            Text = "Passphrase 2:",
            X = Pos.Left(passphrase1Label),
            Y = Pos.Bottom(passphrase1Label) + 1
        };

        TextField passphrase2Input = new("")
        {
            Secret = true,
            // align with the text box above
            X = Pos.Right(passphrase2Label) + 1,
            Y = Pos.Top(passphrase2Label),
            Width = Dim.Fill() - 1
        };

        Label passwordLengthLabel = new()
        {
            Text = "Password length:",
            X = Pos.Left(passphrase2Label),
            Y = Pos.Bottom(passphrase2Label) + 1
        };

        TextField passwordLengthInput = new("32")
        {
            X = Pos.Right(passwordLengthLabel) + 1,
            Y = Pos.Top(passwordLengthLabel),
            Width = Dim.Fill() - 1
        };

        passwordLengthInput.TextChanging += (args) =>
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
        };

        // Create generate button
        Button generateButton = new()
        {
            Text = "Generate",
            Y = Pos.Bottom(passwordLengthLabel) + 1,
            // center the login button horizontally
            X = Pos.Center(),
            IsDefault = true
        };

        Label resultLabel = new()
        {
            Text = "Result:",
            Visible = false,
            X = Pos.Left(passwordLengthLabel),
            Y = Pos.Bottom(generateButton) + 2
        };

        TextField resultField = new("")
        {
            ReadOnly = true,
            Secret = true,
            Visible = false,
            TextAlignment = TextAlignment.Right,
            X = Pos.Left(resultLabel),
            Y = Pos.Bottom(resultLabel),
            Width = Dim.Fill()
        };

        CheckBox showPasswordCheckbox = new()
        {
            Text = "Show password",
            Checked = false,
            Visible = false,
            X = Pos.Center(),
            Y = Pos.Bottom(resultField)
        };

        Label entropyLabel = new()
        {
            Text = "Entropy: 0.00 bits",
            Visible = false,
            X = Pos.Left(resultLabel),
            Y = Pos.Bottom(showPasswordCheckbox)
        };

        Label elapsedTimeLabel = new()
        {
            Text = "Elapsed time: 0.000 s",
            Visible = false,
            X = Pos.Left(entropyLabel),
            Y = Pos.Bottom(entropyLabel)
        };

        generateButton.Clicked += () =>
        {
            _passphrase1 = passphrase1Input.Text.ToString() ?? string.Empty;
            _passphrase2 = passphrase2Input.Text.ToString() ?? string.Empty;

            if (int.TryParse(passwordLengthInput.Text.ToString(), out int number))
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

                // MessageBox.Query("Result", result.Password, "Ok");

                if (Clipboard.TrySetClipboardData(result.Password))
                {
                    // MessageBox.Query("Password copied to Clipboard", $"Entropy: {result.Entropy.ToString("F2")} bits\n"
                    //     + $"Elapsed time: {result.ElapsedTime} s", "Ok");

                    resultLabel.Visible = true;
                    resultField.Text = result.Password;
                    resultField.Width = _passwordLength;
                    resultField.X = Pos.Center();
                    resultField.Visible = true;

                    showPasswordCheckbox.Visible = true;

                    entropyLabel.Text = $"Entropy: {result.Entropy.ToString("F2")} bits";
                    entropyLabel.Visible = true;

                    elapsedTimeLabel.Text = $"Elapsed time: {result.ElapsedTime} s";
                    elapsedTimeLabel.Visible = true;
                }
            }
        };

        showPasswordCheckbox.Toggled += (arg) =>
        {
            if (showPasswordCheckbox.Checked)
            {
                resultField.Secret = false;
            }
            else
            {
                resultField.Secret = true;
            }
        };

        // Add the views to the Window
        Add(appDescription, passphrase1Label, passphrase1Input, passphrase2Label, passphrase2Input, passwordLengthLabel,
            passwordLengthInput, generateButton, resultLabel, resultField, showPasswordCheckbox, entropyLabel, elapsedTimeLabel);
    }

    private string _passphrase1 = string.Empty;
    private string _passphrase2 = string.Empty;
    private int _passwordLength;
}