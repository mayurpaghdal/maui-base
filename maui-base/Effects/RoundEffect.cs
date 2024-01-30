#if ANDROID
using Android.Graphics;
using AG = Android.Graphics;
using AV = Android.Views;
using Android.Content;
using Android.Util;
#endif

#if IOS
using UIKit;
#endif

using Microsoft.Maui.Controls.Platform;
using MC = Microsoft.Maui.Controls;

namespace MauiBase.Effects;

#region Shared
public static class RoundEffect
{
    #region CornerRadius
    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.CreateAttached(
            "CornerRadius",
            typeof(float),
            typeof(TouchEffect),
            default!,
            propertyChanged: PropertyChanged
        );

    public static void SetCornerRadius(BindableObject view, float value)
    {
        view.SetValue(CornerRadiusProperty, value);
    }

    public static float GetCornerRadius(BindableObject view)
    {
        return (float)view.GetValue(CornerRadiusProperty);
    }
    #endregion

    #region UseAutoRadius
    public static readonly BindableProperty UseAutoRadiusProperty =
        BindableProperty.CreateAttached(
            "UseAutoRadius",
            typeof(bool),
            typeof(TouchEffect),
            default!,
            propertyChanged: PropertyChanged
        );

    public static void SetUseAutoRadius(BindableObject view, bool value)
    {
        view.SetValue(UseAutoRadiusProperty, value);
    }

    public static bool GetUseAutoRadius(BindableObject view)
    {
        return (bool)view.GetValue(UseAutoRadiusProperty);
    }
    #endregion

    static void PropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!(bindable is MC.View view))
            return;

        var eff = view.Effects.FirstOrDefault(e => e is RoundRoutingEffect);
        if (GetCornerRadius(bindable) != default!)
        {
            view.InputTransparent = false;

            if (eff != null) return;
            view.Effects.Add(new RoundRoutingEffect());
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
public class RoundRoutingEffect : RoutingEffect
{
    public RoundRoutingEffect() : base()
    {
    }
}
#endregion

#if ANDROID
public class RoundEffectPlatform : PlatformEffect
{
    AV.ViewOutlineProvider originalProvider;
    private AV.View _view; 

    public RoundEffectPlatform() : base()
    {
        _view = Control ?? Container;
    }

    protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
    {
        base.OnElementPropertyChanged(args);

        if (args.PropertyName == RoundEffect.CornerRadiusProperty.PropertyName
            || args.PropertyName == RoundEffect.UseAutoRadiusProperty.PropertyName)
        {
            OnAttached();
        }
    }

    protected override void OnAttached()
    {
        try
        {
            if (_view is null) return;

            originalProvider = _view.OutlineProvider!;
            _view.OutlineProvider = new CornerRadiusOutlineProvider(Element, _view.Context!);
            _view.ClipToOutline = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to set corner radius: {ex.Message}");
        }
    }

    protected override void OnDetached()
    {
        if (_view != null)
        {
            _view.OutlineProvider = originalProvider;
            _view.ClipToOutline = false;
        }
    }

    class CornerRadiusOutlineProvider : AV.ViewOutlineProvider
    {
        Element element;
        Context _context;

        public CornerRadiusOutlineProvider(Element formsElement, Context context)
        {
            element = formsElement;
            _context = context;
        }

        public override void GetOutline(AV.View? view, Outline? outline)
        {
            if (element is null
                || view is null
                || outline is null)
                return;

            var cornerRadius = RoundEffect.GetCornerRadius(element)!;
            var hasAutoRadius = RoundEffect.GetUseAutoRadius(element)!;

            float scale = view.Resources?.DisplayMetrics?.Density ?? 0;
            double width = (double)element.GetValue(VisualElement.WidthProperty) * scale;
            double height = (double)element.GetValue(VisualElement.HeightProperty) * scale;
            float minDimension = (float)Math.Min(height, width);
            float radius = hasAutoRadius ? minDimension / 2f : cornerRadius;
            AG.Rect rect = new(0, 0, (int)width, (int)height);
            outline.SetRoundRect(rect, DpToPixels(_context, radius));
        }

        private float DpToPixels(Context context, float valueInDp)
        {
            DisplayMetrics metrics = context.Resources.DisplayMetrics!;
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
        }
    }
}
#endif

#if IOS
public class RoundEffectPlatform : PlatformEffect
{
    nfloat originalRadius;
    public UIView View => Control ?? Container;

    #region Overridden Methods
    protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
    {
        base.OnElementPropertyChanged(args);

        if (args.PropertyName == RoundEffect.CornerRadiusProperty.PropertyName
            || args.PropertyName == RoundEffect.UseAutoRadiusProperty.PropertyName)
        {
            OnAttached();
        }
    }

    protected override void OnAttached()
    {
        try
        {
            var cornerRadius = RoundEffect.GetCornerRadius(Element)!;
            var hasAutoRadius = RoundEffect.GetUseAutoRadius(Element)!;

            originalRadius = View.Layer.CornerRadius;
            View.ClipsToBounds = true;

            if (hasAutoRadius)
                View.Layer.CornerRadius = CalculateRadius();
            else
                View.Layer.CornerRadius = cornerRadius;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to set corner radius: {ex.Message}");
        }
    }

    protected override void OnDetached()
    {
        if (View is null
            || View.Layer is null)
            return;

        View.ClipsToBounds = false;
        View.Layer.CornerRadius = originalRadius;
    }
    #endregion

    #region Private Methods
    private float CalculateRadius()
    {
        double width = (double)Element.GetValue(VisualElement.WidthRequestProperty);
        double height = (double)Element.GetValue(VisualElement.HeightRequestProperty);
        float minDimension = (float)Math.Min(height, width);
        float radius = minDimension / 2f;

        return radius;
    }
    #endregion
}
#endif