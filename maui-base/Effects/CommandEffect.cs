#if ANDROID
using Android.Views;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using AV = Android.Views;
using View = Android.Views.View;
#endif

#if IOS
using UIKit;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
#endif

using Microsoft.Maui.Controls.Platform;
using MC = Microsoft.Maui.Controls;
using System.Windows.Input;

namespace MauiBase.Effects;

#region Attached Properties
public static class Commands
{
    public static readonly BindableProperty TapProperty =
        BindableProperty.CreateAttached(
            "Tap",
            typeof(ICommand),
            typeof(Commands),
            default(ICommand),
            propertyChanged: PropertyChanged
        );

    public static void SetTap(BindableObject view, ICommand value)
    {
        view.SetValue(TapProperty, value);
    }

    public static ICommand GetTap(BindableObject view)
    {
        return (ICommand)view.GetValue(TapProperty);
    }

    public static readonly BindableProperty TapParameterProperty =
        BindableProperty.CreateAttached(
            "TapParameter",
            typeof(object),
            typeof(Commands),
            default(object),
            propertyChanged: PropertyChanged
        );

    public static void SetTapParameter(BindableObject view, object value)
    {
        view.SetValue(TapParameterProperty, value);
    }

    public static object GetTapParameter(BindableObject view)
    {
        return view.GetValue(TapParameterProperty);
    }

    public static readonly BindableProperty LongTapProperty =
        BindableProperty.CreateAttached(
            "LongTap",
            typeof(ICommand),
            typeof(Commands),
            default(ICommand),
            propertyChanged: PropertyChanged
        );

    public static void SetLongTap(BindableObject view, ICommand value)
    {
        view.SetValue(LongTapProperty, value);
    }

    public static ICommand GetLongTap(BindableObject view)
    {
        return (ICommand)view.GetValue(LongTapProperty);
    }

    public static readonly BindableProperty LongTapParameterProperty =
        BindableProperty.CreateAttached(
            "LongTapParameter",
            typeof(object),
            typeof(Commands),
            default(object)
        );

    public static void SetLongTapParameter(BindableObject view, object value)
    {
        view.SetValue(LongTapParameterProperty, value);
    }

    public static object GetLongTapParameter(BindableObject view)
    {
        return view.GetValue(LongTapParameterProperty);
    }

    static void PropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!(bindable is MC.View view))
            return;

        var eff = view.Effects.FirstOrDefault(e => e is CommandsRoutingEffect);

        if (GetTap(bindable) != null || GetLongTap(bindable) != null)
        {
            view.InputTransparent = false;

            if (eff != null) return;
            view.Effects.Add(new CommandsRoutingEffect());
            if (EffectsConfig.AutoChildrenInputTransparent && bindable is Layout &&
                !EffectsConfig.GetChildrenInputTransparent(view))
            {
                EffectsConfig.SetChildrenInputTransparent(view, true);
            }
        }
        else
        {
            if (eff == null || view.BindingContext == null) return;
            view.Effects.Remove(eff);
            if (EffectsConfig.AutoChildrenInputTransparent && bindable is Layout &&
                EffectsConfig.GetChildrenInputTransparent(view))
            {
                EffectsConfig.SetChildrenInputTransparent(view, false);
            }
        }
    }
}
#endregion

#region Routing Effect
public class CommandsRoutingEffect : RoutingEffect
{
    public CommandsRoutingEffect() : base()
    {
    }
} 
#endregion

#if ANDROID

public class CommandEffectPlatform : PlatformEffect
{
    public AV.View View => Control ?? Container;
    public bool IsDisposed => (Container as IVisualElementRenderer)?.Element == null;

    DateTime _tapTime;
    readonly Rect _rect = new Rect();
    readonly int[] _location = new int[2];

    protected override void OnAttached()
    {
        View.Clickable = true;
        View.LongClickable = true;
        View.SoundEffectsEnabled = true;
        TouchCollector.Add(View, OnTouch);
    }

    void OnTouch(AV.View.TouchEventArgs args)
    {
        switch (args.Event.Action)
        {
            case MotionEventActions.Down:
                _tapTime = DateTime.Now;
                break;

            case MotionEventActions.Up:
                //if (IsViewInBounds((int)args.Event.RawX, (int)args.Event.RawY))
                //{
                    var range = (DateTime.Now - _tapTime).TotalMilliseconds;
                    if (range > 800)
                        LongClickHandler();
                    else
                        ClickHandler();
                //}
                break;
        }
    }

    bool IsViewInBounds(int x, int y)
    {
        View.GetDrawingRect(_rect);
        View.GetLocationOnScreen(_location);
        _rect.Offset(_location[0], _location[1]);
        return _rect.Contains(x, y);
    }

    void ClickHandler()
    {
        var cmd = Commands.GetTap(Element);
        var param = Commands.GetTapParameter(Element);
        if (cmd?.CanExecute(param) ?? false)
            cmd.Execute(param);
    }

    void LongClickHandler()
    {
        var cmd = Commands.GetLongTap(Element);

        if (cmd == null)
        {
            ClickHandler();
            return;
        }

        var param = Commands.GetLongTapParameter(Element);
        if (cmd.CanExecute(param))
            cmd.Execute(param);
    }

    protected override void OnDetached()
    {
        if (IsDisposed) return;
        TouchCollector.Delete(View, OnTouch);
    }
}
#endif

#if IOS
public class CommandsPlatform : PlatformEffect
{
    public UIView View => Control ?? Container;

    DateTime _tapTime;
    ICommand _tapCommand;
    ICommand _longCommand;
    object _tapParameter;
    object _longParameter;

    protected override void OnAttached()
    {
        View.UserInteractionEnabled = true;

        UpdateTap();
        UpdateTapParameter();
        UpdateLongTap();
        UpdateLongTapParameter();

        TouchGestureCollector.Add(View, OnTouch);
    }

    protected override void OnDetached()
    {
        TouchGestureCollector.Delete(View, OnTouch);
    }

    void OnTouch(TouchGestureRecognizer.TouchArgs e)
    {
        switch (e.State)
        {
            case TouchGestureRecognizer.TouchState.Started:
                _tapTime = DateTime.Now;
                break;

            case TouchGestureRecognizer.TouchState.Ended:
                if (e.Inside)
                {
                    var range = (DateTime.Now - _tapTime).TotalMilliseconds;
                    if (range > 800)
                        LongClickHandler();
                    else
                        ClickHandler();
                }
                break;

            case TouchGestureRecognizer.TouchState.Cancelled:
                break;
        }
    }

    void ClickHandler()
    {
        if (_tapCommand?.CanExecute(_tapParameter) ?? false)
            _tapCommand.Execute(_tapParameter);
    }

    void LongClickHandler()
    {
        if (_longCommand == null)
            ClickHandler();
        else if (_longCommand.CanExecute(_longParameter))
            _longCommand.Execute(_longParameter);
    }

    protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
    {
        base.OnElementPropertyChanged(args);

        if (args.PropertyName == Commands.TapProperty.PropertyName)
            UpdateTap();
        else if (args.PropertyName == Commands.TapParameterProperty.PropertyName)
            UpdateTapParameter();
        else if (args.PropertyName == Commands.LongTapProperty.PropertyName)
            UpdateLongTap();
        else if (args.PropertyName == Commands.LongTapParameterProperty.PropertyName)
            UpdateLongTapParameter();
    }

    void UpdateTap()
    {
        _tapCommand = Commands.GetTap(Element);
    }

    void UpdateTapParameter()
    {
        _tapParameter = Commands.GetTapParameter(Element);
    }

    void UpdateLongTap()
    {
        _longCommand = Commands.GetLongTap(Element);
    }

    void UpdateLongTapParameter()
    {
        _longParameter = Commands.GetLongTapParameter(Element);
    }

    public static void Init()
    {
    }
}
#endif