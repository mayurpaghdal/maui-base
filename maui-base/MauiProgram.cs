using Maui.FreakyEffects;
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
                effects.InitFreakyEffects();
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

        //Register Cache Barrel
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

        //Register API Service

        //Register View Models
        services.AddSingleton<MainPageViewModel>();
        services.AddScoped<ItemDetailPageViewModel>();

        services.AddScoped<HomeView>();
        services.AddScoped<NewsView>();

        services.AddScoped<HomeViewModel>();
        services.AddScoped<NewsViewModel>();

        App.Services = services.BuildServiceProvider();
    }
}
