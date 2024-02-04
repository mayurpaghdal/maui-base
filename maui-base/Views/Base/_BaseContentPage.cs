using MauiBase.Helpers;
using Mopups.Services;
using System.Runtime.CompilerServices;

namespace MauiBase.Views;

public partial class BaseContentPage<TViewModel> : BasePage where TViewModel : BaseViewModel
{
    #region Data Members
    protected bool _isLoaded = false;

    protected TViewModel _vm { get; set; }
    //internal NavigationParameters Parameters { get; set; }

    protected event EventHandler ViewModelInitialized;
    #endregion

    #region Ctor
    public BaseContentPage()
    {
        BindingContext = _vm = ServiceHelper.GetService<TViewModel>();
        this.Loaded += BaseContentPage_Loaded;
    }

    private void BaseContentPage_Loaded(object? sender, EventArgs e)
    {
        if (Mode == PageMode.ModalPopup)
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

    //public BaseContentPage(NavigationParameters parameters) : base()
    //{
    //    Parameters = parameters;
    //}
    #endregion

    #region Overridden Methods
    protected override void OnAppearing()
    {
        //Initialize only if page is not loaded previously
        if (!_isLoaded)
        {
            base.OnAppearing();

            _vm.Navigation = this.Navigation;
            _vm.Page = this;

            //Raise Event to notify that ViewModel has been Initialized
            ViewModelInitialized?.Invoke(this, new EventArgs());
            
            //Navigate to View Model's OnNavigatedTo method
            _vm.OnNavigatedTo(_vm._parameters);

            _isLoaded = true;
        }
        else
            _vm.OnRecurringNavigatedTo(_vm._parameters);

        _vm._parameters = null!;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _vm.OnNavigatedFrom(_vm._parameters);
    }

    protected override bool OnBackButtonPressed()
    {
        if (Mode == PageMode.ModalPopup)
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
        await _vm.GoBackCommand?.ExecuteAsync(false)!;
    }
    #endregion
}
