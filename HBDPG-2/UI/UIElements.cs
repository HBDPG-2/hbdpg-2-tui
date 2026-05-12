/*  Copyright (C) 2025-2026 Piotr Kniaz

    This file is part of HBDPG-2.
    Repository: https://github.com/HBDPG-2/hbdpg-2-tui

    Licensed under the MIT License. See LICENSE file in the project root for details.
*/

using System.Reflection;
using Terminal.Gui.Views;
using Terminal.Gui.Drawing;
using Terminal.Gui.ViewBase;
using Terminal.Gui.App;

namespace HBDPG2.UI;

public partial class MainWindow : Window
{
    private static readonly string _appVersion = Assembly.GetExecutingAssembly()
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
        .InformationalVersion.Split('+')[0] ?? "unknown";
    private static readonly Label _appNameLabel = new()
    {
        Text = "HBDPG-2\n(Hashing-based Deterministic Password Generator - 2nd Gen)",
        TextAlignment = Alignment.Center,
        Width = Dim.Fill()
    };

    private static readonly Label _appDescriptionLabel = new()
    {
        Text = $"App version: {_appVersion}"
               + "\nCore version: 1.0-beta"
               + "\n(C) 2025-2026 Piotr Kniaz. MIT license",
        TextAlignment = Alignment.Center,
        Width = Dim.Fill(),
        Y = Pos.Bottom(_appNameLabel)
    };

    private static readonly Label _passphrase1Label = new()
    {
        Text = "Passphrase 1:",
        X = 1,
        Y = Pos.Bottom(_appDescriptionLabel) + 1
    };

    private static readonly TextField _passphrase1Input = new()
    {
        Secret = true,
        X = Pos.Right(_passphrase1Label) + 1,
        Y = Pos.Bottom(_appDescriptionLabel) + 1,
        Width = Dim.Fill() - 1
    };

    private static readonly Label _passphrase2Label  = new()
    {
        Text = "Passphrase 2:",
        X = 1,
        Y = Pos.Bottom(_passphrase1Label) + 1
    };

    private static readonly TextField _passphrase2Input = new()
    {
        Secret = true,
        X = Pos.Right(_passphrase2Label) + 1,
        Y = Pos.Top(_passphrase2Label),
        Width = Dim.Fill() - 1
    };

    private static readonly Label _passwordLengthLabel = new()
    {
        Text = "Password length:",
        X = 1,
        Y = Pos.Bottom(_passphrase2Label) + 1
    };

    private static readonly TextField _passwordLengthInput = new()
    {
        Text = "32",
        X = Pos.Right(_passwordLengthLabel) + 1,
        Y = Pos.Top(_passwordLengthLabel),
        Width = Dim.Fill() - 1
    };

    private static readonly Button _generateButton = new()
    {
        Text = "Generate",
        X = Pos.Center(),
        Y = Pos.Bottom(_passwordLengthLabel) + 1,
        // IsDefault = true
    };

    private static readonly Label _generateLabel = new()
    {
        Text = "Generating...",
        Visible = false,
        X = Pos.Center(),
        Y = Pos.Bottom(_passwordLengthLabel) + 1,
    };

    private static readonly Button _clearButton = new()
    {
        Text = "Clear fields",
        Visible = false,
        X = Pos.Center(),
        Y = Pos.Bottom(_generateButton)
    };

    private static readonly Label _autoClearLabel = new()
    {
        Text = "Autoclear in 60 s",
        Visible = false,
        X = Pos.Center(),
        Y = Pos.Bottom(_clearButton)
    };

    private static readonly Label _resultLabel = new()
    {
        Text = "Result:",
        Visible = false,
        X = Pos.Center(),
        Y = Pos.Bottom(_autoClearLabel) + 1
    };

    private static readonly TextField _resultField = new()
    {
        // ReadOnly = true,
        Secret = true,
        Visible = false,
        TextAlignment = Alignment.End,
        X = Pos.Center(),
        Y = Pos.Bottom(_resultLabel),
        Width = Dim.Fill()
    };

    private static readonly CheckBox _showPasswordCheckbox = new()
    {
        Text = "Show password",
        Value = CheckState.UnChecked,
        Visible = false,
        X = Pos.Center(),
        Y = Pos.Bottom(_resultField)
    };

    private static readonly Label _entropyLabel = new()
    {
        Text = "Entropy: 0.00 bits",
        Visible = false,
        X = 1,
        Y = Pos.Bottom(_showPasswordCheckbox)
    };

    private static readonly Label _elapsedTimeLabel = new()
    {
        Text = "Elapsed time: 0.000 s",
        Visible = false,
        X = 1,
        Y = Pos.Bottom(_entropyLabel)
    };

    private static readonly Label _passwordCopiedLabel = new()
    {
        Text = "Password copied to clipboard",
        Visible = false,
        X = Pos.Center(),
        Y = Pos.Bottom(_elapsedTimeLabel)
    };
}