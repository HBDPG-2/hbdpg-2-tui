/*  Copyright (C) 2025 Piotr Kniaz

    This file is part of HBDPG-2.
    Repository: https://github.com/HBDPG-2/hbdpg-2-cli

    Licensed under the MIT License. See LICENSE file in the project root for details.
*/

string passphrase1;
string passphrase2;
int passwordLength;

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

// Passphrase 1 input
while (true)
{
    Console.Write("\nEnter passphrase 1: ");
    passphrase1 = CLI.ReadPassword();

    if (passphrase1.Length >= 8)
    {
        break;
    }
    else
    {
        Console.WriteLine("\nPassphrase must be at least 8 characters long.");
    }
}

// Passphrase 2 input
while (true)
{
    Console.Write("\nEnter passphrase 2: ");
    passphrase2 = CLI.ReadPassword();

    if (passphrase2.Length >= 8)
    {
        break;
    }
    else
    {
        Console.WriteLine("\nPassphrase must be at least 8 characters long.");
    }
}

// Password length input
while (true)
{
    Console.Write("\nEnter password length: ");

    if (int.TryParse(Console.ReadLine(), out passwordLength) && passwordLength >= 16 && passwordLength <= 64)
    {
        break;
    }
    else if (passwordLength == 0)
    {
        passwordLength = 32;
        Console.WriteLine("Using default password length of 32.");
        break;
    }
    else
    {
        Console.WriteLine("Number must be between 16 and 64.");
    }
}

Result result = Core.Generate(passphrase1, passphrase2, passwordLength);

if (result.Password != string.Empty)
{
    Console.WriteLine($"\nGenerated password: {result.Password}");
    Console.WriteLine($"\nEntropy: {result.Entropy.ToString("F2")} bits");
    Console.WriteLine($"Elapsed time: {result.ElapsedTime.ToString("F3")} s");
    // Console.WriteLine($"Attempt: {result.Attempt}");
}
else
{
    Console.WriteLine("\nFailed to generate secure password. Try another passphrases or password length.");
}

Console.WriteLine($"\n(C) 2025 Piotr Kniaz. MIT license.");
Console.WriteLine("\nPress any key to exit.");
Console.ReadKey(true);
Console.Clear();