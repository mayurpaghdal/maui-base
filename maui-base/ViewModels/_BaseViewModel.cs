
using Mopups.Interfaces;

namespace MauiBase.ViewModels;

public partial class BaseViewModel : RootBaseViewModel
{
    #region Properties

    [ObservableProperty]
    private string title = string.Empty;

    public bool IsBackNavigated { get; private set; }
    #endregion

    #region Services
    #endregion

    #region Ctor
    public BaseViewModel(INavigationService navigation = null!,
                         IEventAggregator eventAggregator = null!)
        : base(navigation, eventAggregator)
    {
        IsBackNavigated = _navigation.Mode == Common.NavigationMode.Back;
    }
    #endregion

    #region Command Exeutables

    #endregion

    //Called on Page Appearing
    #region Navigation Methods
    public virtual async void OnNavigatedTo(INavigationParameters parameters) =>
    await Task.CompletedTask;

    public virtual async void OnRecurringNavigatedTo(INavigationParameters parameters) =>
        await Task.CompletedTask;

    public virtual async void OnNavigatedFrom(INavigationParameters parameters) =>
        await Task.CompletedTask; 
    #endregion
}
