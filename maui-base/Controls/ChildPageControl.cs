using MauiAppDemo.Helpers;
using System.Reflection;

namespace MauiAppDemo.Controls;

//class ChildPageControl
public class ChildPageControl : ContentView
{
    internal Type _activeType = null!;

    private Assembly ChildViewAssembly = null!;
    private List<string> ViewAppearedCache = [];
    private Dictionary<string, Type> TypeCache = [];
    private Dictionary<string, ChildView> ViewCache = [];

    public ChildPageControl()
    {
        ChildViewAssembly = typeof(HomeView).Assembly; // As ref.
    }

    public bool IsSubMenuVisible
    {
        get { return (bool)GetValue(IsSubMenuVisibleProperty); }
        set
        {
            SetValue(IsSubMenuVisibleProperty, value);
            OnPropertyChanged(nameof(IsSubMenuVisible));
        }
    }

    public static readonly BindableProperty IsSubMenuVisibleProperty = BindableProperty.Create(nameof(IsSubMenuVisible), typeof(bool), typeof(bool), default(bool));

    public string ChildViewName
    {
        get { return (string)GetValue(ChildViewNameProperty); }
        set { SetValue(ChildViewNameProperty, value); }
    }

    public static readonly BindableProperty ChildViewNameProperty =
      BindableProperty.Create(nameof(ChildViewName),
                              typeof(string),
                              typeof(string),
                              null,
                              propertyChanged: OnViewNameChange);

    public NavigationParameters NavigationParameters
    {
        get { return (NavigationParameters)GetValue(NavigationParametersProperty); }
        set { SetValue(NavigationParametersProperty, value); }
    }

    public static readonly BindableProperty NavigationParametersProperty =
      BindableProperty.Create(nameof(NavigationParameters), typeof(NavigationParameters), typeof(NavigationParameters), null, BindingMode.OneWay, null);

    public string ChildViewTitle
    {
        get { return (string)GetValue(ChildViewTitleProperty); }
        set { SetValue(ChildViewTitleProperty, value); }
    }

    public static readonly BindableProperty ChildViewTitleProperty = BindableProperty.Create(nameof(ChildViewTitle), typeof(string), typeof(string), null);

    public IChildViewBase CurrentViewBase
    {
        get { return (IChildViewBase)GetValue(CurrentViewBaseProperty); }
        set { SetValue(CurrentViewBaseProperty, value); }
    }

    public static readonly BindableProperty CurrentViewBaseProperty =
      BindableProperty.Create(nameof(CurrentViewBase), typeof(IChildViewBase), typeof(IChildViewBase), null);

    public static TaskCompletionSource<bool> IsAnimationRunningCompletionSource = new TaskCompletionSource<bool>();

    private static async void OnViewNameChange(BindableObject bindable, object oldValue, object newValue)
    {
        try
        {
            var container = (ChildPageControl)bindable;

            if (oldValue == newValue)
                return;

            if (container.CurrentViewBase != null)
            {
                container.CurrentViewBase.IsActiveSelection = false;

                //Cache existing view with current state.
                if (container.Content is ChildView existingView
                    && existingView != null)
                {
                    container.ViewCache.Remove(existingView.GetType().Name);
                    container.ViewCache.Add(existingView.GetType().Name, existingView);
                }

                await container.CurrentViewBase.DestroyAsync();

                container.CurrentViewBase.OnNavigatedFrom(container.NavigationParameters);

                IsAnimationRunningCompletionSource = new TaskCompletionSource<bool>();
            }

            if (container.Content != null && container.Content is ChildView childViewInstance)
            {
                var childView = container.Content as ChildView;
                childView?.OnViewDisappearing();

                await childViewInstance.FadeTo(0, 50).ContinueWith((t) =>
                {
                    IsAnimationRunningCompletionSource.TrySetResult(true);
                });
            }
            else
                IsAnimationRunningCompletionSource.TrySetResult(true);

            if (string.IsNullOrEmpty(container.ChildViewName))
                return;

            #region Initiate View and its objects
            var viewName = container.ChildViewName;

            var viewObj = container.InitiateView(viewName);

            if (viewObj is null
                || viewObj.Item1 is null)
                return;

            Type viewType = viewObj.Item1;
            ChildView view = viewObj.Item2;
            ChildBaseViewModel vm = viewObj.Item3;
            bool viewExists = viewObj.Item4;
            #endregion

            if (vm == null)
                throw new Exception($"{vm?.GetType()} could not be constructed. Please check its constructor and make sure everything is registered in the container.");

            vm.IsActiveSelection = true;

            container.ChildViewTitle = vm.Title;
            container.CurrentViewBase = vm;
            container.CurrentViewBase.NotifyTitleChange(vm.Title); // For constructor inits.

            container._activeType = viewType;
            App.Instance.ActiveChildVM = vm;

            view.OnViewReAppearing();

            view.Opacity = 0;

            _ = await IsAnimationRunningCompletionSource.Task;

            //if view is not appeared anytime in the app then add it in the cache (to call OnNavigatedTo)
            if (!container.ViewAppearedCache.Contains(viewName))
            {
                viewExists = false;
                container.ViewAppearedCache.Add(viewName);
            }
            //Device.BeginInvokeOnMainThread(async () =>
            //{
            uint delay = 50;

            if (!viewExists)
                delay = 75;

            container.Content = view;
            _ = await view.FadeTo(1, delay);
            //});

            //Call OnNavigatedTo only if the instance is newly created.
            if (!viewExists)
                await vm.OnNavigatedTo(container.NavigationParameters);

            await vm.OnRecurringNavigatedTo(container.NavigationParameters);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    internal Tuple<Type, ChildView, ChildBaseViewModel, bool> InitiateView(string viewName)
    {
        Type viewType = null!;
        ChildView view = default!;
        ChildBaseViewModel viewModel = null!;
        bool viewExists = false;

        try
        {
            if (!TypeCache.ContainsKey(viewName))
            {
                viewType = ChildViewAssembly.ExportedTypes.FirstOrDefault(a => a.Name.Equals(viewName))!;
                TypeCache.Add(viewName, viewType!);
            }
            else
            {
                if (!TypeCache.TryGetValue(viewName, out viewType!))
                    throw new Exception("Failed to get type from cache");
            }

            if (viewType is null)
                return null!;

            viewExists = ViewCache.ContainsKey(viewName);

            if (!viewExists)
            {
                view = (ServiceHelper.GetService(viewType) as ChildView)!;

                var viewAttribute = view.GetType().GetTypeInfo().GetCustomAttribute<ChildViewModelAttribute>();

                if (viewAttribute == null)
                    throw new Exception($"You forgot to add 'ChildViewModel' attribute for {viewType.Name}. Go to {viewType.Name}.xaml.cs and add ChildViewModelAttribute with the type of the ViewModel for it.");

                var vmInstance = ServiceHelper.GetService(viewAttribute.ViewModelType);

                if (vmInstance is not ChildBaseViewModel)
                    throw new Exception("Type in the attribute is not a ChildViewBase. Modify it.");

                viewModel = (ChildBaseViewModel)vmInstance;
                view.BindingContext = viewModel 
                                      ?? throw new Exception($"{viewAttribute.ViewModelType} could not be constructed. Please check its constructor and make sure everything is registered in the container.");
                
                ViewCache.Add(viewName, view);
                view.OnViewAppearing();
            }
            else
            {
                view = ViewCache.First(k => k.Key == viewName).Value;
                viewModel = (view.BindingContext as ChildBaseViewModel)!;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            //Util.Instance.LogCrashlytics(string.Format("SessionID : {0}, Pagename : {1}, Methodname : {2}, Error :  {3}", App.SessionID, MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
        }

        return new Tuple<Type, ChildView, ChildBaseViewModel, bool>(viewType, view, viewModel, viewExists);
    }
}
