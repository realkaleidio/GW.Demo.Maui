namespace GW.Demo.Maui;

using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using GW.Demo.Maui.ViewModels;
using GW.Demo.Maui.Views;
using GW.Demo.Maui.Posts;

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
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddTransient<SwipeHostViewModel>()
                        .AddTransient<SwipeHostPage>()
                        .AddTransient<PostsViewModel>()
                        .AddSingleton<PostsStore>()
                        .AddHttpClient<IPostsClient, PostsClient>();

        var app = builder.Build();
        ServiceHelper.Services = app.Services;

        return app;
    }
}
