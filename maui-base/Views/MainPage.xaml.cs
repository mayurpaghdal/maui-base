namespace MauiBase.Views;

public partial class MainPage : BaseContentPage<MainPageViewModel>
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override bool OnBackButtonPressed()
    {
        try
        {
            var activeVMName = App.Instance.ActiveVM?.GetType()?.Name;

            if (!string.IsNullOrWhiteSpace(activeVMName)
                && activeVMName.Contains("filterPage", StringComparison.OrdinalIgnoreCase))
            {
                App.Instance.ActiveVM = this._vm;
                return true;
            }

            var firstView = _vm.BottomActions.First();
            switch (_vm.ActiveAction)
            {
                case BottomAction.HomeView:
                    if (App.Instance.ActiveChildVM is HomeViewModel homeVm)
                    {
                        this.Dispatcher.Dispatch(async () =>
                        {
                            await Task.Delay(150);
                            Process.GetCurrentProcess().Kill();
                        });

                        return base.OnBackButtonPressed();
                    }
                    return true;
                case BottomAction.NewsView:
                    if (App.Instance.ActiveChildVM is NewsViewModel newsVm)
                    {
                        var canGoBack = true;
                        //var canGoBack = ValidateNewsBackPress(newsVm);

                        if (canGoBack)
                            _vm.SwitchViewCommand.Execute(firstView);
                    }
                    return true;
                //case BottomAction.ActivityBoardView:
                //    if (App.Instance.ActiveChildVM is ActivityBoardViewModel activityBoardVm
                //       && activityBoardVm.CurrentPageVm is ActivityBoardSubBaseViewModel subBaseVm)
                //        ValidateActivityBoardBackPress(subBaseVm, firstView);
                //    return true;
                //case BottomAction.DirectoryView:
                //    if (App.Instance.ActiveChildVM is DirectoryViewModel directoryVm
                //       && directoryVm.CurrentPageVm is DirectorySubBaseViewModel dirSubBaseVm)
                //        ValidateDirectoryBackPress(dirSubBaseVm, firstView);
                //    return true;
                case BottomAction.DirectoryView:
                case BottomAction.NotificationsView:
                case BottomAction.LeavesView:
                case BottomAction.SettingsView:
                    if (App.Instance.ActiveVM is MainPageViewModel mainPageVm)
                    {
                        _vm.SwitchViewCommand.Execute(firstView);
                    }
                    return true;
                default:
                    return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            //Util.Instance.LogCrashlytics(string.Format("SessionID : {0}, Pagename : {1}, Methodname : {2}, Error :  {3}", App.SessionID, MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
        }
        return true;
    }
}
