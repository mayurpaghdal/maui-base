#if ANDROID
using Android.Animation;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;
using AG = Android.Graphics;
using AV = Android.Views;
#endif

#if IOS
using CoreFoundation;
using Foundation;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using UIKit;
using MauiBase.Effects;
#endif

using MC = Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;

namespace MauiBase.Effects;

#region Shared
public static class TouchEffect
{
    public static readonly BindableProperty ColorProperty =
        BindableProperty.CreateAttached(
            "Color",
            typeof(Color),
            typeof(TouchEffect),
            Color.FromRgba(0, 0, 0, 0.2),
            propertyChanged: PropertyChanged
        );

    public static void SetColor(BindableObject view, Color value)
    {
        view.SetValue(ColorProperty, value);
    }

    public static Color GetColor(BindableObject view)
    {
        return (Color)view.GetValue(ColorProperty);
    }

    static void PropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!(bindable is MC.View view))
            return;

        var eff = view.Effects.FirstOrDefault(e => e is TouchRoutingEffect);
        if (GetColor(bindable) != Colors.Transparent)
        {
            view.InputTransparent = false;

            if (eff != null) return;
            view.Effects.Add(new TouchRoutingEffect());
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

public static class EffectsConfig
{
    public static bool AutoChildrenInputTransparent { get; set; } = true;

    public static readonly BindableProperty ChildrenInputTransparentProperty =
        BindableProperty.CreateAttached(
            "ChildrenInputTransparent",
            typeof(bool),
            typeof(EffectsConfig),
            false,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                ConfigureChildrenInputTransparent(bindable);
            }
        );

    public static void SetChildrenInputTransparent(BindableObject view, bool value)
    {
        view.SetValue(ChildrenInputTransparentProperty, value);
    }

    public static bool GetChildrenInputTransparent(BindableObject view)
    {
        return (bool)view.GetValue(ChildrenInputTransparentProperty);
    }

    static void ConfigureChildrenInputTransparent(BindableObject bindable)
    {
        if (!(bindable is Layout layout))
            return;

        if (GetChildrenInputTransparent(bindable))
        {
            foreach (var layoutChild in layout.Children)
                AddInputTransparentToElement((layoutChild as BindableObject)!);
            layout.ChildAdded += Layout_ChildAdded;
        }
        else
        {
            layout.ChildAdded -= Layout_ChildAdded;
        }
    }

    static void Layout_ChildAdded(object sender, ElementEventArgs e)
    {
        AddInputTransparentToElement(e.Element);
    }

    static void AddInputTransparentToElement(BindableObject obj)
    {
        if (obj is MC.View view
            && TouchEffect.GetColor(view) == Colors.Transparent
            && Commands.GetTap(view) == null && Commands.GetLongTap(view) == null)
        {
            view.InputTransparent = true;
        }
    }
}



#region Routing Effects
public class TouchRoutingEffect : RoutingEffect
{
    public TouchRoutingEffect() : base()
    {
    }
}
#endregion

#endregion

#if ANDROID
internal static class TouchCollector
{
    static Dictionary<AV.View, List<Action<AV.View.TouchEventArgs>>> Collection { get; } =
        new Dictionary<AV.View, List<Action<AV.View.TouchEventArgs>>>();

    static AV.View _activeView;

    public static void Add(AV.View view, Action<AV.View.TouchEventArgs> action)
    {
        if (Collection.ContainsKey(view))
        {
            Collection[view].Add(action);
        }
        else
        {
            view.Touch += ActionActivator;
            Collection.Add(view, new List<Action<AV.View.TouchEventArgs>> { action });
        }
    }

    public static void Delete(AV.View view, Action<AV.View.TouchEventArgs> action)
    {
        if (!Collection.ContainsKey(view)) return;

        var actions = Collection[view];
        actions.Remove(action);

        if (actions.Count != 0) return;
        view.Touch -= ActionActivator;
        Collection.Remove(view);
    }

    static void ActionActivator(object sender, AV.View.TouchEventArgs e)
    {
        var view = (AV.View)sender;
        if (!Collection.ContainsKey(view) || (_activeView != null && _activeView != view)) return;

        switch (e.Event?.Action)
        {
            case AV.MotionEventActions.Down:
                _activeView = view;
                //view.PlaySoundEffect(SoundEffects.Click);
                break;

            case AV.MotionEventActions.Up:
            case AV.MotionEventActions.Cancel:
                _activeView = null!;
                e.Handled = true;
                break;
        }

        var actions = Collection[view].ToArray();
        foreach (var valueAction in actions)
        {
            valueAction?.Invoke(e);
        }
    }
}
public class TouchEffectPlatform : PlatformEffect
{
    public bool EnableRipple => OperatingSystem.IsAndroidVersionAtLeast((int)Android.OS.BuildVersionCodes.Lollipop);
    public bool IsDisposed => Container.RootView == null;

    public AV.View View => Control ?? Container;

    AG.Color _color;
    byte _alpha;
    RippleDrawable _ripple;
    FrameLayout _viewOverlay;
    ObjectAnimator _animator;

    public TouchEffectPlatform() : base()
    {
    }

    protected override void OnAttached()
    {
        if (Control is MC.ListView
            || Control is MC.ScrollView)
            return;

        View.Clickable = true;
        View.LongClickable = true;
        _viewOverlay = new FrameLayout(Container.Context!)
        {
            LayoutParameters = new AV.ViewGroup.LayoutParams(-1, -1),
            Clickable = false,
            Focusable = false,
        };
        Container.LayoutChange += ViewOnLayoutChange;

        if (EnableRipple)
            _viewOverlay.Background = CreateRipple(_color);

        SetEffectColor();

        TouchCollector.Add(View, OnTouch);
        (Container as ViewGroup)?.AddView(_viewOverlay);
        _viewOverlay.BringToFront();
    }

    protected override void OnDetached()
    {
        if (IsDisposed) return;

        //_viewOverlay.RemoveFromParent();
        (Container as ViewGroup)?.RemoveView(_viewOverlay);
        _viewOverlay.Pressed = false;
        _viewOverlay.Foreground = null;
        _viewOverlay.Dispose();
        Container.LayoutChange -= ViewOnLayoutChange;

        if (EnableRipple)
            _ripple?.Dispose();

        TouchCollector.Delete(View, OnTouch);
    }

    protected override void OnElementPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(e);

        if (e.PropertyName == TouchEffect.ColorProperty.PropertyName)
        {
            SetEffectColor();
        }
    }

    void SetEffectColor()
    {
        var color = TouchEffect.GetColor(Element);

        if (color == Colors.Transparent)
            return;

        _color = color.ToAndroid();
        _alpha = _color.A == 255 ? (byte)80 : _color.A;

        if (EnableRipple)
            _ripple.SetColor(GetPressedColorSelector(_color));
    }

    void OnTouch(AV.View.TouchEventArgs args)
    {
        switch (args.Event?.Action)
        {
            case MotionEventActions.Down:
                if (EnableRipple)
                    ForceStartRipple(args.Event.GetX(), args.Event.GetY());
                else
                    BringLayer();
                break;

            case MotionEventActions.Up:
            case MotionEventActions.Cancel:
                if (IsDisposed) return;

                if (EnableRipple)
                    ForceEndRipple();
                else
                    TapAnimation(250, _alpha, 0);
                break;
        }
    }

    void ViewOnLayoutChange(object sender, AV.View.LayoutChangeEventArgs layoutChangeEventArgs)
    {
        AV.View group = (sender as AV.ViewGroup)!;

        if (group is null)
            group = (sender as AV.View)!;

        if (group is null || IsDisposed) 
            return;
        _viewOverlay.Right = group.Width;
        _viewOverlay.Bottom = group.Height;
    }

    #region Ripple

    RippleDrawable CreateRipple(AG.Color color)
    {
        var maskColor = App.Current.UserAppTheme == AppTheme.Dark ? AG.Color.Black : AG.Color.White;
        if (Element is Layout)
        {
            var mask = new ColorDrawable(maskColor);
            return _ripple = new RippleDrawable(GetPressedColorSelector(color), null, mask);
        }

        var back = View.Background;
        if (back == null)
        {
            var mask = new ColorDrawable(maskColor);
            return _ripple = new RippleDrawable(GetPressedColorSelector(color), null, mask);
        }

        if (back is RippleDrawable)
        {
            _ripple = (back.GetConstantState()?.NewDrawable() as RippleDrawable)!;
            _ripple.SetColor(GetPressedColorSelector(color));

            return _ripple;
        }

        return _ripple = new RippleDrawable(GetPressedColorSelector(color), back, null);
    }

    static ColorStateList GetPressedColorSelector(int pressedColor)
    {
        return new ColorStateList([[]], [pressedColor,]);
    }

    void ForceStartRipple(float x, float y)
    {

        if (IsDisposed || !(_viewOverlay.Background is RippleDrawable bc)) return;

        _viewOverlay.BringToFront();
        bc.SetHotspot(x, y);

        _viewOverlay.Pressed = true;
    }

    void ForceEndRipple()
    {
        if (IsDisposed) return;

        _viewOverlay.Pressed = false;
    }

    #endregion

    #region Overlay

    void BringLayer()
    {
        if (IsDisposed)
            return;

        ClearAnimation();

        _viewOverlay.BringToFront();
        var color = _color;
        color.A = _alpha;
        _viewOverlay.SetBackgroundColor(color);
    }

    void TapAnimation(long duration, byte startAlpha, byte endAlpha)
    {
        if (IsDisposed)
            return;

        _viewOverlay.BringToFront();

        var start = _color;
        var end = _color;
        start.A = startAlpha;
        end.A = endAlpha;

        ClearAnimation();

        _animator = ObjectAnimator.OfObject(_viewOverlay,
                                            "BackgroundColor",
                                            new ArgbEvaluator(),
                                            start.ToArgb(),
                                            end.ToArgb())!;

        _animator.SetDuration(duration);
        _animator.RepeatCount = 0;
        _animator.RepeatMode = ValueAnimatorRepeatMode.Restart;
        _animator.Start();
        _animator.AnimationEnd += AnimationOnAnimationEnd;
    }

    void AnimationOnAnimationEnd(object sender, EventArgs eventArgs)
    {
        if (IsDisposed) return;

        ClearAnimation();
    }

    void ClearAnimation()
    {
        if (_animator == null) return;
        _animator.AnimationEnd -= AnimationOnAnimationEnd;
        _animator.Cancel();
        _animator.Dispose();
        _animator = null!;
    }
    #endregion
}
#endif

#if IOS
internal static class TouchGestureCollector
{
    static Dictionary<UIView, GestureActionsContainer> Collection { get; } =
        new Dictionary<UIView, GestureActionsContainer>();

    public static void Add(UIView view, Action<TouchGestureRecognizer.TouchArgs> action)
    {
        if (Collection.ContainsKey(view))
        {
            Collection[view].Actions.Add(action);
        }
        else
        {
            var gest = new TouchGestureRecognizer
            {
                CancelsTouchesInView = false,
                Delegate = new TouchGestureRecognizerDelegate(view)
            };
            gest.OnTouch += ActionActivator;
            Collection.Add(view,
                new GestureActionsContainer
                {
                    Recognizer = gest,
                    Actions = new List<Action<TouchGestureRecognizer.TouchArgs>> { action }
                });
            view.AddGestureRecognizer(gest);
        }
    }

    public static void Delete(UIView view, Action<TouchGestureRecognizer.TouchArgs> action)
    {
        if (!Collection.ContainsKey(view)) return;

        var ci = Collection[view];
        ci.Actions.Remove(action);

        if (ci.Actions.Count != 0) return;
        view.RemoveGestureRecognizer(ci.Recognizer);
        Collection.Remove(view);
    }

    static void ActionActivator(object sender, TouchGestureRecognizer.TouchArgs e)
    {
        var gest = (TouchGestureRecognizer)sender;
        if (!Collection.ContainsKey(gest.View)) return;

        var actions = Collection[gest.View].Actions.ToArray();
        foreach (var valueAction in actions)
        {
            valueAction?.Invoke(e);
        }
    }

    class GestureActionsContainer
    {
        public TouchGestureRecognizer Recognizer { get; set; }
        public List<Action<TouchGestureRecognizer.TouchArgs>> Actions { get; set; }
    }
}


public class TouchGestureRecognizer : UIGestureRecognizer
{
    public class TouchArgs : EventArgs
    {
        public TouchState State { get; }
        public bool Inside { get; }

        public TouchArgs(TouchState state, bool inside)
        {
            State = state;
            Inside = inside;
        }
    }

    public enum TouchState
    {
        Started,
        Ended,
        Cancelled
    }

    bool _disposed;
    bool _startCalled;

    public static bool IsActive { get; private set; }

    public bool Processing => State == UIGestureRecognizerState.Began || State == UIGestureRecognizerState.Changed;
    public event EventHandler<TouchArgs> OnTouch;

    public override async void TouchesBegan(NSSet touches, UIEvent evt)
    {
        base.TouchesBegan(touches, evt);
        if (Processing)
            return;

        State = UIGestureRecognizerState.Began;
        IsActive = true;
        _startCalled = false;

        await Task.Delay(125);
        DispatchQueue.MainQueue.DispatchAsync(() =>
        {
            if (!Processing || _disposed) return;
            OnTouch?.Invoke(this, new TouchArgs(TouchState.Started, true));
            _startCalled = true;
        });
    }

    public override void TouchesMoved(NSSet touches, UIEvent evt)
    {
        base.TouchesMoved(touches, evt);

        var inside = View.PointInside(LocationInView(View), evt);

        if (!inside)
        {
            if (_startCalled)
                OnTouch?.Invoke(this, new TouchArgs(TouchState.Ended, false));
            State = UIGestureRecognizerState.Ended;
            IsActive = false;
            return;
        }

        State = UIGestureRecognizerState.Changed;
    }

    public override void TouchesEnded(NSSet touches, UIEvent evt)
    {
        base.TouchesEnded(touches, evt);

        if (!_startCalled)
            OnTouch?.Invoke(this, new TouchArgs(TouchState.Started, true));

        OnTouch?.Invoke(this, new TouchArgs(TouchState.Ended, View.PointInside(LocationInView(View), null)));
        State = UIGestureRecognizerState.Ended;
        IsActive = false;
    }

    public override void TouchesCancelled(NSSet touches, UIEvent evt)
    {
        base.TouchesCancelled(touches, evt);
        OnTouch?.Invoke(this, new TouchArgs(TouchState.Cancelled, false));
        State = UIGestureRecognizerState.Cancelled;
        IsActive = false;
    }

    internal void TryEndOrFail()
    {
        if (_startCalled)
        {
            OnTouch?.Invoke(this, new TouchArgs(TouchState.Ended, false));
            State = UIGestureRecognizerState.Ended;
        }

        State = UIGestureRecognizerState.Failed;
        IsActive = false;
    }

    protected override void Dispose(bool disposing)
    {
        _disposed = true;
        IsActive = false;

        base.Dispose(disposing);
    }
}

public class TouchGestureRecognizerDelegate : UIGestureRecognizerDelegate
{
    readonly UIView _view;

    public TouchGestureRecognizerDelegate(UIView view)
    {
        _view = view;
    }

    public override bool ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer,
        UIGestureRecognizer otherGestureRecognizer)
    {
        if (gestureRecognizer is TouchGestureRecognizer rec && otherGestureRecognizer is UIPanGestureRecognizer &&
            otherGestureRecognizer.State == UIGestureRecognizerState.Began)
        {
            rec.TryEndOrFail();
        }

        return true;
    }

    public override bool ShouldReceiveTouch(UIGestureRecognizer recognizer, UITouch touch)
    {
        if (recognizer is TouchGestureRecognizer && TouchGestureRecognizer.IsActive)
        {
            return false;
        }

        return touch.View == _view;
    }
}

public class TouchEffectPlatform : PlatformEffect
{
    public bool IsDisposed => (Container as IVisualElementRenderer)?.Element == null;
    public UIView View => Control ?? Container;

    UIView _layer;
    nfloat _alpha;

    protected override void OnAttached()
    {
        View.UserInteractionEnabled = true;
        _layer = new UIView
        {
            UserInteractionEnabled = false,
            Opaque = false,
            Alpha = 0,
            TranslatesAutoresizingMaskIntoConstraints = false
        };

        UpdateEffectColor();
        TouchGestureCollector.Add(View, OnTouch);

        View.AddSubview(_layer);
        View.BringSubviewToFront(_layer);
        _layer.TopAnchor.ConstraintEqualTo(View.TopAnchor).Active = true;
        _layer.LeftAnchor.ConstraintEqualTo(View.LeftAnchor).Active = true;
        _layer.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;
        _layer.RightAnchor.ConstraintEqualTo(View.RightAnchor).Active = true;
    }

    protected override void OnDetached()
    {
        TouchGestureCollector.Delete(View, OnTouch);
        _layer?.RemoveFromSuperview();
        _layer?.Dispose();
    }

    void OnTouch(TouchGestureRecognizer.TouchArgs e)
    {
        switch (e.State)
        {
            case TouchGestureRecognizer.TouchState.Started:
                BringLayer();
                break;

            case TouchGestureRecognizer.TouchState.Ended:
                EndAnimation();
                break;

            case TouchGestureRecognizer.TouchState.Cancelled:
                if (!IsDisposed && _layer != null)
                {
                    _layer.Layer.RemoveAllAnimations();
                    _layer.Alpha = 0;
                }

                break;
        }
    }

    protected override void OnElementPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(e);

        if (e.PropertyName == TouchEffect.ColorProperty.PropertyName)
        {
            UpdateEffectColor();
        }
    }

    void UpdateEffectColor()
    {
        var color = TouchEffect.GetColor(Element);
        if (color == Colors.Transparent)
            return;

        _alpha = color.Alpha < 1.0 ? 1 : (nfloat)0.3;
        _layer.BackgroundColor = color.ToPlatform();
    }

    void BringLayer()
    {
        _layer.Layer.RemoveAllAnimations();
        _layer.Alpha = _alpha;
        View.BringSubviewToFront(_layer);
    }

    void EndAnimation()
    {
        if (!IsDisposed && _layer != null)
        {
            _layer.Layer.RemoveAllAnimations();
            UIView.Animate(0.225,
            () =>
            {
                _layer.Alpha = 0;
            });
        }
    }
}
#endif