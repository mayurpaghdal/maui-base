#if ANDROID
using AV = Android.Views;

namespace MauiBase.Platforms.GestureCollectors;

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
#endif