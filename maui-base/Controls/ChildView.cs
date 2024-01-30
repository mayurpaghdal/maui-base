namespace MauiBase.Controls;

public class ChildView : ContentView
{
    public ChildView()
    {
        Color backgroundColorLight = Colors.White;
        Color backgroundColorDark = Colors.Black;
        if (App.Current.Resources.TryGetValue("Black", out var colorvalue))
            backgroundColorDark = (Color)colorvalue;

        if (App.Current.Resources.TryGetValue("White", out var colorvalueLight))
            backgroundColorLight = (Color)colorvalueLight;

        this.SetAppThemeColor(ContentView.BackgroundColorProperty, backgroundColorLight, backgroundColorDark);
        
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
