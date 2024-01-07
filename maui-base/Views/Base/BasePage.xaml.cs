
namespace MauiAppDemo.Views;

public partial class BasePage : ContentPage
{
    public IList<IView> PageContent => PageContentGrid.Children;
    public IList<IView> PageIcons => PageIconsGrid.Children;

    protected bool IsBackButtonEnabled
    {
        set => NavigateBackButton.IsEnabled = value;
    }

    #region Bindable properties
    public static readonly BindableProperty PageTitleProperty = BindableProperty.Create(
        nameof(PageTitle),
        typeof(string),
        typeof(BasePage),
        string.Empty,
        defaultBindingMode:
        BindingMode.OneWay,
        propertyChanged: OnPageTitleChanged);

    public string PageTitle
    {
        get => (string)GetValue(PageTitleProperty);
        set => SetValue(PageTitleProperty, value);
    }

    private static void OnPageTitleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is BasePage basePage)
        {
            basePage.TitleLabel.Text = (string)newValue;
            basePage.TitleLabel.IsVisible = true;
        }
    }

    public static readonly BindableProperty PageModeProperty = BindableProperty.Create(
        nameof(PageMode),
        typeof(PageMode),
        typeof(BasePage),
        PageMode.None,
        propertyChanged: OnPageModePropertyChanged);

    public PageMode PageMode
    {
        get => (PageMode)GetValue(PageModeProperty);
        set => SetValue(PageModeProperty, value);
    }

    private static void OnPageModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is BasePage basePage)
            basePage.SetPageMode((PageMode)newValue);
    }

    private void SetPageMode(PageMode pageMode)
    {
        switch (pageMode)
        {
            case PageMode.Menu:
                HamburgerButton.IsVisible = true;
                NavigateBackButton.IsVisible = false;
                CloseButton.IsVisible = false;
                break;
            case PageMode.Navigate:
                HamburgerButton.IsVisible = false;
                NavigateBackButton.IsVisible = true;
                CloseButton.IsVisible = false;
                break;
            case PageMode.Modal:
                HamburgerButton.IsVisible = false;
                NavigateBackButton.IsVisible = false;
                CloseButton.IsVisible = true;
                break;
            default:
                HamburgerButton.IsVisible = false;
                NavigateBackButton.IsVisible = false;
                CloseButton.IsVisible = false;
                break;
        }
    }


    public static readonly BindableProperty DisplayModeProperty = BindableProperty.Create(
        nameof(DisplayMode),
        typeof(DisplayMode),
        typeof(BasePage),
        DisplayMode.NoNavigationBar,
        propertyChanged: OnDisplayModePropertyChanged);

    public DisplayMode DisplayMode
    {
        get => (DisplayMode)GetValue(DisplayModeProperty);
        set => SetValue(DisplayModeProperty, value);
    }

    private static void OnDisplayModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is BasePage basePage)
            basePage.SetDisplayMode((DisplayMode)newValue);
    }

    private void SetDisplayMode(DisplayMode DisplayMode)
    {
        switch (DisplayMode)
        {
            case DisplayMode.NavigationBar:
                Grid.SetRow(PageContentGrid, 2);
                Grid.SetRowSpan(PageContentGrid, 1);
                break;
            case DisplayMode.NoNavigationBar:
                Grid.SetRow(PageContentGrid, 0);
                Grid.SetRowSpan(PageContentGrid, 3);
                break;
            default:
                //Do Nothing
                break;
        }
    }
    #endregion


    public BasePage()
	{
		InitializeComponent();

        //Hide the Xamarin Forms build in navigation header
        NavigationPage.SetHasNavigationBar(this, false);

        //Set Page Mode
        SetPageMode(PageMode.None);

        //Set Content Display Mode
        SetDisplayMode(DisplayMode.NoNavigationBar);
    }
}