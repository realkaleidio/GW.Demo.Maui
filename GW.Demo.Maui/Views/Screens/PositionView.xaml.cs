namespace GW.Demo.Maui.Views.Screens;

using GW.Demo.Maui.ViewModels;



public partial class PositionView: ContentView
{
    private IDispatcherTimer? _Timer;


    public PositionView()
    {
        InitializeComponent();
    }


    private async void _OnLoaded(object? sender, EventArgs e)
    {
        await ((PositionViewModel) Resources["VM"]).RefreshAsync();

        if(_Timer == null)
        {
            _Timer = Dispatcher.CreateTimer();
            _Timer.Interval = new TimeSpan(0, 0, 0, 0, 350);
            _Timer.IsRepeating = true;
            _Timer.Tick += async (_, __) => { if(IsVisible) { await ((PositionViewModel) Resources["VM"]).RefreshAsync(); } };
        }

        if(!_Timer.IsRunning) { _Timer.Start(); }
    }


    private void _OnUnloaded(object? sender, EventArgs e)
    {
        if(_Timer != null && _Timer.IsRunning) { _Timer.Stop(); }
    }
}
