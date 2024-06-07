namespace blazchat.Client.Services.Interfaces;

public interface ISpinnerService
{
    public event Action OnShow;
    public event Action OnHide;

    public void Show();
    public void Hide();
}