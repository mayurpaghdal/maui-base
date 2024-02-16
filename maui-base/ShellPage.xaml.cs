
namespace MauiBase
{
    //https://stackoverflow.com/questions/76879858/how-can-i-create-a-custom-tabbar-in-a-maui-app
    //https://vladislavantonyuk.github.io/articles/Adding-custom-action-button-to-.NET-MAUI-Shell-TabBar/
    public partial class ShellPage : Shell
    {
        private BaseViewModel _currVm;

        private readonly INavigationService _navigation;
        private readonly IEventAggregator _eventAggregator;
        public ShellPage(INavigationService navigation,
                         IEventAggregator eventAggregator)
        {
            RegisterRoutes();

            InitializeComponent();
            _navigation = navigation;
            _eventAggregator = eventAggregator;

            Loaded += ShellPage_Loaded;
        }

        #region Overridden Events
        protected override async void OnParentSet()
        {
            base.OnParentSet();

            //TODO: Identify where to go? to login or Startup page.
            if (Parent is not null)
            {
                //Login or MainPage
                await _navigation.NavigateAsync("//LoginPage");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            try
            {
                this.Dispatcher.Dispatch(async () =>
                {
                    await Task.Delay(150);
                    Process.GetCurrentProcess().Kill();
                });

                return base.OnBackButtonPressed();
                //var firstView = _vm.BottomActions.First();
                //switch (tabContainer.CurrentItem)
                //{
                //    case BottomAction.HomeView:
                //        if (App.Instance.ActiveChildVM is HomeViewModel homeVm)
                //        {
                //            this.Dispatcher.Dispatch(async () =>
                //            {
                //                await Task.Delay(150);
                //                Process.GetCurrentProcess().Kill();
                //            });

                //            return base.OnBackButtonPressed();
                //        }
                //        return true;
                //    case BottomAction.NewsView:
                //        if (App.Instance.ActiveChildVM is NewsViewModel newsVm)
                //        {
                //            var canGoBack = true;
                //            //var canGoBack = ValidateNewsBackPress(newsVm);

                //            if (canGoBack)
                //                _vm.SwitchViewCommand.Execute(firstView);
                //        }
                //        return true;
                //    //case BottomAction.ActivityBoardView:
                //    //    if (App.Instance.ActiveChildVM is ActivityBoardViewModel activityBoardVm
                //    //       && activityBoardVm.CurrentPageVm is ActivityBoardSubBaseViewModel subBaseVm)
                //    //        ValidateActivityBoardBackPress(subBaseVm, firstView);
                //    //    return true;
                //    //case BottomAction.DirectoryView:
                //    //    if (App.Instance.ActiveChildVM is DirectoryViewModel directoryVm
                //    //       && directoryVm.CurrentPageVm is DirectorySubBaseViewModel dirSubBaseVm)
                //    //        ValidateDirectoryBackPress(dirSubBaseVm, firstView);
                //    //    return true;
                //    case BottomAction.DirectoryView:
                //    case BottomAction.NotificationsView:
                //    case BottomAction.LeavesView:
                //    case BottomAction.SettingsView:
                //        if (App.Instance.ActiveVM is MainPageViewModel mainPageVm)
                //        {
                //            _vm.SwitchViewCommand.Execute(firstView);
                //        }
                //        return true;
                //    default:
                //        return true;
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Util.Instance.LogCrashlytics(string.Format("SessionID : {0}, Pagename : {1}, Methodname : {2}, Error :  {3}", App.SessionID, MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
            }
            return true;
        }
        #endregion

        #region Events
        private void ShellPage_Loaded(object? sender, EventArgs e)
        {
            _eventAggregator.GetEvent<ToggleTabBarEvent>()?.Subscribe(ToggleTabbar);
        }
        #endregion

        #region Private Methods
        private void ToggleTabbar(bool makeVisible)
        {
            SetTabBarIsVisible(this, true);
        }

        private void RegisterRoutes()
        {
            var assm = Assembly.GetAssembly(typeof(MainPage));
            var lists = assm?.GetTypes().ToList();

            if (lists is null || lists.Count == 0)
                return;

            var types = lists.Where(t => typeof(IBasePage).IsAssignableFrom(t)
                                         && t != typeof(BasePage)
                                         && t != typeof(BaseContentPage<>)
                                         && t.IsClass
                                         && !t.IsAbstract).ToList();

            types.ForEach(t => Routing.RegisterRoute(t.Name, t));
        }
        #endregion
    }
}
