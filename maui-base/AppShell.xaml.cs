﻿
namespace MauiBase
{
    public partial class AppShell : Shell
    {
        private readonly INavigationService _navigation;
        public AppShell(INavigationService navigation)
        {
            RegisterRoutes();
            
            InitializeComponent();

            _navigation = navigation;
        }

        protected override async void OnParentSet()
        {
            base.OnParentSet();

            //TODO: Identify where to go? to login or Startup page.
            if (Parent is not null)
            {
                //Login or MainPage
                await _navigation.NavigateAsync("//LoginPage");
            }
        }

        #region Private Methods
        private void RegisterRoutes()
        {
            var assm = Assembly.GetAssembly(typeof(LoginPage));
            var lists = assm?.GetTypes().ToList();

            if (lists is null || lists.Count == 0)
                return;

            var types = lists.Where(t => typeof(IBasePage).IsAssignableFrom(t)
                                         && t != typeof(BasePage)
                                         && t != typeof(BaseContentPage<>)
                                         && t.IsClass
                                         && !t.IsAbstract).ToList();

            types.ForEach(t => Routing.RegisterRoute(t.Name, t));
        }
        #endregion
    }
}
