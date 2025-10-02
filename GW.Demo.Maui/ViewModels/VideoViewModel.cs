namespace GW.Demo.Maui.ViewModels;

using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


public partial class VideoViewModel: ObservableObject
{
    private readonly string _BaseUrl = "https://test-streams.mux.dev/x36xhzz/x36xhzz.m3u8";
    private string _VideoUrl = string.Empty;
    private bool _IsBusy;


    public string VideoUrl
    {
        get { return _VideoUrl; }
        set { SetProperty(ref _VideoUrl, value); }
    }

    
    public bool IsBusy
    {
        get { return _IsBusy; }
        set { SetProperty(ref _IsBusy, value); }
    }


    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsBusy = true;
        VideoUrl = _BaseUrl + "?r=" + Guid.NewGuid().ToString("N");
        await Task.Delay(50);
        IsBusy = false;
    }


    public async Task OnAppearingAsync()
    {
        if(string.IsNullOrWhiteSpace(VideoUrl)) { await RefreshAsync(); }
    }


    public void OnDisappearing()
    {}
}
