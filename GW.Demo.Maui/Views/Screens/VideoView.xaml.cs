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


    private async void _OnLoaded(object? sender, EventArgs e)
    {
        await ((VideoViewModel) Resources["VM"]).OnAppearingAsync();
    }


    private void _OnUnloaded(object? sender, EventArgs e)
    {
        _Unhook();
        _StopAndRelease();

        ((VideoViewModel) Resources["VM"]).OnDisappearing();
    }


    private void _OnHandlerChanged(object? sender, EventArgs e)
    {
        _Unhook();

        if(Window is not null)
        {
            Window.Stopped += _OnWindowStopped;
            Window.Resumed += _OnWindowResumed;
        }
    }


    private void _Unhook()
    {
        if(Window is not null)
        {
            Window.Stopped -= _OnWindowStopped;
            Window.Resumed -= _OnWindowResumed;
        }
    }


    private void _OnWindowStopped(object? sender, EventArgs e)
    {
        _StopAndRelease();
    }

    private void _OnWindowResumed(object? sender, EventArgs e)
    {}


    private void _StopAndRelease()
    {
        try
        {
            if(VPlayer is null) return;
            VPlayer.Stop();
            VPlayer.Source = null;
        }
        catch {}
    }
}
