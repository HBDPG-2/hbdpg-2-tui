/*  Copyright (C) 2025 Piotr Kniaz

    This file is part of HBDPG-2.
    Repository: https://github.com/HBDPG-2/hbdpg-2-tui

    Licensed under the MIT License. See LICENSE file in the project root for details.
*/

using System.Reflection;
using Terminal.Gui;

// namespace

public partial class MainWindow : Window
{
    private static Label _appName = new()
    {
        Text = "HBDPG-2\n(Hashing-based Deterministic Password Generator - 2nd Gen)",
        ColorScheme = new()
        {
            Normal = Application.Driver.MakeAttribute(Color.White, Color.Magenta)
        },
        TextAlignment = TextAlignment.Centered,
        Width = Dim.Fill()
    };

    private static Label _appDescription = new()
    {
        Text = $"App version: {Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                               .InformationalVersion.Split('+')[0] ?? "unknown"}"
               + "\nCore version: 1.0-beta"
               + "\n(C) 2025 Piotr Kniaz. MIT license",
        TextAlignment = TextAlignment.Centered,
        Width = Dim.Fill(),
        Y = Pos.Bottom(_appName)
    };

    private static Label _passphrase1Label = new()
    {
        Text = "Passphrase 1:",
        X = 1,
        Y = Pos.Bottom(_appDescription) + 1
    };

    private static TextField _passphrase1Input = new("")
    {
        Secret = true,
        X = Pos.Right(_passphrase1Label) + 1,
        Y = Pos.Bottom(_appDescription) + 1,
        Width = Dim.Fill() - 1
    };

    private static Label _passphrase2Label  = new()
    {
        Text = "Passphrase 2:",
        X = 1,
        Y = Pos.Bottom(_passphrase1Label) + 1
    };

    private static TextField _passphrase2Input = new("")
    {
        Secret = true,
        X = Pos.Right(_passphrase2Label) + 1,
        Y = Pos.Top(_passphrase2Label),
        Width = Dim.Fill() - 1
    };

    private static Label _passwordLengthLabel = new()
    {
        Text = "Password length:",
        X = 1,
        Y = Pos.Bottom(_passphrase2Label) + 1
    };

    private static TextField _passwordLengthInput = new("32")
    {
        X = Pos.Right(_passwordLengthLabel) + 1,
        Y = Pos.Top(_passwordLengthLabel),
        Width = Dim.Fill() - 1
    };

    private static Button _generateButton = new()
    {
        Text = "Generate",
        X = Pos.Center(),
        Y = Pos.Bottom(_passwordLengthLabel) + 1,
        IsDefault = true
    };

    private static Label _generateLabel = new()
    {
        Text = "Generating...",
        Visible = false,
        X = Pos.Center(),
        Y = Pos.Bottom(_passwordLengthLabel) + 1,
    };

    private static Button _clearButton = new()
    {
        Text = "Clear fields",
        Visible = false,
        X = Pos.Center(),
        Y = Pos.Bottom(_generateButton)
    };

    private static Label _autoClearLabel = new()
    {
        Text = "Autoclear in 60 s",
        Visible = false,
        X = Pos.Center(),
        Y = Pos.Bottom(_clearButton)
    };

    private static Label _resultLabel = new()
    {
        Text = "Result:",
        Visible = false,
        X = Pos.Center(),
        Y = Pos.Bottom(_autoClearLabel) + 1
    };

    private static TextField _resultField = new("")
    {
        ReadOnly = true,
        Secret = true,
        Visible = false,
        TextAlignment = TextAlignment.Right,
        X = Pos.Center(),
        Y = Pos.Bottom(_resultLabel),
        Width = Dim.Fill(),
        ColorScheme = new()
        {
            Normal = Application.Driver.MakeAttribute(Color.White, Color.Magenta),
            Focus = Application.Driver.MakeAttribute(Color.Gray, Color.Magenta),
            HotNormal = Application.Driver.MakeAttribute(Color.White, Color.Magenta),
            HotFocus = Application.Driver.MakeAttribute(Color.Gray, Color.Magenta),
            Disabled = Application.Driver.MakeAttribute(Color.White, Color.Magenta)
        }
    };

    private static CheckBox _showPasswordCheckbox = new()
    {
        Text = "Show password",
        Checked = false,
        Visible = false,
        X = Pos.Center(),
        Y = Pos.Bottom(_resultField)
    };

    private static Label _entropyLabel = new()
    {
        Text = "Entropy: 0.00 bits",
        Visible = false,
        X = 1,
        Y = Pos.Bottom(_showPasswordCheckbox)
    };

    private static Label _elapsedTimeLabel = new()
    {
        Text = "Elapsed time: 0.000 s",
        Visible = false,
        X = 1,
        Y = Pos.Bottom(_entropyLabel)
    };

    private static Label _passwordCopiedLabel = new()
    {
        Text = "Password copied to clipboard",
        Visible = false,
        X = Pos.Center(),
        Y = Pos.Bottom(_elapsedTimeLabel)
    };
}