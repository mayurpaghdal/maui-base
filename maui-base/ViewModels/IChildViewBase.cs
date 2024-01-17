namespace MauiBase.ViewModels;

public interface IChildViewBase
{
    event EventHandler<string> ChildViewTitleUpdated;
    event EventHandler<bool> ShowSearchUpdated;
    event EventHandler<bool> ShowFilterUpdated;
    event EventHandler<string> FilterIconUpdated;
    event EventHandler<bool> ShowSyncIconUpdated;
    event Action SearchClicked;
    //Action FilterClicked;

    void NotifyTitleChange(string newTitle);
    bool IsActiveSelection { get; set; }
    Task OnNavigatedTo(NavigationParameters parameters);
    void OnNavigatedFrom(NavigationParameters parameters);
    Task DestroyAsync();

    event EventHandler<PropertyChangedEventArgs> InternalPropertyChanged;
    event EventHandler<RootNavigationRequestedEventArgs> RootNavigationRequested;

    void InvokeFilterClick();
    void InvokeSearchClick();

    void InvokeSyncIconClick();

    string Title { get; set; }
}
