using Terminal.Gui;

// namespace

public class MainWindow : Window
{
    public MainWindow()
    {
        Title = "HBDPG-2 (Ctrl+Q to quit)";

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
            Y = 1
        };

        TextField passphrase1Input = new("")
        {
            Secret = true,
            // Position text field adjacent to the label
            X = Pos.Right(passphrase1Label) + 1,
            Y = 1,

            // Fill remaining horizontal space
            Width = Dim.Fill()
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
            Width = Dim.Fill()
        };

        Label passwordLengthLabel = new()
        {
            Text = "Password length:",
            X = Pos.Left(passphrase2Label),
            Y = Pos.Bottom(passphrase2Label) + 1
        };

        TextField passwordLengthInput = new("")
        {
            X = Pos.Right(passwordLengthLabel) + 1,
            Y = Pos.Top(passwordLengthLabel),
            Width = Dim.Fill()
        };

        passwordLengthInput.TextChanging += (args) =>
        {
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

        // When login button is clicked display a message popup
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
                MessageBox.ErrorQuery("Passphrase too short", "Passphrases must be at least 8 characters long.", "Ok");
            }
            else if (_passwordLength < 16 || _passwordLength > 64)
            {
                MessageBox.ErrorQuery("Invalid password length", $"Number must be between 16 and 64.", "Ok");
            }
            else
            {
                Result result = Core.Generate(_passphrase1, _passphrase2, _passwordLength == 0 ? 32 : _passwordLength);

                MessageBox.Query("Result", result.Password, "Ok");
            }
            // if (passphrase1Input.Text == "password" && passphrase2Input.Text == "12345678") {
            //     MessageBox.Query("Logging In", "Login Successful", "Ok");
            //     Application.RequestStop();
            // } else {
            //     MessageBox.ErrorQuery("Logging In", "Incorrect username or password", "Ok");
            // }
        };

        // Add the views to the Window
        Add(passphrase1Label, passphrase1Input, passphrase2Label, passphrase2Input,
            passwordLengthLabel, passwordLengthInput, generateButton);
    }

    private string _passphrase1 = string.Empty;
    private string _passphrase2 = string.Empty;
    private int _passwordLength;
}