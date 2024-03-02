namespace MauiBase.Controls;

public class CustomDatePicker : DatePicker
{
    #region BackgroundColor
    public static new readonly BindableProperty BackgroundColorProperty =
    BindableProperty.Create(nameof(BackgroundColor),
        typeof(Color), typeof(CustomDatePicker), Colors.Transparent);
    // Gets or sets BorderColor value  
    public new Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }
    #endregion

    #region BorderColor
    public static readonly BindableProperty BorderColorProperty =
    BindableProperty.Create(nameof(BorderColor),
        typeof(Color), typeof(CustomDatePicker), Colors.Transparent);
    // Gets or sets BorderColor value  
    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }
    #endregion


    #region BorderWidth
    public static readonly BindableProperty BorderWidthProperty =
    BindableProperty.Create(nameof(BorderWidth), typeof(int),
        typeof(CustomDatePicker), 1);

    // Gets or sets BorderWidth value  
    public int BorderWidth
    {
        get => (int)GetValue(BorderWidthProperty);
        set => SetValue(BorderWidthProperty, value);
    }
    #endregion

    #region Padding
    public static readonly BindableProperty PaddingProperty =
    BindableProperty.Create(nameof(Padding),
        typeof(Thickness), typeof(CustomDatePicker), new Thickness(8, 2));

    // Gets or sets Padding value  
    public Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }
    #endregion

    #region CornerRadius
    public static readonly BindableProperty CornerRadiusProperty =
    BindableProperty.Create(nameof(CornerRadius),
        typeof(double), typeof(CustomDatePicker), 6.0);


    // Gets or sets CornerRadius value  
    public double CornerRadius
    {
        get => (double)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }
    #endregion
}
