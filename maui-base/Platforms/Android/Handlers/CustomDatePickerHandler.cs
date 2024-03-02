using Android;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Util;
using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace MauiBase.Platforms.Android.Handlers;

public class CustomDatePickerHandler : DatePickerHandler
{
    #region Ctor
    public CustomDatePickerHandler() : base(PropertyMapper)
    {

    }
    #endregion

    public static PropertyMapper<CustomDatePicker, CustomDatePickerHandler> PropertyMapper = new(ViewMapper)
    {
        //[nameof(CustomDatePicker.Text)] = MapControl,
        [nameof(CustomDatePicker.TextColor)] = MapControl,
        [nameof(CustomDatePicker.BorderColor)] = MapControl,
        [nameof(CustomDatePicker.BorderWidth)] = MapControl,
        [nameof(CustomDatePicker.CornerRadius)] = MapControl,
        [nameof(CustomDatePicker.Padding)] = MapControl,
        [nameof(CustomDatePicker.IsEnabled)] = MapControl
    };

    #region Protected Methods
    protected override DatePickerDialog CreateDatePickerDialog(int year, int month, int day)
    {
        var dialog = base.CreateDatePickerDialog(year, month, day);
        dialog.Window?.SetBackgroundDrawableResource(Resource.Drawable.dialog_bg);
        return dialog;
    }
    protected override MauiDatePicker CreatePlatformView()
    {
        var native = new MauiDatePicker(Context);

        return native;
    }

    protected override void ConnectHandler(MauiDatePicker platformView)
    {
        base.ConnectHandler(platformView);

        // Perform any control setup here
        //if (platformView != null)
        //{
        //    platformView.
        //    var view = (CustomDatePicker)Element;
        //    if (view.IsCurvedCornersEnabled)
        //    {
        //        // creating gradient drawable for the curved background  
        //        var _gradientBackground = new GradientDrawable();
        //        _gradientBackground.SetShape(ShapeType.Rectangle);
        //        _gradientBackground.SetColor(view.BackgroundColor.ToAndroid());

        //        // Thickness of the stroke line  
        //        _gradientBackground.SetStroke(view.BorderWidth, view.BorderColor.ToAndroid());

        //        // Radius for the curves  
        //        _gradientBackground.SetCornerRadius(
        //            DpToPixels(this.Context, Convert.ToSingle(view.CornerRadius)));

        //        // set the background of the   
        //        Control.SetBackground(_gradientBackground);
        //    }
        //    // Set padding for the internal text from border  
        //    Control.SetPadding(
        //        (int)DpToPixels(this.Context, Convert.ToSingle(5)), Control.PaddingTop,
        //        (int)DpToPixels(this.Context, Convert.ToSingle(5)), Control.PaddingBottom);
        //}
    }

    protected override void DisconnectHandler(MauiDatePicker platformView)
    {
        // Perform any native view cleanup here
        platformView.Dispose();
        base.DisconnectHandler(platformView);
    }
    #endregion

    #region Private Methods
    //private static void MapText(CustomDatePickerHandler handler, CustomDatePicker view)
    //{
    //    handler.PlatformView.Text = view.Text;
    //    handler.PlatformView?.SetSelection(handler.PlatformView?.Text?.Length ?? 0);
    //}

    private static void MapTextColor(CustomDatePickerHandler handler, CustomDatePicker view)
    {
        handler.PlatformView?.SetTextColor(view.TextColor.ToPlatform());
    }

    private static void MapControl(CustomDatePickerHandler handler, CustomDatePicker entry)
    {
        MapBorder(handler, entry);
        MapPadding(handler, entry);
        //MapText(handler, entry);
        MapTextColor(handler, entry);

        handler.PlatformView.Enabled = entry.IsEnabled;
    }

    private static void MapPadding(CustomDatePickerHandler handler, CustomDatePicker entry)
    {
        var padLeft = DpToPixels(handler.Context, (float)entry.Padding.Left);
        var padTop = DpToPixels(handler.Context, (float)entry.Padding.Top);
        var padRight = DpToPixels(handler.Context, (float)entry.Padding.Right);
        var padBottom = DpToPixels(handler.Context, (float)entry.Padding.Bottom);

        handler.PlatformView?.SetPadding((int)padLeft, (int)padTop, (int)padRight, (int)padBottom);

        MapBorder(handler, entry);
    }

    private static void MapBorder(CustomDatePickerHandler handler, CustomDatePicker view)
    {
        var gd = new GradientDrawable();
        gd.SetColor(view.BackgroundColor.ToPlatform());
        gd.SetCornerRadius(handler.Context.ToPixels(view.CornerRadius));
        gd.SetStroke((int)handler.Context.ToPixels(view.BorderWidth), view.BorderColor.ToPlatform());
        handler.PlatformView.Background = gd;
    }

    private static float DpToPixels(Context context, float valueInDp)
    {
        if (context is null)
            return valueInDp;

        var metrics = context.Resources?.DisplayMetrics!;
        return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
    }
    #endregion
}