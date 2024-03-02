namespace MauiBase.Controls;

public class SkeletonView : BoxView
{
    public SkeletonView()
    {
        Dispatcher.StartTimer(TimeSpan.FromSeconds(0.5), () =>
        {
            this.FadeTo(0.4, 750, Easing.CubicInOut).ContinueWith((x) =>
            {
                this.FadeTo(0.7, 750, Easing.CubicInOut);
            });

            return true;
        });
    }
}
