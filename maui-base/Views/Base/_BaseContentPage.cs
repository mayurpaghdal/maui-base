namespace MauiBase.Views;

public partial class BaseContentPage<TViewModel> : BasePage where TViewModel : BaseViewModel
{
    #region Data Members
    protected bool _isLoaded = false;

    protected TViewModel _vm { get; set; }

    protected event EventHandler ViewModelInitialized;
    #endregion

    #region Ctor
    public BaseContentPage()
    {
        BindingContext = _vm = ServiceHelper.GetService<TViewModel>();

        if (_vm is not null)
        {
            try
            {
                _vm.Page = this;
                _vm.PagePresentationMode = Shell.GetPresentationMode(this);
            }
            catch { }
        }
        this.Loaded += BaseContentPage_Loaded;
    }
    #endregion

    #region Events
    private void BaseContentPage_Loaded(object? sender, EventArgs e)
    {
        if (_vm.PagePresentationMode == PresentationMode.ModalNotAnimated)
        {
            BackgroundColor = Colors.Black.WithAlpha(0f);
            Background = new SolidColorBrush(Colors.Black.WithAlpha(0f));

            var navPage = (this as NavigationPage)!;

            if (navPage is not null)
            {
                navPage.BackgroundColor = Colors.Black.WithAlpha(0f);
                navPage.Background = new SolidColorBrush(Colors.Black.WithAlpha(0f));
            }
            PopIn(Easing.CubicInOut);
        }
    }
    #endregion

    #region Overridden Methods
    protected override void OnAppearing()
    {
        //Initialize only if page is not loaded previously
        if (!_isLoaded)
        {
            base.OnAppearing();

            //Raise Event to notify that ViewModel has been Initialized
            ViewModelInitialized?.Invoke(this, new EventArgs());

            //Navigate to View Model's OnNavigatedTo method
            _vm.OnNavigatedTo(_vm.NavParameters);

            _isLoaded = true;
        }
        else
            _vm.OnRecurringNavigatedTo(_vm.NavParameters);

        _vm.NavParameters = null!;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _vm.OnNavigatedFrom(_vm.NavParameters);
    }

    protected override bool OnBackButtonPressed()
    {
        if (_vm.PagePresentationMode == PresentationMode.ModalNotAnimated)
        {
            GoBackAsync();
            return true;
        }
        return base.OnBackButtonPressed();
    }
    #endregion

    #region Protected Methods
    protected async void GoBackAsync()
    {
        if (_vm.GoBackCommand is not null)
            await _vm.GoBackCommand.ExecuteAsync(false)!;
    }
    #endregion
}
