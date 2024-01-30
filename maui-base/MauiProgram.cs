using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using Mopups.Hosting;
using Mopups.Interfaces;
using Mopups.Services;

namespace MauiBase;

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
            })
            .UseMauiCompatibility()
            .ConfigureMauiHandlers(handlers =>
            {
                //handlers.AddCompatibilityRenderer(typeof(TouchRoutingEffect), typeof(TouchEffectPlatform));
            })
            .ConfigureEffects(effects =>
            {
                effects.Add<TouchRoutingEffect, TouchEffectPlatform>(); 
                effects.Add<IconTintColorRoutingEffect, IconTintColorEffectPlatform>(); 
                effects.Add<CommandsRoutingEffect, CommandEffectPlatform>();
                effects.Add<RoundRoutingEffect, RoundEffectPlatform>();
            })
            .ConfigureMopups();

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
        services.AddSingleton(MopupService.Instance);
        
        //Register Cache
        #region Cache Registration
        Akavache.Registrations.Start("MauiBase");
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
        services.AddScoped<NewsFilterPageViewModel>();
        services.AddScoped<ItemDetailPageViewModel>();

        services.AddScoped<HomeView, HomeViewModel>();
        services.AddScoped<NewsView, NewsViewModel>();
    }
}
