/*  Copyright (C) 2025 Piotr Kniaz

    This file is part of HBDPG-2.
    Repository: https://github.com/HBDPG-2/hbdpg-2-cli

    Licensed under the MIT License. See LICENSE file in the project root for details.
*/

using TextCopy;

string passphrase1;
string passphrase2;
int passwordLength;

bool isClipboardSupported = false;

Console.Clear();
Console.WriteLine(".....................................................................");
Console.WriteLine("..##.....##.########..########..########...######..........#######...");
Console.WriteLine("..##.....##.##.....##.##.....##.##.....##.##....##........##.....##..");
Console.WriteLine("..##.....##.##.....##.##.....##.##.....##.##.....................##..");
Console.WriteLine("..#########.########..##.....##.########..##..####.######..#######...");
Console.WriteLine("..##.....##.##.....##.##.....##.##........##....##........##.........");
Console.WriteLine("..##.....##.##.....##.##.....##.##........##....##........##.........");
Console.WriteLine("..##.....##.########..########..##.........######.........#########..");
Console.WriteLine(".....................................................................");
Console.WriteLine("                            (CLI edition)                            ");
Console.WriteLine("                             version 0.1                             ");
Console.WriteLine("                  (C) 2025 Piotr Kniaz. MIT license.                 ");
Console.WriteLine("\n");

// Passphrase 1 input
while (true)
{
    Console.Write("Enter passphrase 1: ");
    int currentConsoleLine = Console.CursorTop;

    passphrase1 = CLI.ReadPassword();

    if (passphrase1.Length >= 8)
    {
        CLI.ClearLine(currentConsoleLine + 1);
        break;
    }
    else
    {
        Console.SetCursorPosition(0, currentConsoleLine + 1);
        Console.WriteLine("Passphrase must be at least 8 characters long.");

        CLI.ClearLine(currentConsoleLine);
        Console.SetCursorPosition(0, currentConsoleLine);
    }
}

// Passphrase 2 input
while (true)
{
    Console.Write("Enter passphrase 2: ");
    int currentConsoleLine = Console.CursorTop;

    passphrase2 = CLI.ReadPassword();

    if (passphrase2.Length >= 8)
    {
        CLI.ClearLine(currentConsoleLine + 1);
        break;
    }
    else
    {
        Console.SetCursorPosition(0, currentConsoleLine + 1);
        Console.WriteLine("Passphrase must be at least 8 characters long.");

        CLI.ClearLine(currentConsoleLine);
        Console.SetCursorPosition(0, currentConsoleLine);
    }
}

// Password length input
while (true)
{
    Console.Write("Enter password length: ");
    int currentConsoleLine = Console.CursorTop;

    string? input = Console.ReadLine();

    if (int.TryParse(input, out passwordLength) && passwordLength >= 16 && passwordLength <= 64)
    {
        CLI.ClearLine(currentConsoleLine + 1);
        break;
    }
    else if (passwordLength == 0 && input == string.Empty)
    {
        passwordLength = 32;
        CLI.ClearLine(currentConsoleLine + 1);
        Console.WriteLine("Using default password length of 32.");
        break;
    }
    else
    {
        Console.SetCursorPosition(0, currentConsoleLine + 1);
        Console.WriteLine("Number must be between 16 and 64.");

        CLI.ClearLine(currentConsoleLine);
        Console.SetCursorPosition(0, currentConsoleLine);
    }
}

Result result = Core.Generate(passphrase1, passphrase2, passwordLength);

if (result.Password != null)
{
    Console.Write("\nGenerated password: ");

    try
    {
        ClipboardService.SetText(result.Password);

        for (int i = 0; i < result.Password.Length; i++)
        {
            Console.Write("*");
        }

        Console.WriteLine("\nPassword copied to clipboard.");
        isClipboardSupported = true;
    }
    catch (Exception)
    {
        Console.WriteLine(result.Password);
        Console.WriteLine("Failed to copy password to clipboard.");
    }

    Console.WriteLine($"\nEntropy: {result.Entropy.ToString("F2")} bits");
    Console.WriteLine($"Elapsed time: {result.ElapsedTime.ToString("F3")} s");
    // Console.WriteLine($"Attempt: {result.Attempt}");
}
else
{
    Console.WriteLine("\nFailed to generate secure password. Try another passphrases or password length.");
}

Console.Write("\nPress any key to exit.");

if (isClipboardSupported)
{
    Console.WriteLine(" Clipboard will be cleared automatically!");
}
else
{
    Console.WriteLine();
}

Console.ReadKey(true);

if (isClipboardSupported && ClipboardService.GetText() == result.Password)
{
    ClipboardService.SetText("");
}

Console.Clear();