namespace GW.Demo.Maui.Views.Screens;

using GW.Demo.Maui.ViewModels;

using Microsoft.Maui.Controls;



public partial class ContactsView : ContentView
{
    private bool _IsInit;


    public ContactsView()
    {
        InitializeComponent();

        Loaded += OnLoaded;
    }

    
    private async void OnLoaded(object? sender, EventArgs e)
    {
        if(_IsInit) return;
        _IsInit = true;

        await Task.Delay(350);
        await ((ContactsViewModel) Resources["VM"]).RefreshCommand.ExecuteAsync(null);
    }
}
