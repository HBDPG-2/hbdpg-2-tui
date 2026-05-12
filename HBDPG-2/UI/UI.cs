/*  Copyright (C) 2025-2026 Piotr Kniaz

    This file is part of HBDPG-2.
    Repository: https://github.com/HBDPG-2/hbdpg-2-tui

    Licensed under the MIT License. See LICENSE file in the project root for details.
*/

using Terminal.Gui.Views;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Drawing;
using Terminal.Gui.App;
using Terminal.Gui.Input;
using HBDPG2.Core;

namespace HBDPG2.UI;

public partial class MainWindow : Window
{
    public MainWindow(IApplication app)
    {
        _app = app;

        Title = "HBDPG-2 (Ctrl+Q to quit)";

        // Colors.Error = new ColorScheme {
        //     Normal = new Terminal.Gui.Drawing.Attribute(Color.White, Color.Red),
        //     Focus = new Terminal.Gui.Drawing.Attribute(Color.Black, Color.Gray),
        //     HotNormal = new Terminal.Gui.Drawing.Attribute(Color.White, Color.Red),
        //     HotFocus = new Terminal.Gui.Drawing.Attribute(Color.Black, Color.Gray)
        // };

        // ColorScheme = new ColorScheme
        // {
        //     Normal = new Terminal.Gui.Drawing.Attribute(Color.White, Color.Black),
        //     Focus = new Terminal.Gui.Drawing.Attribute(Color.Black, Color.Gray),
        //     HotNormal = new Terminal.Gui.Drawing.Attribute(Color.White, Color.Black),
        //     HotFocus = new Terminal.Gui.Drawing.Attribute(Color.Black, Color.Gray)
        // };

        InitializeUI();
        SetupEvents();
    }

    private void InitializeUI()
    {
        Add(
            _appNameLabel,
            _appDescriptionLabel,
            _passphrase1Label,
            _passphrase1Input,
            _passphrase2Label,
            _passphrase2Input,
            _passwordLengthLabel,
            _passwordLengthInput,
            _generateButton,
            _generateLabel,
            _clearButton,
            _autoClearLabel,
            _resultLabel,
            _resultField,
            _showPasswordCheckbox,
            _entropyLabel,
            _elapsedTimeLabel,
            _passwordCopiedLabel
        );
        
        if (_app.Clipboard?.IsSupported ?? false)
        {
            _clearButton.Text = "Clear fields and clipboard";
        }
        else
        {
            MessageBox.ErrorQuery(
                _app,
                "Clipboard is not supported",
                "You may encounter problems when trying to copy!",
                "OK"
            );
        }
    }

    private void SetupEvents()
    {
        _passwordLengthInput.TextChanging += OnPasswordLengthChanged;
        _generateButton.Accepted += OnGenerateButtonClicked;
        _clearButton.Accepted += OnClearButtonClicked;
        _showPasswordCheckbox.ValueChanged += ShowPassword;

        // Closing += OnClosed;
    }

    private void OnPasswordLengthChanged(object? sender, ResultEventArgs<string> args)
    {
        if (args.Result is null) return;

        if (args.Result.Length > 2)
        {
            args.Handled = true;
            // args.Cancel = true;
            return;
        }

        foreach (char c in args.Result)
        {
            if (!char.IsDigit(c))
            {
                args.Handled = true;
                // args.Cancel = true;
                return;
            }
        }
    }

    private void OnGenerateButtonClicked(object? sender, CommandEventArgs args) => Generate();

    private void OnClearButtonClicked(object? sender, CommandEventArgs args) => ClearFieldsAndClipboard();

    private void ShowPassword(object? sender, ValueChangedEventArgs<CheckState> args)
    {
        if (_showPasswordCheckbox.Value is CheckState.Checked)
        {
            _resultField.Secret = false;
        }
        else
        {
            _resultField.Secret = true;
        }
    }

    private async void Generate()
    {
        _passphrase1 = _passphrase1Input.Text.ToString() ?? string.Empty;
        _passphrase2 = _passphrase2Input.Text.ToString() ?? string.Empty;

        if (int.TryParse(_passwordLengthInput.Text.ToString(), out int number))
        {
            _passwordLength = number;
        }

        if (_passphrase1.Length < 8 || _passphrase2.Length < 8)
        {
            MessageBox.ErrorQuery(
                _app,
                "Passphrase is too short",
                "Passphrases must be at least 8 characters long.",
                "OK"
            );
        }
        else if (_passwordLength < 16 || _passwordLength > 64)
        {
            MessageBox.ErrorQuery(
                _app,
                "Invalid password length",
                "Number must be between 16 and 64.",
                "OK"
            );
        }
        else
        {
            _generateButton.Visible = false;
            _generateLabel.Visible = true;
            _autoClearRemaining = 60;
            _autoClearLabel.Text = $"Autoclear in {_autoClearRemaining} s";
            _autoClearLabel.Visible = false;
            _clearButton.Visible = false;
            if (_timer is not null)
            {
                _app.RemoveTimeout(_timer);
            }

            Result result = await Task.Run(() => Core.Core.Generate(_passphrase1, _passphrase2, _passwordLength));

            _generateLabel.Visible = false;
            _generateButton.Visible = true;

            if (result.Password is null)
            {
                MessageBox.ErrorQuery(
                    _app,
                    "Failed to generate secure password",
                    "Try another passphrases or password length.", "OK"
                );
                ClearFieldsAndClipboard();
                return;
            }

            _clearButton.Visible = true;
            _autoClearLabel.Visible = true;

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

            _timer = _app.AddTimeout(TimeSpan.FromSeconds(1), Counter);

            if (_app.Clipboard?.TrySetClipboardData(result.Password) ?? false)
            {
                _resultField.CanFocus = false;
                _passwordCopiedLabel.Visible = true;
            }
            else
            {
                
            }
        }
    }

    private void ClearFieldsAndClipboard()
    {
        if (_app.Clipboard?.TryGetClipboardData(out string data) ?? false && data == _resultField.Text)
        {
            _app.Clipboard.TrySetClipboardData(string.Empty);
        }

        _passphrase1 = string.Empty;
        _passphrase2 = string.Empty;
        _passwordLength = 0;

        _passphrase1Input.Text = string.Empty;
        _passphrase2Input.Text = string.Empty;
        _passwordLengthInput.Text = "32";

        if (_timer is not null)
        {
            _app.RemoveTimeout(_timer);
        }
        _autoClearRemaining = 60;
        _autoClearLabel.Text = $"Autoclear in {_autoClearRemaining} s";
        _resultField.Text = string.Empty;
        _entropyLabel.Text = "Entropy: 0.00 bits";
        _elapsedTimeLabel.Text = "Elapsed time: 0.000 s";

        _autoClearLabel.Visible = false;
        _resultLabel.Visible = false;
        _resultField.Visible = false;
        _showPasswordCheckbox.Visible = false;
        _entropyLabel.Visible = false;
        _elapsedTimeLabel.Visible = false;
        _passwordCopiedLabel.Visible = false;

        _passphrase1Input.SetFocus();
        _clearButton.Visible = false;
    }

    private void OnClosed(EventArgs args)
    {
        ClearFieldsAndClipboard();
    }

    private bool Counter()
    {
        if (_autoClearRemaining != 0)
        {
            _autoClearLabel.Text = $"Autoclear in {--_autoClearRemaining} s";
            return true;
        }
        else
        {
            ClearFieldsAndClipboard();
            return false;
        }
    }

    private string _passphrase1 = string.Empty;
    private string _passphrase2 = string.Empty;
    private int _passwordLength;

    private readonly IApplication _app;
    private object? _timer;
    private int _autoClearRemaining = 60;
}