namespace MauiAppDemo.ViewModels;

public partial class ItemDetailPageViewModel : BaseViewModel
{
    #region Properties
    [ObservableProperty]
    public DetailModel detail = new();
    #endregion

    #region Ctor
    public ItemDetailPageViewModel()
    {
        
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
