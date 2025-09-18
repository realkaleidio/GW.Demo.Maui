namespace GW.Demo.Maui.Views.Screens;

using GW.Demo.Maui.ViewModels;
using Microsoft.Maui.Controls;
using System;



public partial class PhotoView: ContentView
{
    public PhotoView()
    {
        InitializeComponent();
        this.Loaded += _OnLoaded;
        this.Unloaded += _OnUnloaded;
    }


    private void _OnLoaded(object? sender, EventArgs e)
    {
        var vm = (PhotoViewModel)Resources["VM"];
        vm.Load();
    }


    private void _OnUnloaded(object? sender, EventArgs e)
    {
        this.Loaded -= _OnLoaded;
        this.Unloaded -= _OnUnloaded;
    }
}
