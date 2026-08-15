using Telerik.Blazor;
using Telerik.Blazor.Components;

namespace frontend_blazor.Services;

/// <summary>
/// Every success and every failure, said out loud.
///
/// Before this, a refused save just… did nothing: the button clicked, the row did not change, and
/// the reason ("484 senses use this value", "that rule closes a cycle") stayed in an HTTP response
/// nobody saw. The server already writes good messages; this is what puts them on screen.
///
/// Scoped, so each circuit has its own. The host component registers itself on first render.
/// </summary>
public class Toast
{
    private TelerikNotification? _host;

    /// <summary>Called by <c>ToastHost</c> once the Telerik component exists.</summary>
    public void Register(TelerikNotification host) => _host = host;

    public void Success(string message) => Show(message, ThemeConstants.Notification.ThemeColor.Success);

    public void Error(string message) => Show(message, ThemeConstants.Notification.ThemeColor.Error);

    public void Info(string message) => Show(message, ThemeConstants.Notification.ThemeColor.Info);

    /// <summary>
    /// Shows <paramref name="message"/> if it is a real failure, or the success text if not.
    /// Most call sites hold a "null means it worked" result, and this saves them an if.
    /// </summary>
    public void Result(string? error, string successMessage)
    {
        if (string.IsNullOrWhiteSpace(error)) Success(successMessage);
        else Error(error);
    }

    private void Show(string message, string themeColor)
    {
        if (_host is null || string.IsNullOrWhiteSpace(message)) return;

        _host.Show(new NotificationModel
        {
            Text = message,
            ThemeColor = themeColor,
            // Errors stay until dismissed. A message explaining why the thing you just tried was
            // refused is worth more than four seconds, and it is exactly the message people miss.
            Closable = true,
            CloseAfter = themeColor == ThemeConstants.Notification.ThemeColor.Error ? 0 : 4000,
        });
    }
}
