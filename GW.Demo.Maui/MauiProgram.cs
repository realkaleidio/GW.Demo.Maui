namespace GW.Demo.Maui;

using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;

using GW.Demo.Maui.Posts;
using GW.Demo.Maui.Utility;
using GW.Demo.Maui.ViewModels;
using GW.Demo.Maui.Views;

using Microsoft.Extensions.Logging;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMediaElement(static options => { options.SetDefaultAndroidViewType(AndroidViewType.TextureView); })
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
