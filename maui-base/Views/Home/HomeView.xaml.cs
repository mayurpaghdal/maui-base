namespace MauiBase.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
[ChildViewModel(typeof(HomeViewModel), true)]
public partial class HomeView : ChildView
{
    public HomeView()
    {
        InitializeComponent();
        Loaded += HomeView_Loaded;
    }

    private void HomeView_Loaded(object? sender, EventArgs e)
    {
        Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            lblCounter.Text = App.IntervalCounter.ToString("F2");

            if (App.IntervalCounter >= 60)
                return false;

            return true;
        });
    }
}