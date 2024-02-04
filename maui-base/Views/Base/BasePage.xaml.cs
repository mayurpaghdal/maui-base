
namespace MauiBase.Views;

public partial class BasePage : ContentPage
{
    public IList<IView> PageContent => PageContentGrid.Children;
    public IList<IView> PageIcons => PageIconsGrid.Children;

    public ContentPopDirection PopInContentDirection { get; set; } = ContentPopDirection.BottomToTop;

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

    public static readonly BindableProperty ModeProperty = BindableProperty.Create(
        nameof(Mode),
        typeof(PageMode),
        typeof(BasePage),
        PageMode.Navigate,
        propertyChanged: OnPageModePropertyChanged);

    public PageMode Mode
    {
        get => (PageMode)GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
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

    #region Internal Methods
    internal void PopIn(Easing easing)
    {
        // Measure the actual content size
        var contentSize = this.Content.Measure(Window.Width, Window.Height, MeasureFlags.IncludeMargins);
        var contentHeight = contentSize.Request.Height;
        var contentWidth = contentSize.Request.Width;

        // Start by translating the content below / off screen
        switch (PopInContentDirection)
        {
            case ContentPopDirection.LeftToRight:
                this.Content.TranslationX = -contentWidth;
                break;
            case ContentPopDirection.RightToLeft:
                this.Content.TranslationX = contentWidth;
                break;
            case ContentPopDirection.TopToBottom:
                this.Content.TranslationY = -contentHeight;
                break;
            case ContentPopDirection.BottomToTop:
            default:
                this.Content.TranslationY = contentHeight;
                break;
        }

        // Animate the translucent background, fading into view
        this.Animate("Background",
            callback: v => this.Background = new SolidColorBrush(Colors.Black.WithAlpha((float)v)),
            start: 0d,
            end: 0.7d,
            rate: 32,
            length: 200,
            easing: Easing.CubicOut,
            finished: (v, k) =>
                this.Background = new SolidColorBrush(Colors.Black.WithAlpha(0.7f)));

        var animationEndValue = PopInContentDirection == ContentPopDirection.LeftToRight || PopInContentDirection == ContentPopDirection.RightToLeft
                                ? contentWidth : contentHeight;

        // Also animate the content sliding up from below the screen
        this.Animate("Content",
            callback: v =>
            {
                switch (PopInContentDirection)
                {
                    case ContentPopDirection.LeftToRight:
                        this.Content.TranslationX = (int)(contentWidth + v);
                        break;
                    case ContentPopDirection.RightToLeft:
                        this.Content.TranslationX = (int)(contentWidth - v);
                        break;
                    case ContentPopDirection.TopToBottom:
                        this.Content.TranslationY = (int)(contentHeight + v);
                        break;
                    case ContentPopDirection.BottomToTop:
                    default:
                        this.Content.TranslationY = (int)(contentHeight - v);
                        break;
                }
            },
            start: 0,
            end: animationEndValue,
            length: 200,
            easing: easing ?? Easing.CubicInOut,
            finished: (v, k) =>
            {
                switch (PopInContentDirection)
                {
                    case ContentPopDirection.LeftToRight:
                    case ContentPopDirection.RightToLeft:
                        this.Content.TranslationX = 0;
                        break;
                    case ContentPopDirection.TopToBottom:
                    case ContentPopDirection.BottomToTop:
                    default:
                        this.Content.TranslationY = 0;
                        break;
                }
            });
    }

    internal Task PopOutAsync()
    {
        var done = new TaskCompletionSource();

        // Measure the content size so we know how much to translate
        var contentSize = this.Content.Measure(Window.Width, Window.Height, MeasureFlags.IncludeMargins);
        var contentHeight = contentSize.Request.Height;
        var contentWidth = contentSize.Request.Width;

        // Start fading out the background
        this.Animate("Background",
                     callback: v => this.Background = new SolidColorBrush(Colors.Black.WithAlpha((float)v)),
                     start: 0.7d,
                     end: 0d,
                     rate: 32,
                     length: 150,
                     easing: Easing.CubicIn,
                     finished: (v, k) => this.Background = new SolidColorBrush(Colors.Black.WithAlpha(0.0f)));

        var animationStartValue = PopInContentDirection == ContentPopDirection.LeftToRight || PopInContentDirection == ContentPopDirection.RightToLeft
                                ? contentWidth : contentHeight;

        animationStartValue = PopInContentDirection == ContentPopDirection.LeftToRight | PopInContentDirection == ContentPopDirection.TopToBottom
                              ? -1 * animationStartValue : animationStartValue;

        // Start sliding the content down below the bottom of the screen
        this.Animate("Content",
            callback: v =>
            {
                switch (PopInContentDirection)
                {
                    case ContentPopDirection.LeftToRight:
                        this.Content.TranslationX = -contentWidth - v;
                        break;
                    case ContentPopDirection.RightToLeft:
                        this.Content.TranslationX = contentWidth - v;
                        break;
                    case ContentPopDirection.TopToBottom:
                        this.Content.TranslationY = -contentHeight - v;
                        break;
                    case ContentPopDirection.BottomToTop:
                    default:
                        this.Content.TranslationY = contentHeight - v;
                        break;
                }
            },
            start: animationStartValue,
            end: 0,
            length: 100,
            easing: Easing.CubicInOut,
            finished: (v, k) =>
            {
                // Start by translating the content below / off screen
                switch (PopInContentDirection)
                {
                    case ContentPopDirection.LeftToRight:
                    case ContentPopDirection.RightToLeft:
                        this.Content.TranslationX = contentWidth;
                        break;
                    case ContentPopDirection.TopToBottom:
                    case ContentPopDirection.BottomToTop:
                    default:
                        this.Content.TranslationY = contentHeight;
                        break;
                }
                // Important: Set our completion source to done!
                done.TrySetResult();
            });

        // We return the task so we can wait for the animation to finish
        return done.Task;
    }
    #endregion
}