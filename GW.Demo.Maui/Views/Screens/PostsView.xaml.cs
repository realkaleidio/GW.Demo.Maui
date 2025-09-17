namespace GW.Demo.Maui.Views.Screens;

using GW.Demo.Maui.ViewModels;
using Microsoft.Extensions.DependencyInjection;



public partial class PostsView : ContentView
{
    private readonly PostsViewModel _Vm;


    public PostsView()
    {
        InitializeComponent();

        Resources["VM"] = BindingContext = _Vm = Application.Current?.Handler?.MauiContext?.Services?.GetService<PostsViewModel>()!;

        BindingContextChanged += (_, _) => { if(BindingContext is not PostsViewModel) { BindingContext = _Vm; } };
        Loaded += async (_, _) => { await _Vm.OnAppearingAsync(); };
    }
}
