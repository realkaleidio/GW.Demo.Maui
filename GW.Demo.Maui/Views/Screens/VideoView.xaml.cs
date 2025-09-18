using CommunityToolkit.Maui.Core;

using GW.Demo.Maui.ViewModels;

namespace GW.Demo.Maui.Views.Screens;



public partial class VideoView: ContentView
{
    public VideoView()
    {
        InitializeComponent();

        Loaded += async (_, _) => { await ((VideoViewModel) Resources["VM"]).OnAppearingAsync(); };
        Unloaded += (_, _) => { ((VideoViewModel) Resources["VM"]).OnDisappearing(); };
    }
}

