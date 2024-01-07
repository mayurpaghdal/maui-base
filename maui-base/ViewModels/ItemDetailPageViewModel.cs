namespace MauiAppDemo.ViewModels;

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
    public ItemDetailPageViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
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
    public override void OnNavigatedTo(NavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);

        _eventAggregator.GetEvent<NewsViewChangedEvent>()?.Publish();

        this.Title = "Hello World";

        if (parameters != null)
        {
            if (parameters["Abc"] is DetailModel _detail)
                Detail = _detail;
        }

        //await Search();
    }
    #endregion

    #region Private Methods

    #endregion
}
