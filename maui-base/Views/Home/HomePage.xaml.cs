namespace MauiBase.Views;

public partial class HomePage : BaseContentPage<HomePageViewModel>
{
    public HomePage()
    {
        InitializeComponent();
        Shell.SetTabBarIsVisible(this, true);
    }
}