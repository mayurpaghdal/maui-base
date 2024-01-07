
namespace MauiAppDemo.ViewModels;

public partial class BaseViewModel : RootBaseViewModel
{
    #region Properties

    [ObservableProperty]
    private string title = string.Empty;
    #endregion

    #region Services

    #endregion

    #region Ctor
    public BaseViewModel()
    {

    }
    #endregion

    #region Command Exeutables
    
    #endregion

    //Called on Page Appearing
    public virtual async void OnNavigatedTo(NavigationParameters parameters) =>
        await Task.CompletedTask;

    public virtual async void OnRecurringNavigatedTo(NavigationParameters parameters) =>
        await Task.CompletedTask;

    public virtual async void OnNavigatedFrom(NavigationParameters parameters) =>
        await Task.CompletedTask;
}
