using MauiAppDemo.Helpers;

namespace MauiAppDemo.Views;

public partial class BaseContentPage<TViewModel> : BasePage where TViewModel : BaseViewModel
{
    #region Data Members
    protected bool _isLoaded = false;

    protected TViewModel _vm { get; set; }
    internal NavigationParameters Parameters { get; set; }

    protected event EventHandler ViewModelInitialized; 
    #endregion

    #region Ctor
    public BaseContentPage()
    {
        BindingContext = _vm = ServiceHelper.GetService<TViewModel>();
    }

    public BaseContentPage(NavigationParameters parameters) : base()
    {
        Parameters = parameters;
    }
    #endregion

    #region Overridden Methods
    protected override void OnAppearing()
    {
        //Initialize only if page is not loaded previously
        if (!_isLoaded)
        {
            base.OnAppearing();

            _vm.Navigation = this.Navigation;

            //Raise Event to notify that ViewModel has been Initialized
            ViewModelInitialized?.Invoke(this, new EventArgs());

            //Navigate to View Model's OnNavigatedTo method
            _vm.OnNavigatedTo(Parameters);

            _isLoaded = true;
        }

        _vm.OnRecurringNavigatedTo(Parameters);
    } 
    #endregion
}
