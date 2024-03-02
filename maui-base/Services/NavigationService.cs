namespace MauiBase.Services;

public interface INavigationService
{
    Common.NavigationMode Mode { get; protected set; }
    Task NavigateAsync(string route, INavigationParameters parameters = default!, bool animate = true);
    Task GoBackAsync(INavigationParameters parameters = default!, bool animate = true);
}

public class NavigationService : INavigationService
{
    #region Properties
    public Common.NavigationMode Mode { get; set; } = Common.NavigationMode.New;
    #endregion

    #region Public Methods
    public Task GoBackAsync(INavigationParameters parameters = default!, bool animate = true)
        => NavigateToPageAsync("..", parameters, animate, Common.NavigationMode.Back);

    public Task NavigateAsync(string route, INavigationParameters parameters = default!, bool animate = true)
        => NavigateToPageAsync(route, parameters, animate);
    #endregion

    #region Private Methods
    private Task NavigateToPageAsync(string route,
                                     INavigationParameters parameters = default!,
                                     bool animate = true,
                                     Common.NavigationMode navMode = Common.NavigationMode.New)
    {
        Mode = navMode;
        if (parameters != null)
        {
            var prams = parameters.ToDictionary();

            return Shell.Current.GoToAsync(route, animate, prams);
        }
        else { return Shell.Current.GoToAsync(route, animate); }
    }
    #endregion
}
