
namespace MauiBase.ViewModels;

public partial class UserInitializePageViewModel : BaseViewModel
{
    #region Properties
    [ObservableProperty]
    public DetailModel detail = new();
    #endregion

    #region services
    private readonly IEventAggregator _eventAggregator;
    #endregion

    #region Ctor
    public UserInitializePageViewModel(IEventAggregator eventAggregator,
                                       INavigationService navigation)
        : base(navigation, eventAggregator)
    {
        GoBackCommand = new AsyncRelayCommand(GoBackAsync);
    }
    #endregion

    #region Command Executables
    
    #endregion

    #region Overridden Methods
    public override async void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);

        await Task.Delay(3000);
        await _navigation.NavigateAsync($"//{nameof(MainPage)}");
    }
    #endregion

    #region Private Methods
    private async Task GoBackAsync()
    {
        var detail = new DetailModel
        {
            ID = 1,
            Name = "Mayur",
            Address = "Koteshwar Road",
            City = "Motera"
        };

        var n = new NavigationParameters
            {
                { "Abcd", detail }
            };

        await _navigation.GoBackAsync(n);
    }
    #endregion
}
