
namespace MauiBase.Controls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class PopupView : ContentView
{
    #region Ctor
    public PopupView()
    {
        InitializeComponent();
    }
    #endregion

    #region Bindable Properties
    
    #region ContentBackgroundColor
    public static readonly BindableProperty ContentBackgroundColorProperty =
        BindableProperty.Create(propertyName: nameof(ContentBackgroundColor),
                                returnType: typeof(Color),
                                declaringType: typeof(PopupView),
                                defaultValue: Colors.Transparent);

    public Color ContentBackgroundColor
    {
        get { return (Color)GetValue(ContentBackgroundColorProperty); }
        set { SetValue(ContentBackgroundColorProperty, value); }
    } 
    #endregion

    #region ContentMargin
    public static readonly BindableProperty ContentMarginProperty =
    BindableProperty.Create(propertyName: nameof(ContentMargin),
                            returnType: typeof(Thickness),
                            declaringType: typeof(PopupView),
                            defaultValue: new Thickness(0));

    public Thickness ContentMargin
    {
        get { return (Thickness)GetValue(ContentMarginProperty); }
        set { SetValue(ContentMarginProperty, value); }
    }
    #endregion

    #region ContentCornerRadius
    public static readonly BindableProperty ContentCornerRadiusProperty =
    BindableProperty.Create(propertyName: nameof(ContentCornerRadius),
                            returnType: typeof(CornerRadius),
                            declaringType: typeof(PopupView),
                            defaultValue: new CornerRadius(12, 12, 0, 0));

    public CornerRadius ContentCornerRadius
    {
        get { return (CornerRadius)GetValue(ContentCornerRadiusProperty); }
        set { SetValue(ContentCornerRadiusProperty, value); }
    }
    #endregion

    #region ContentVerticalOptions
    public static readonly BindableProperty ContentVerticalOptionsProperty =
    BindableProperty.Create(propertyName: nameof(ContentVerticalOptions),
                            returnType: typeof(LayoutOptions),
                            declaringType: typeof(PopupView),
                            defaultValue: LayoutOptions.End);

    public LayoutOptions ContentVerticalOptions
    {
        get { return (LayoutOptions)GetValue(ContentVerticalOptionsProperty); }
        set { SetValue(ContentVerticalOptionsProperty, value); }
    }
    #endregion

    #region ShowPopup
    public static readonly BindableProperty ShowPopupProperty =
    BindableProperty.Create(propertyName: nameof(ShowPopup),
                            returnType: typeof(bool),
                            declaringType: typeof(PopupView),
                            defaultValue: false,
                            propertyChanged: OnVisibilityChanged);

    public bool ShowPopup
    {
        get { return (bool)GetValue(ShowPopupProperty); }
        set { SetValue(ShowPopupProperty, value); }
    } 
    #endregion
    
    #endregion

    #region Private Methods
    private static async void OnVisibilityChanged(BindableObject bindable, object oldValue, object newValue)
    {
        try
        {
            var control = (PopupView)bindable;
            var isVisible = (bool)newValue;

            if (control == null
                || isVisible == control.IsVisible)
                return;

            if (isVisible)
            {
                await control.FadeTo(0, 0);
                control.IsVisible = isVisible;
                await control.FadeTo(1, 150);
            }
            else
            {
                await control.FadeTo(1, 0);
                await control.FadeTo(0, 150);
                control.IsVisible = isVisible;
            }

        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    private void PopupBackground_Tapped(object sender, EventArgs e)
    {
        ShowPopup = false;
    }
}