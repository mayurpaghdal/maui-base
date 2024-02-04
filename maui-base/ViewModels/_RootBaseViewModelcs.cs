using System.Text.RegularExpressions;
using System.Windows.Input;

namespace MauiBase.ViewModels;

public partial class RootBaseViewModel : ObservableObject, IQueryAttributable
{
    #region Data Members
    public delegate void SearchHandler(string searchText);
    protected internal INavigationParameters _parameters;
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

    public BasePage Page { get; internal set; }

    #endregion

    #region Commands
    public IAsyncRelayCommand GoBackCommand { get; set; }
    #endregion

    #region Events
    public event EventHandler<RootNavigationRequestedEventArgs> RootNavigationRequested;
    #endregion

    #region Services
    internal readonly INavigationService _navigation;
    internal readonly IEventAggregator _eventAggregator;
    #endregion

    #region Ctor
    public RootBaseViewModel(INavigationService navigation = null!,
                         IEventAggregator eventAggregator = null!)
    {
        _navigation = navigation;
        _eventAggregator = eventAggregator;

        InitCommands();
    } 
    #endregion

    #region Public Methods
    public virtual void InitCommands()
    {
        GoBackCommand = new AsyncRelayCommand(GoBack);
    }

    public virtual void SetResources()
    {
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query is not null)
            _parameters = new NavigationParameters(query);
    }
    #endregion

    #region Protected Methods
    protected internal bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email,
                             @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                             RegexOptions.IgnoreCase);
    }
    #endregion

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

    #region Private Methods
    private async Task GoBack()
    {
        var isAnimated = true;

        PresentationMode mode = PresentationMode.Animated;
        try
        {
            mode = Shell.GetPresentationMode(Page);
        }
        catch { }

        //var currentVmName = this.GetType().Name;
        if (mode == PresentationMode.ModalNotAnimated)
        {
            isAnimated = false;
            await Page.PopOutAsync();
        }

        await _navigation.GoBackAsync(null!, isAnimated);
    }
    #endregion
}
