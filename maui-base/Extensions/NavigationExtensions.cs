namespace MauiBase.Extensions
{
    public static class NavigationExtensions
    {
        public static async Task NavigateAsync(this INavigation navigation, BasePage page, bool isModalNavigation = false, bool isAnimated = true)
        {
            if (page is null || navigation is null)
                return;

            if (isModalNavigation)
                await navigation.PushModalAsync(page, isAnimated);
            else
                await navigation.PushAsync(page, isAnimated);
        }

        public static async Task GoBackAsync(this INavigation navigation, string currentVmName, bool isAnimated = true)
        {
            if (navigation.ModalStack.Count > 0
                && navigation.ModalStack.Any(n => n.BindingContext is not null && n.BindingContext.GetType().Name.Contains(currentVmName)))
            {
                await navigation.PopModalAsync(isAnimated);
            }
            else if (navigation.NavigationStack.Count > 0
                     && navigation.NavigationStack.Any(n => n.BindingContext is not null && n.BindingContext.GetType().Name.Contains(currentVmName)))
                await navigation.PopAsync(isAnimated);
        }
    }
}
