#if ANDROID
using Android.Animation;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using MauiBase.Effects;
using MauiBase.Platforms.GestureCollectors;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using AG = Android.Graphics;
using AV = Android.Views;
using MC = Microsoft.Maui.Controls;

[assembly: ResolutionGroupName("MauiBase.Effects")]
namespace MauiBase.Platforms.Effects;

public class TouchEffectPlatform : PlatformEffect
{
    public bool EnableRipple => true;// Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop;
    public bool IsDisposed => (Container as IVisualElementRenderer)?.Element == null;

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
        if (Control is MC.ListView || Control is MC.ScrollView)
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

        Container.AddChildrenForAccessibility([_viewOverlay]);
        _viewOverlay.BringToFront();
    }

    protected override void OnDetached()
    {
        if (IsDisposed) return;

        _viewOverlay.RemoveFromParent();
        //Container.RemoveView(_viewOverlay);
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
        var group = (AV.ViewGroup)sender;
        if (group == null || IsDisposed) return;
        _viewOverlay.Right = group.Width;
        _viewOverlay.Bottom = group.Height;
    }

    #region Ripple

    RippleDrawable CreateRipple(AG.Color color)
    {
        if (Element is Layout)
        {
            var mask = new ColorDrawable(AG.Color.White);
            return _ripple = new RippleDrawable(GetPressedColorSelector(color), null, mask);
        }

        var back = View.Background;
        if (back == null)
        {
            var mask = new ColorDrawable(AG.Color.White);
            return _ripple = new RippleDrawable(GetPressedColorSelector(color), null, mask);
        }

        if (back is RippleDrawable)
        {
            _ripple = (RippleDrawable)back.GetConstantState().NewDrawable();
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