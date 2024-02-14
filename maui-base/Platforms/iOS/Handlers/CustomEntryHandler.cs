using CoreGraphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace MauiBase.Platforms.iOS.Handlers;

public class CustomEntryHandler : EntryHandler
{
    #region Ctor
    public CustomEntryHandler() : base(PropertyMapper)
    {

    }
    #endregion

    public static PropertyMapper<CustomEntry, CustomEntryHandler> PropertyMapper = new(ViewHandler.ViewMapper)
    {
        [nameof(CustomEntry.Text)] = MapText,
        [nameof(CustomEntry.TextColor)] = MapTextColor,
        [nameof(CustomEntry.BorderColor)] = MapBorder,
        [nameof(CustomEntry.BorderWidth)] = MapBorder,
        [nameof(CustomEntry.CornerRadius)] = MapBorder
    };


    #region Protected Methods
    protected override MauiTextField CreatePlatformView()
    {
        var nativeEntry = new MauiTextField();
        var myentry = VirtualView as CustomEntry;

        //if (myentry.UnderlineThickness == 0)
        //{   // Hide Underline.
        //    nativeEntry.PaintFlags &= ~Android.Graphics.PaintFlags.UnderlineText;
        //    //nativeEntry.Background = null;
        //    //nativeEntry.SetBackgroundColor(global::Android.Graphics.Color.Transparent);
        //}
        //else
        //{   // Show Underline. (Is only under the typed text, not the whole control.)
        //    nativeEntry.PaintFlags |= Android.Graphics.PaintFlags.UnderlineText;
        //    // TODO: Line thickness and color. For color, see https://stackoverflow.com/a/62486103/199364.
        //    // For thickness, probably need to "nest controls", similar to Windows implementation.
        //}

        return nativeEntry;
    }

    protected override void ConnectHandler(MauiTextField platformView)
    {
        base.ConnectHandler(platformView);

        // Perform any control setup here
        //if (platformView != null)
        //{
        //    platformView.
        //    var view = (CustomEntry)Element;
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

    protected override void DisconnectHandler(MauiTextField platformView)
    {
        // Perform any native view cleanup here
        platformView.Dispose();
        base.DisconnectHandler(platformView);
    }
    #endregion

    #region Private Methods
    private static void MapBorder(CustomEntryHandler handler, CustomEntry view)
    {
        handler.PlatformView.LeftView = new UIView(new CGRect(0f, 0f, 9f, 20f));
        handler.PlatformView.LeftViewMode = UITextFieldViewMode.Always;

        handler.PlatformView.KeyboardAppearance = UIKeyboardAppearance.Dark;
        handler.PlatformView.ReturnKeyType = UIReturnKeyType.Done;

        // Radius for the curves  
        handler.PlatformView.Layer.CornerRadius = Convert.ToSingle(view.CornerRadius);
        // Thickness of the Border Color  
        handler.PlatformView.Layer.BorderColor = view.BorderColor.ToCGColor();
        // Thickness of the Border Width  
        handler.PlatformView.Layer.BorderWidth = view.BorderWidth;
        handler.PlatformView.ClipsToBounds = true;
        handler.PlatformView.BackgroundColor = view.BackgroundColor.ToPlatform();
    }
    #endregion
}
