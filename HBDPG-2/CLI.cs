using TextCopy;

static class CLI
{
    public static string ReadPassword()
    {
        string password = string.Empty;
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(intercept: true);
            
            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[0..^1];
                Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        } while (key.Key != ConsoleKey.Enter);

        return password;
    }

    public static void ClearLine(int line)
    {
        Console.SetCursorPosition(0, line);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, line);
    }

    public static void WriteCountdown(int seconds)
    {
        _countdownLine = Console.CursorTop;
        Console.WriteLine($"Clipboard will be cleared in {seconds} second{(seconds > 1 ? "s" : "")} or immediately after exiting!");
    }

    public static void UpdateCountdown(object? state)
    {
        int seconds = Security.RemainingTime;
        int currentConsoleLine = Console.CursorTop;

        if (seconds > 0)
        {
            ClearLine(_countdownLine);
            WriteCountdown(seconds);
            Console.SetCursorPosition(0, currentConsoleLine);
        }
        else
        {
            Environment.Exit(0);
        }
    }

    private static int _countdownLine;
}