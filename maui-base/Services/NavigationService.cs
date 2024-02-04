namespace MauiBase.Services;

public interface INavigationService
{
    Task NavigateAsync(string route, INavigationParameters parameters = default!, bool animate = true);
    Task GoBackAsync(INavigationParameters parameters = default!, bool animate = true);
}

public class NavigationService : INavigationService
{
    public Task GoBackAsync(INavigationParameters parameters = default!, bool animate = true)
     => NavigateAsync("..", parameters, animate);

    public Task NavigateAsync(string route, INavigationParameters parameters = default!, bool animate = true)
    {
        if (parameters != null)
        {
            var prams = parameters.ToDictionary();

                return Shell.Current.GoToAsync(route, animate, prams);
        }
        else { return Shell.Current.GoToAsync(route, animate); }
    }
}
