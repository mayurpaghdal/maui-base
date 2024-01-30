namespace MauiBase.Views;

public partial class BasePopupPage : PopupPage
{
    public BasePopupPage()
	{
		InitializeComponent();

        //Hide the Xamarin Forms build in navigation header
        //NavigationPage.SetHasNavigationBar(this, false);

        ////Set Page Mode
        //SetPageMode(PageMode.None);

        ////Set Content Display Mode
        //SetDisplayMode(DisplayMode.NoNavigationBar);
    }
}