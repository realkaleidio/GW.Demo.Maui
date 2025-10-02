namespace GW.Demo.Maui.Views.Screens;

using GW.Demo.Maui.ViewModels;
using Microsoft.Extensions.DependencyInjection;



public partial class PostsView : ContentView
{
    public PostsView()
    {
        InitializeComponent();

        Loaded += async (_, _) =>
        {
            await ((PostsViewModel) Resources["VM"]).OnAppearingAsync();
        };
    }
}
