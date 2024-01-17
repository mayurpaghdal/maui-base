namespace MauiBase.Controls;

public class ChildView : ContentView
{
    public ChildView()
    {
        if (App.Current.Resources.TryGetValue("AppBackgroundColor", out var colorvalue))
            BackgroundColor = (Color)colorvalue;
        else
            BackgroundColor = Colors.White;
    }

    public EventHandler ValueChanged { get; set; }

    public virtual void OnViewAppearing()
    {

    }

    public virtual void OnViewReAppearing()
    {

    }

    public virtual void OnViewDisappearing()
    {

    }
}
