namespace MauiAppDemo.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
[ChildViewModel(typeof(HomeViewModel), true)]
public partial class HomeView : ChildView
{
    public HomeView()
    {
        InitializeComponent();
    }
}