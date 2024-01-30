namespace MauiBase.Controls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class TitleBar : ContentView
{
    public TitleBar()
    {
        InitializeComponent();
    }

    #region RightPanelContent
    public View RightPanelContent
    {
        get { return ContentArea.Content; }
        set { ContentArea.Content = value; }
    }
    #endregion

    #region ShowBackButton
    public static readonly BindableProperty ShowBackButtonProperty =
        BindableProperty.Create(
            propertyName: nameof(ShowBackButton),
            returnType: typeof(bool),
            declaringType: typeof(TitleBar),
            defaultValue: true);

    public bool ShowBackButton
    {
        get { return (bool)GetValue(ShowBackButtonProperty); }
        set { SetValue(ShowBackButtonProperty, value); }
    }
    #endregion
}