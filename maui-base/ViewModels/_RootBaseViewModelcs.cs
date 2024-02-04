using Mopups.Interfaces;
using System.Text.RegularExpressions;

namespace MauiBase.ViewModels;

public partial class RootBaseViewModel : ObservableObject
{
    public delegate void SearchHandler(string searchText);

    #region Services
    internal IPopupNavigation _popupNavigation;
    internal IEventAggregator _eventAggregator;
    #endregion

    #region Events
    public event EventHandler<RootNavigationRequestedEventArgs> RootNavigationRequested;
    #endregion

    #region Data Members
    private bool _isBusy = false;
    #endregion

    #region Properties

    public INavigation Navigation { get; set; }

    public bool IsBusy
    {
        get { return _isBusy; }
        set
        {
            SetProperty(ref _isBusy, value);
            OnPropertyChanged(nameof(IsControlsEnabled));
        }
    }

    public bool IsConnected => App.Instance.IsConnected;

    public bool IsControlsEnabled => !IsBusy;

    public IDisposable _activeToastDialog = null!;

    public BasePage Page { get;  internal set; }

    #endregion

    #region Commands
    [RelayCommand]
    async Task GoBack()
    {

        var isAnimated = true;

        var currentVmName = this.GetType().Name;
        if (Page is not null
            && Page.Mode == PageMode.ModalPopup)
        {
            isAnimated = false;
            await Page.PopOutAsync();
            //await Task.Delay(100);
        }

        await Navigation.GoBackAsync(currentVmName, isAnimated);

        //if (Navigation.ModalStack.Count > 0
        //    && Navigation.ModalStack.Any(n => n.BindingContext is not null && n.BindingContext.GetType().Name.Contains(currentVmName)))
        //{
        //    await Navigation.PopModalAsync(isAnimated);
        //}
        //else if (Navigation.NavigationStack.Count > 0
        //         && Navigation.NavigationStack.Any(n => n.BindingContext is not null && n.BindingContext.GetType().Name.Contains(currentVmName)))
        //    await Navigation.PopAsync(isAnimated);
    }

    #endregion

    public RootBaseViewModel(IEventAggregator eventAggregator = null!,
                             IPopupNavigation popupNavigation = null!)
    {
        _eventAggregator = eventAggregator;
        _popupNavigation = popupNavigation;
    }

    #region Methods
    public virtual void InitCommands()
    {
    }

    public virtual void SetResources()
    {
    }

    protected internal bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email,
                             @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                             RegexOptions.IgnoreCase);
    }

    //protected void ShowForegroundToast(IUserDialogs _dialog,
    //                                 string message,
    //                                 int duration = 7,
    //                                 ToastPosition position = ToastPosition.Bottom)
    //{
    //    if (_dialog is null)
    //    {
    //        Console.WriteLine("::::::::::::::::::::::::::::::: DIALOG SERVICE for TOAST is NULL :::::::::::::::::::::::::::::::");
    //        return;
    //    }

    //    if (_activeToastDialog != null)
    //        _activeToastDialog.Dispose();

    //    System.Drawing.Color color = IsConnected ? System.Drawing.Color.FromArgb(200, 74, 150, 7) : System.Drawing.Color.FromArgb(200, 201, 53, 8);

    //    _activeToastDialog = _dialog.Toast(new ToastConfig(message)
    //                                      .SetDuration(TimeSpan.FromSeconds(duration))
    //                                      .SetBackgroundColor(color)
    //                                      .SetPosition(position));
    //}

    //protected void ShowToast(string message,
    //                         int duration = 7,
    //                         ToastPosition position = ToastPosition.Bottom)
    //{
    //    if (!string.IsNullOrWhiteSpace(message))
    //        DependencyService.Get<IToastService>().Show(message);
    //}

    //protected async Task ShowNoInternetDialog(IUserDialogs _dialog,
    //                                          string title = "No Internet Connection")
    //{
    //    if (_dialog is null)
    //    {
    //        Console.WriteLine("::::::::::::::::::::::::::::::: DIALOG SERVICE for NO INTERNET CONNECTION is NULL :::::::::::::::::::::::::::::::");
    //        return;
    //    }

    //    var message = title;

    //    await _dialog.AlertAsync(message, title, "OK");
    //}
}
#endregion
