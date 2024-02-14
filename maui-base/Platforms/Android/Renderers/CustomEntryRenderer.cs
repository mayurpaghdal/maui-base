using Android.Content;
using Android.Graphics.Drawables;
using Android.Util;
using MauiBase.Controls;
using System;
using System.Reflection;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;

namespace Companion.Android.CustomRenderers
{

    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            try
            {
                base.OnElementChanged(e);
                if (e.NewElement != null)
                {
                    var view = (CustomEntry)Element;
                    if (view.IsCurvedCornersEnabled)
                    {
                        // creating gradient drawable for the curved background  
                        var _gradientBackground = new GradientDrawable();
                        _gradientBackground.SetShape(ShapeType.Rectangle);
                        _gradientBackground.SetColor(view.BackgroundColor.ToAndroid());

                        // Thickness of the stroke line  
                        _gradientBackground.SetStroke(view.BorderWidth, view.BorderColor.ToAndroid());

                        // Radius for the curves  
                        _gradientBackground.SetCornerRadius(
                            DpToPixels(this.Context, Convert.ToSingle(view.CornerRadius)));

                        // set the background of the   
                        Control.SetBackground(_gradientBackground);
                    }
                    // Set padding for the internal text from border  
                    Control.SetPadding(
                        (int)DpToPixels(this.Context, Convert.ToSingle(5)), Control.PaddingTop,
                        (int)DpToPixels(this.Context, Convert.ToSingle(5)), Control.PaddingBottom);
                }
            }
            catch (Exception ex)
            {                
               Utils.Util.Instance.LogCrashlytics(string.Format("SessionID : {0}, Pagename : {1}, Methodname : {2}, Error :  {3}", App.SessionID, MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message),ex); 
            }
        }
        public static float DpToPixels(Context context, float valueInDp)
        {
            DisplayMetrics metrics = context.Resources.DisplayMetrics;
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
        }
    }
}