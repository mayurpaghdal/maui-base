namespace MauiBase.Models;

public class TabMenuModel : BaseModel
{
    private bool _isActive;

    public string Title { get; set; }
    public BottomAction Action { get; set; }
    public string ActionStr => Action.ToString();
    public ImageSource Icon { get; set; }
    public IRelayCommand Command { get; set; }

    public bool IsActive
    {
        get => _isActive;
        set => SetProperty(ref _isActive, value);
    }
}
