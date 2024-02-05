
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
        GoBackCommand = new AsyncRelayCommand(GoBackAsync);
    }
    #endregion

    #region Command Executables
    
    #endregion

    #region Overridden Methods
    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);

        //_eventAggregator.GetEvent<NewsViewChangedEvent>()?.Publish();

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
