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

    public static void CopyToClipboard(Result? result)
    {
        if (result != null && result.Password != null)
        {
            try
            {
                ClipboardService.SetText(result.Password);
            }
            catch
            {
                throw new Exception("Clipboard can't be cleared.");
            }
        }
    }

    public static void ClearClipboard(Result? result)
    {
        if (result != null && result.Password != null && ClipboardService.GetText() == result.Password)
        {
            try
            {
                ClipboardService.SetText(string.Empty);
            }
            catch
            {}
        }
    }
}