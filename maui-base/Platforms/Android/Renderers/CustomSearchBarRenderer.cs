using Android.Content;
using Android.Graphics.Drawables;
using Android.Widget;
using Companion.Android.CustomRenderers;
using Companion.Controls;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomSearchBar), typeof(CustomSearchBarRenderer))]
namespace Companion.Android.CustomRenderers
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        public CustomSearchBarRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (Control == null || e.NewElement == null) return;

            //var padding = (int)UXDivers.Grial.SearchBarProperties.GetHorizontalPadding(e.NewElement);
            //this.Control.SetPadding(padding, padding - 3, padding, 0);
            var view = (CustomSearchBar)Element;

            DrawControl(e.NewElement, view);
        }

        void DrawControl(object sender, CustomSearchBar view)
        {
            var padding = new Thickness(0, 0, 0, 0);

            var plateId = Resources.GetIdentifier("android:id/search_plate", null, null);
            var plate = Control.FindViewById(plateId);
            plate.SetBackgroundColor(Color.Transparent.ToAndroid());
            plate.SetPadding((int)padding.Left, (int)padding.Top, (int)padding.Right, (int)padding.Bottom);

            GradientDrawable shape = new GradientDrawable();
            shape.SetShape(ShapeType.Rectangle);
            var backColor = view.IsPlainEntry ? Color.Transparent.ToAndroid() : view.CurveBackgroundColor.ToAndroid();
            shape.SetColor(backColor);

            int stroke = view.BorderWidth;
            var brdrColor = view.IsPlainEntry ? Color.Transparent.ToAndroid() : view.BorderColor.ToAndroid();
            shape.SetStroke(stroke, brdrColor);

            var radius = (float)(view.CornerRadius * 2);
            shape.SetCornerRadius(radius);

            this.Control.SetBackground(shape);

            //Hide search icon
            int searchIconId = Context.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
            ImageView searchViewIcon = (ImageView)Control.FindViewById<ImageView>(searchIconId);
            var iconPadding = view.IconPadding;

            // search close button icon, and attach closeIcon.click.
            int searchCloseButtonId = Context.Resources.GetIdentifier("android:id/search_close_btn", null, null);
            var closeIcon = Control.FindViewById(searchCloseButtonId);

            closeIcon.Click += CloseIcon_Click;


            if (searchViewIcon != null)
            {
                searchViewIcon?.SetPadding(-20, (int)iconPadding.Top, (int)iconPadding.Right, (int)iconPadding.Bottom);
                searchViewIcon.Left = -20;// (0,(int)iconPadding.Top, (int)iconPadding.Right, (int)iconPadding.Bottom);
                                          //searchViewIcon.SetImageDrawable(null);
            }
        }

        private void CloseIcon_Click(object sender, EventArgs e)
        {
            if (Element is null)
                return;

            Element.Text = string.Empty;
            Element.Unfocus();
        }
    }
}