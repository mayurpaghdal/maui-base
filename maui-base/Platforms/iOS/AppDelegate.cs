using Foundation;

namespace MauiBase
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp()
        {
            var app = MauiProgram.CreateMauiApp();
            BackgroundAggregator.Init(this);
            return app;
        }
    }
}
