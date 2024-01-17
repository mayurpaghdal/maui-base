namespace MauiBase.Helpers;

public static class ServiceHelper
{
    public static TService GetService<TService>() => Current.GetService<TService>()!;
    public static object GetService(Type type) => Current.GetRequiredService(type)!;

    public static IServiceProvider Current =>
#if ANDROID
MauiApplication.Current.Services;
#elif IOS || MACCATALYST
			MauiUIApplicationDelegate.Current.Services;
#else
			null;
#endif
}
