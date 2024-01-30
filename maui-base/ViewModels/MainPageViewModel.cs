using System.Diagnostics.Metrics;

namespace MauiBase.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    int counter = 0;
    #region Data Members
    private ObservableCollection<TabMenuModel> _bottomActions = [];
    private ObservableCollection<TabMenuModel> _bottomActionsLevel2 = [];
    private List<KeyValuePair<string, NavigationParameters>> navStack = [];
    private BottomAction _activeAction = BottomAction.HomeView;
    private string _currChildViewTitle;
    private IChildViewBase _currChildViewBase;
    private NavigationParameters _parameters;
    private List<BottomAction> renderedTabs = [];
    private string _currChildViewName;
    private bool _isSkeletonVisible;
    private bool _showMenuPopup;
    #endregion

    #region Properties

    public ObservableCollection<TabMenuModel> BottomActions { get => _bottomActions; set => SetProperty(ref _bottomActions, value); }
    public ObservableCollection<TabMenuModel> BottomActionsLevel2 { get => _bottomActionsLevel2; set => SetProperty(ref _bottomActionsLevel2, value); }
    public bool IsSkeletonVisible { get => _isSkeletonVisible; set => SetProperty(ref _isSkeletonVisible, value); }
    public bool ShowMenuPopup { get => _showMenuPopup; set => SetProperty(ref _showMenuPopup, value); }

    #region ChildView Properties
    public string CurrChildViewName
    {
        get => _currChildViewName;
        set => SetProperty(ref _currChildViewName, value);
    }
    public BottomAction ActiveAction
    {
        get => _activeAction;
        set
        {
            SetProperty(ref _activeAction, value);

            IsSkeletonVisible = !renderedTabs.Any(t => t.Equals(value));

            if (IsSkeletonVisible)
                renderedTabs.Add(value);
        }
    }
    public string CurrChildViewTitle
    {
        get => _currChildViewTitle;
        set => SetProperty(ref _currChildViewTitle, value);
    }
    public IChildViewBase CurrChildViewBase
    {
        get => _currChildViewBase;
        set
        {
            if (CurrChildViewBase != null)
            {
                CurrChildViewBase.RootNavigationRequested -= RootNavigationFromChildViewRequested;
                CurrChildViewBase.ChildViewTitleUpdated -= ChildViewTitleUpdated;
            }

            SetProperty(ref _currChildViewBase, value);

            if (CurrChildViewBase != null)
            {
                CurrChildViewBase.RootNavigationRequested += RootNavigationFromChildViewRequested;
                CurrChildViewBase.ChildViewTitleUpdated += ChildViewTitleUpdated;
                //CurrChildViewBase.ShowSearchUpdated += ShowSearchUpdated;
                //CurrChildViewBase.ShowSyncIconUpdated += ShowSyncIconUpdated;
                //CurrChildViewBase.ShowFilterUpdated += ShowFilterUpdated;
                //CurrChildViewBase.FilterIconUpdated += FilterIconUpdated;
            }
        }
    }

    public bool HasChildViewNavigation => Parameters != null && Parameters.ContainsKey(ParamKey.CHILD_VIEW_NAV_KEY);
    #endregion

    #region Navigation
    protected bool IsBackNavigated { get; set; }

    public NavigationParameters Parameters
    {
        get { return _parameters; }
        set { SetProperty(ref _parameters, value); }
    }
    #endregion

    #endregion

    #region Commands
    public IRelayCommand<TabMenuModel> SwitchViewCommand { get; set; }
    #endregion

    #region Services
    private readonly IEventAggregator _eventAggregator;
    #endregion

    #region Ctor
    public MainPageViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
        InitCommands();

        _eventAggregator.GetEvent<NewsViewChangedEvent>()?.Subscribe(ShowMessage);
    }

    private void ShowMessage()
    {
        //Title = $"Things changed {++counter}";
    }
    #endregion

    #region Command Executables
    [RelayCommand]
    async Task GoToDetailPage()
    {
        try
        {
            var detail = new DetailModel
            {
                ID = 1,
                Name = "Mayur",
                Address = "Koteshwar Road",
                City = "Motera"
            };

            var detailPage = new ItemDetailPage
            {
                Parameters = new NavigationParameters
                {
                    { "Abc", detail }
                }
            };
            await Navigation.PushAsync(detailPage, true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region Overridden Methods
    public override void InitCommands()
    {
        base.InitCommands();
        SwitchViewCommand = new RelayCommand<TabMenuModel>(SwitchView);
    }

    public override async void OnNavigatedTo(NavigationParameters parameters)
    {
        this.Title = "App";

        BottomActions.Add(new TabMenuModel
        {
            Action = BottomAction.HomeView,
            Title = "Home",
            Icon = ImageSource.FromFile(IconKey.TAB_HOME_BLACK),
            IsActive = BottomActions.Count == 0,
            Command = SwitchViewCommand
        });
        BottomActions.Add(new TabMenuModel
        {
            Action = BottomAction.NewsView,
            Title = "News",
            Icon = ImageSource.FromFile(IconKey.TAB_NEWS_BLACK),
            IsActive = BottomActions.Count == 0,
            Command = SwitchViewCommand
        });
        BottomActions.Add(new TabMenuModel
        {
            Action = BottomAction.DirectoryView,
            Title = "Directory",
            Icon = ImageSource.FromFile(IconKey.TAB_DIRECTORY_BLACK),
            IsActive = BottomActions.Count == 0,
        });

        BottomActions.Add(new TabMenuModel
        {
            Action = BottomAction.More,
            Title = "More",
            Icon = ImageSource.FromFile(IconKey.TAB_MORE_BLACK),
            IsActive = BottomActions.Count == 0,
        });

        BottomActionsLevel2.Add(new TabMenuModel
        {
            Action = BottomAction.LeavesView,
            Title = "Presence",
            Icon = ImageSource.FromFile(IconKey.TAB_PRESENCE_BLACK),
        });

        BottomActionsLevel2.Add(new TabMenuModel
        {
            Action = BottomAction.PagesView,
            Title = "Portal",
            Icon = ImageSource.FromFile(IconKey.TAB_INFO_BLACK),
        });

        BottomActionsLevel2.Add(new TabMenuModel
        {
            Action = BottomAction.SettingsView,
            Title = "Settings",
            Icon = ImageSource.FromFile(IconKey.TAB_SETTING_BLACK),
        });

        try
        {
            //CrossFirebaseCrashlytics.Current.Log("NI:S try OnNavigatedTo MPVM" + DateTime.Now);
            App.Instance.ActiveVM = this;
            //_eventAggregator.GetEvent<ChangeTitleEvent>().Subscribe(ChangeTitle);
            //_eventAggregator.GetEvent<FilterIconChangedEvent>().Subscribe(ChangeFilterIcon);

            //App.IsAppInForeground = true;
            await Task.Run(() => { });

            if (parameters != null)
            {
                //CrossFirebaseCrashlytics.Current.Log("NI: if (parameters != null) OnNavigatedTo MPVM" + DateTime.Now);
                //NavigationMode navMode = Navigation.NavigationMode.New;
                //try
                //{
                //    navMode = parameters.GetNavigationMode();
                //}
                //catch { }

                //if (IsBackNavigated
                //    && parameters.TryGetValue(ParamKey.PUSH_PAYLOAD, out NotificationPayloadModel payload))
                //    App.PassedPayload = payload;

                //if (IsBackNavigated
                //    && App.PassedPayload == null)
                //{
                //    _eventAggregator?.GetEvent<BackNavigatedEvent>()?.Publish(parameters);
                //    return;
                //}
                //CrossFirebaseCrashlytics.Current.Log("NI:navMode = " + navMode.ToString() + " OnNavigatedTo MPVM" + DateTime.Now);

                if (App.PassedPayload != null)
                {
                    //CrossFirebaseCrashlytics.Current.Log("NI:if (payload != null) OnNavigatedTo MPVM" + DateTime.Now);
                    if (DeviceInfo.Platform == DevicePlatform.iOS
                        && App.PassedPayload.IsAlertAutoClosed) // iOS: To handle auto page navigation on auto notification pop up close
                    {
                        App.PassedPayload = null!;
                        return;
                    }
                    var result = Enum.TryParse(App.PassedPayload?.TargetPage, out BottomAction parsed);
                    if (ActiveAction != parsed)
                        ActiveAction = parsed;

                    await Task.Run(() => { });

                    NavigateToInternalView(App.PassedPayload?.TargetPage);

                    //if (IsBackNavigated && ActiveAction == parsed)
                    //{
                    //    if (ActiveAction == BottomAction.NewsView)
                    //        _eventAggregator.GetEvent<RefreshLatestNewsEvent>()?.Publish();
                    //    else if (ActiveAction == BottomAction.NotificationsView)
                    //        _eventAggregator.GetEvent<RefreshLatestNotificationEvent>()?.Publish();
                    //}
                    if (DeviceInfo.Platform == DevicePlatform.iOS
                        & App.PassedPayload != null
                        && !App.PassedPayload.IsAppForeground) // iOS: To handle foreground notification alert issue on notification tap when app is in background
                        App.IsNotificationNavigationDone = true;

                    App.PassedPayload = null!;
                }
                else if (HasChildViewNavigation
                         && Parameters.TryGetValue<string>(ParamKey.CHILD_VIEW_NAV_KEY, out string childViewString))
                {
                    await Task.Run(() => { });

                    NavigateToInternalView(childViewString, true, parameters);
                    if (Enum.TryParse(childViewString, out BottomAction parsed))
                        ActiveAction = parsed;
                }
                else
                    NavigateToInternalView(ActiveAction.ToString());
            }
            else
                NavigateToInternalView(ActiveAction.ToString());

            RootNavigationRequested += RootNavigationFromBaseVMRequested;

            //_eventAggregator.GetEvent<NavigateToPageEvent>()?.Subscribe(NavigateToFlyoutPage);
        }
        catch (Exception ex)
        {
            //_log.Crashlytics(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, App.Instance.SessionID, ex.Message, ex);
        }
    }

    public override void OnRecurringNavigatedTo(NavigationParameters parameters)
    {
        base.OnRecurringNavigatedTo(parameters);
    }
    #endregion

    #region Private Methods
    private void NavigateToInternalView(string pageName,
                                            bool ignoreNavigationStackInsert = false,
                                            NavigationParameters parameters = null!)
    {
        try
        {
            string pagesToIgnore = $"^^";
            if (parameters != null)
                Parameters = parameters;

            CurrChildViewName = pageName;

            var currentView = navStack.FirstOrDefault(t => t.Key == CurrChildViewName);
            if (ignoreNavigationStackInsert == false
                && !string.IsNullOrWhiteSpace(currentView.Key))
            {
                navStack.Remove(currentView);
            }

            if (ignoreNavigationStackInsert == false
                && !pagesToIgnore.Contains($"^{CurrChildViewName}^"))
            {
                navStack.Add(new KeyValuePair<string, NavigationParameters>(CurrChildViewName, Parameters));
            }
            App.CurrentBottomAction = (BottomAction)System.Enum.Parse(typeof(BottomAction), CurrChildViewName);
        }
        catch (Exception ex)
        {
            //Debug.WriteLine(ex.Message);
            //Utils.Util.Instance.LogCrashlytics(string.Format("SessionID : {0}, Pagename : {1}, Methodname : {2}, Error :  {3}", App.SessionID, MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
            //_log.Crashlytics(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, App.Instance.SessionID, ex.Message, ex);
        }
    }

    private void ChildViewTitleUpdated(object sender, string e) => CurrChildViewTitle = e; // TODO: Need to send event to main page for title change

    private void RootNavigationFromBaseVMRequested(object sender, RootNavigationRequestedEventArgs e) => RootNavigationFromChildViewRequested(this, e);

    /// <summary>
    /// Navigate to Page from child View
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void RootNavigationFromChildViewRequested(object sender, RootNavigationRequestedEventArgs e)
    {
        // Child view navigation requested.
        try
        {
            //if (e.NavigateWithService)
            //    await Navigation.PushAsync(e.PageName, e.Parameters);
            //else 
            if (e.IgnoreStackInsertation)
            {
                var currentView = navStack.FirstOrDefault(c => c.Key == CurrChildViewName);
                if (!string.IsNullOrEmpty(currentView.Key) && navStack.Count > 1)
                    navStack.Remove(currentView);

                if (Enum.TryParse(e.PageName, out BottomAction action))
                {
                    var tab = BottomActions.FirstOrDefault(a => a.Action == action);

                    if (tab is null)
                        tab = BottomActionsLevel2.First(a => a.Action == action);

                    SwitchView(tab);
                }
            }
            else if (e.IsBackNavigation)
                await HandleBack();
            else
            {
                var hasKey = Parameters == null ? false : Parameters.ContainsKey("IgnoreStackInsertation");
                var ignoreStackInsertation = hasKey || e.IgnoreStackInsertation;

                if (Enum.TryParse(e.PageName, out BottomAction action))
                {
                    var tab = BottomActions.FirstOrDefault(a => a.Action == action);

                    if (tab is null)
                        tab = BottomActionsLevel2.First(a => a.Action == action);

                    if (e.Parameters == null)
                        SwitchView(tab);
                    else
                        NavigateToInternalView(e.PageName, ignoreStackInsertation, e.Parameters);
                }
            }
            //CrossFirebaseCrashlytics.Current.Log("NI:E try RootNavigationFromChildViewRequested MPVM" + DateTime.Now);
        }
        catch (Exception ex)
        {
            //Debug.WriteLine(ex.Message);
            //Utils.Util.Instance.LogCrashlytics(string.Format("SessionID : {0}, Pagename : {1}, Methodname : {2}, Error :  {3}", App.SessionID, MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
            //_log.Crashlytics(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, App.Instance.SessionID, ex.Message, ex);
        }
    }

    private void SwitchView(TabMenuModel tab)
    {
        if (tab is null)
            return;

        if (tab.Action == BottomAction.More)
        {
            ShowMenuPopup = true;
            return;
        }

        //if (!IsConnected
        //    && (tab.ActionStr.Equals(BottomAction.ActivityBoardView.ToString())
        //    || tab.ActionStr.Equals(BottomAction.LeavesView.ToString())))
        //{
        //    string message = _resourceHelper.GetLabelTextByLabelName(LabelKey.NoInternetFullMessage);
        //    _dialog.Alert(message, CommonFields.ApplicationName);
        //    return;
        //}

        BottomActions.ForEach(b => b.IsActive = false);
        BottomActionsLevel2.ForEach(b => b.IsActive = false);

        //if item is coming from BottomActionLevel2 then activate MORE in BottomActions as well.
        if (BottomActionsLevel2.Any(b => b.Action == tab.Action))
            BottomActions.First(m => m.Action == BottomAction.More).IsActive = true;

        //ShowMenuPopup = false;
        tab.IsActive = true;
        ActiveAction = tab.Action;

        NavigateToInternalView(tab.ActionStr);
    }

    private async Task HandleBack()
    {
        try
        {
            var currentView = navStack.FirstOrDefault(t => t.Key == CurrChildViewName);
            if (!string.IsNullOrWhiteSpace(currentView.Key) && navStack.Count > 1)
            {
                navStack.Remove(currentView);
            }

            var lastPage = navStack.LastOrDefault();

            if (string.IsNullOrEmpty(lastPage.Key) ||
                (!string.IsNullOrEmpty(lastPage.Key) && CurrChildViewName.Equals(lastPage.Key)))
            {
                if (App.Instance.ActiveChildVM.GoBackCommand != null)
                    await App.Instance.ActiveChildVM.GoBackCommand.ExecuteAsync(null!);
            }
            else
            {
                if (CurrChildViewName.Equals(nameof(HomeView)) && !navStack.Any())
                { }
                else
                {
                    Parameters = lastPage.Value;
                    NavigateToInternalView(lastPage.Key);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            //Utils.Util.Instance.LogCrashlytics(string.Format("SessionID : {0}, Pagename : {1}, Methodname : {2}, Error :  {3}", App.SessionID, MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
            // _log.Crashlytics(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, App.Instance.SessionID, ex.Message, ex);
        }
    }
    #endregion
}
