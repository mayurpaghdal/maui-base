
namespace MauiBase.Controls;

    public class CustomSearchBar : SearchBar
{
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
					typeof(double), typeof(CustomSearchBar), 6.0);
	// Gets or sets CornerRadius value
	public double CornerRadius
	{
		get => (double)GetValue(CornerRadiusProperty);
		set => SetValue(CornerRadiusProperty, value);
	}
	#endregion

	#region BorderColor
	public static readonly BindableProperty BorderColorProperty =
		 BindableProperty.Create(nameof(BorderColor),
				 typeof(Color), typeof(CustomSearchBar), Color.Transparent);
	// Gets or sets BorderColor value
	public Color BorderColor
	{
		get => (Color)GetValue(BorderColorProperty);
		set => SetValue(BorderColorProperty, value);
	}
	#endregion

	#region CurveBackgroundColor
	public static readonly BindableProperty CurveBackgroundColorProperty =
		 BindableProperty.Create(nameof(CurveBackgroundColor),
				 typeof(Color), typeof(CustomSearchBar), Colors.Transparent);
	// Gets or sets BorderColor value
	public Color CurveBackgroundColor
	{
		get => (Color)GetValue(CurveBackgroundColorProperty);
		set => SetValue(CurveBackgroundColorProperty, value);
	}
	#endregion

	#region IconPadding

	public static readonly BindableProperty IconPaddingProperty =
			BindableProperty.Create(nameof(IconPadding), typeof(Thickness),
					typeof(CustomSearchBar), new Thickness(0));
	// Gets or sets IconPlatePaddingAndroid value
	public Thickness IconPadding
	{
		get => (Thickness)GetValue(IconPaddingProperty);
		set => SetValue(IconPaddingProperty, value);
	}
	#endregion

	#region IsPlainEntry
	public static readonly BindableProperty IsPlainEntryProperty =
			BindableProperty.Create(nameof(IsPlainEntry),
					typeof(bool), typeof(CustomSearchBar), false);

	// Gets or sets IsPlainEntry value
	public bool IsPlainEntry
	{
		get => (bool)GetValue(IsPlainEntryProperty);
		set => SetValue(IsPlainEntryProperty, value);
	}
	#endregion
}
