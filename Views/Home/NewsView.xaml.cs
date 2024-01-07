namespace MauiAppDemo.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
[ChildViewModel(typeof(NewsViewModel), true)]
public partial class NewsView : ChildView
{
    public NewsView()
    {
        InitializeComponent();
    }
}