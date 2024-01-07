using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace MauiAppDemo;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).ConfigureEffects(effects =>
            {
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        //Register Services
        RegisterTypes(builder.Services);

        return builder.Build();

    }

    private static void RegisterTypes(IServiceCollection services)
    {
        //Add Platform specific Dependencies
        services.AddSingleton(Connectivity.Current);
        services.AddSingleton<IEventAggregator, EventAggregator>();

        //Register Cache Barrel
        #region Cache Registration
        Akavache.Registrations.Start("MauiAppDemo");
        IBlobCache cache = null!;
        ISecureBlobCache secureCache = null!;
        try
        {
            cache = BlobCache.LocalMachine;
            secureCache = BlobCache.Secure;
        }
        catch { }

        services.AddSingleton(cache);
        services.AddSingleton(secureCache); 
        #endregion

        //Register API Service

        //Register View Models
        services.AddScoped<MainPageViewModel>();
        services.AddScoped<ItemDetailPageViewModel>();

        services.AddScoped<HomeView, HomeViewModel>();
        services.AddScoped<NewsView, NewsViewModel>();
    }
}
