#if ANDROID
using Android.Graphics;
using Android.Widget;
using CommunityToolkit.Maui.Behaviors;
using AG = Android.Graphics;
using AV = Android.Views;
using AButton = Android.Widget.Button;
#endif

#if IOS
using UIKit;
#endif

using Microsoft.Maui.Controls.Platform;
using MG = Microsoft.Maui.Graphics;
using MC = Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;

namespace MauiBase.Effects;

#region Shared
public static class IconTintColorEffect
{
    public static readonly BindableProperty ColorProperty =
    BindableProperty.CreateAttached(
        "Color",
        typeof(MG.Color),
        typeof(TouchEffect),
        default!,
        propertyChanged: PropertyChanged
    );

    public static void SetColor(BindableObject view, MG.Color value)
    {
        view.SetValue(ColorProperty, value);
    }

    public static MG.Color GetColor(BindableObject view)
    {
        return (MG.Color)view.GetValue(ColorProperty);
    }

    static void PropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!(bindable is MC.View view))
            return;

        var eff = view.Effects.FirstOrDefault(e => e is IconTintColorRoutingEffect);
        if (GetColor(bindable) != default!)
        {
            view.InputTransparent = false;

            if (eff != null) return;
            view.Effects.Add(new IconTintColorRoutingEffect());
        }
        else
        {
            if (eff == null || view.BindingContext == null) return;
            view.Effects.Remove(eff);
        }
    }
}
#endregion

#region Routing Effects
public class IconTintColorRoutingEffect : RoutingEffect
{
    public IconTintColorRoutingEffect() : base()
    {
    }
}
#endregion

#if ANDROID
public class IconTintColorEffectPlatform : PlatformEffect
{
    public IconTintColorEffectPlatform() : base()
    {
    }

    protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
    {
        base.OnElementPropertyChanged(args);

        if (args.PropertyName == IconTintColorEffect.ColorProperty.PropertyName)
        {
            ApplyTintColor(Control, Container);
        }
    }

    protected override void OnAttached()
    {
        ApplyTintColor(Control, Container);
    }

    protected override void OnDetached()
    {
        ClearTintColor(Control, Container);
    }

    #region Private Methods
    void ApplyTintColor(AV.View element, AV.View control)
    {
        var color = IconTintColorEffect.GetColor(Element);

        if (color == default!)
            return;

        switch (control)
        {
            case ImageView image:
                SetImageViewTintColor(image, color);
                break;
            case AButton button:
                SetButtonTintColor(button, color);
                break;
            default:
                throw new NotSupportedException($"{nameof(IconTintColorBehavior)} only currently supports Android.Widget.Button and {nameof(ImageView)}.");
        }


        static void SetImageViewTintColor(ImageView image, MG.Color? color)
        {
            if (color is null)
            {
                image.ClearColorFilter();
                color = Colors.Transparent;
            }

            image.SetColorFilter(new PorterDuffColorFilter(color.ToPlatform(), PorterDuff.Mode.SrcIn ?? throw new InvalidOperationException("PorterDuff.Mode.SrcIn should not be null at runtime.")));
        }

        static void SetButtonTintColor(AButton button, MG.Color? color)
        {
            var drawables = button.GetCompoundDrawables().Where(d => d is not null);

            if (color is null)
            {
                foreach (var img in drawables)
                {
                    img.ClearColorFilter();
                }
                color = Colors.Transparent;
            }

            foreach (var img in drawables)
            {
                img.SetTint(color.ToPlatform());
            }
        }
    }

    void ClearTintColor(AV.View element, AV.View control)
    {
        switch (control)
        {
            case ImageView image:
                image.ClearColorFilter();
                break;
            case AButton button:
                foreach (var drawable in button.GetCompoundDrawables())
                {
                    drawable?.ClearColorFilter();
                }
                break;
        }
    }
    #endregion
}
#endif

#if IOS
public class IconTintColorEffectPlatform : PlatformEffect
{
    Color color = Colors.Transparent;
    public UIView View => Control ?? Container;

    #region Overridden Methods
    protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
    {
        base.OnElementPropertyChanged(args);

        if (args.PropertyName == IconTintColorEffect.ColorProperty.PropertyName)
        {
            OnAttached();
        }
    }

    protected override void OnAttached()
    {
        color = IconTintColorEffect.GetColor(Element);
        ApplyTintColor();
    }

    protected override void OnDetached()
    {
        ClearTintColor();
    }
    #endregion

    #region Private Methods
    void ApplyTintColor()
    {
        if (color is null)
        {
            ClearTintColor();
            return;
        }

        switch (View)
        {
            case UIImageView imageView:
                SetUIImageViewTintColor(imageView, color);
                break;
            case UIButton button:
                SetUIButtonTintColor(button, color);
                break;
            default:
                throw new NotSupportedException($"{nameof(IconTintColorEffectPlatform)} only currently supports {nameof(UIButton)} and {nameof(UIImageView)}.");
        }
    }

    void ClearTintColor()
    {
        switch (View)
        {
            case UIImageView imageView:
                if (imageView.Image is not null)
                {
                    imageView.Image = imageView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
                }

                break;
            case UIButton button:
                if (button.ImageView?.Image is not null)
                {
                    var originalImage = button.CurrentImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
                    button.SetImage(originalImage, UIControlState.Normal);
                }

                break;

            default:
                throw new NotSupportedException($"{nameof(IconTintColorEffectPlatform)} only currently supports {nameof(UIButton)} and {nameof(UIImageView)}.");
        }
    }


    static void SetUIButtonTintColor(UIButton button, MG.Color color)
    {
        if (button.ImageView.Image is null)
        {
            return;
        }

        var templatedImage = button.CurrentImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);

        button.SetImage(null, UIControlState.Normal);
        var platformColor = color.ToPlatform();
        button.TintColor = platformColor;
        button.ImageView.TintColor = platformColor;
        button.SetImage(templatedImage, UIControlState.Normal);

    }

    static void SetUIImageViewTintColor(UIImageView imageView, MG.Color color)
    {
        if (imageView.Image is null)
        {
            return;
        }

        imageView.Image = imageView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
        imageView.TintColor = color.ToPlatform();
    }
    #endregion
}
#endif