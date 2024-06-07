using blazchat.Client.Services.Interfaces;

namespace blazchat.Client.Services;

public class SpinnerService : ISpinnerService
{
    public event Action? OnShow;
    public event Action? OnHide;

    public void Show()
    {
        OnShow?.Invoke();
    }

    public void Hide()
    {
        OnHide?.Invoke();
    }
}