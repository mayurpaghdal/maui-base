
namespace MauiBase.ViewModels;

public partial class ItemDetailPageViewModel : BaseViewModel
{
    #region Properties
    [ObservableProperty]
    public DetailModel detail = new();
    #endregion

    #region services
    private readonly IEventAggregator _eventAggregator;
    #endregion

    #region Ctor
    public ItemDetailPageViewModel(IEventAggregator eventAggregator,
                                   INavigationService navigation)
        : base(navigation, eventAggregator)
    {
        GoBackCommand = new AsyncRelayCommand(GoBack);
    }
    #endregion

    #region Command Executables
    [RelayCommand]
    async Task GoToDetailPage()
    {
        //await Navigation.PushAsync(new VideoDetailsPage(videoID));
    }
    #endregion

    #region Overridden Methods
    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);

        //_eventAggregator.GetEvent<NewsViewChangedEvent>()?.Publish();

        this.Title = "Hello World";

        if (parameters != null)
        {
            if (parameters["Abcd"] is DetailModel _detail)
                Detail = _detail;
        }

        //await Search();
    }
    #endregion

    #region Private Methods
    private async Task GoBack()
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
                { "Abc", detail }
            };

        await _navigation.GoBackAsync(n);
    }
    #endregion
}
