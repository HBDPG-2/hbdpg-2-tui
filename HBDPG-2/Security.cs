using TextCopy;
using System.Threading;

static class Security
{

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

    public static void StartTimer()
    {
        _countdownTimer = new Timer(CLI.UpdateCountdown, null, 0, 1000);
    }

    public static void StopTimer()
    {
        _countdownTimer?.Dispose();
    }

    public static int RemainingTime { get => _remainingTime--; }

    private static Timer? _countdownTimer;
    private static int _remainingTime = 60;
}