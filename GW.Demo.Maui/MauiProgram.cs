namespace GW.Demo.Maui;

using Microsoft.Extensions.Logging;


public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddTransient<GW.Demo.Maui.ViewModels.SwipeHostViewModel>();
        builder.Services.AddTransient<GW.Demo.Maui.Views.SwipeHostPage>();

        var app = builder.Build();
        ServiceHelper.Services = app.Services;

        return app;
    }
}
