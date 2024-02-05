using CommunityToolkit.Mvvm.DependencyInjection;
using MauiBase.BackgroundServices;
using MauiBase.Helpers;

namespace MauiBase
{
    public partial class App : Application
    {
        #region Static Properties
        internal static App Instance => (Current as App)!;
        public static BottomAction? CurrentBottomAction { get; set; }
        public static NotificationPayloadModel PassedPayload { get; set; }

        #region ChildView Specific
        public static bool IsNotificationNavigationDone { get; set; } = false;
        #endregion

        #endregion

        #region Properties
        public bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
        public BaseViewModel ActiveVM { get; set; } = null!;
        public ChildBaseViewModel ActiveChildVM { get; set; } = null!;
        public static int IntervalCounter { get; internal set; }

        #endregion

        #region Ctor
        public App()
        {
            InitializeComponent();

            var navigation = ServiceHelper.GetService<INavigationService>();
            MainPage = new AppShell(navigation);
            //MainPage = new NavigationPage(new MainPage());
        }
        #endregion

        protected override void OnStart()
        {
            base.OnStart();

            //Start BG services
            BackgroundTaskService.Add(() => new CounterBackgroundService());
            BackgroundTaskService.Start();
        }
    }
}
