
namespace MauiBase.Controls
{
    public class CustomEntry : Entry
    {
        #region BackgroundColor
        public static new readonly BindableProperty BackgroundColorProperty =
        BindableProperty.Create(nameof(BackgroundColor),
            typeof(Color), typeof(CustomEntry), Colors.Transparent);
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
            typeof(Color), typeof(CustomEntry), Colors.Transparent);
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
            typeof(CustomEntry), 1);

        // Gets or sets BorderWidth value  
        public int BorderWidth
        {
            get => (int)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }
        #endregion


        #region CornerRadius
        public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius),
            typeof(double), typeof(CustomEntry), 6.0);


        // Gets or sets CornerRadius value  
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        #endregion

        #region IsCurvedCornersEnabled
        public static readonly BindableProperty IsCurvedCornersEnabledProperty =
        BindableProperty.Create(nameof(IsCurvedCornersEnabled),
            typeof(bool), typeof(CustomEntry), true);
        // Gets or sets IsCurvedCornersEnabled value  
        public bool IsCurvedCornersEnabled
        {
            get => (bool)GetValue(IsCurvedCornersEnabledProperty);
            set => SetValue(IsCurvedCornersEnabledProperty, value);
        } 
        #endregion
    }
}
