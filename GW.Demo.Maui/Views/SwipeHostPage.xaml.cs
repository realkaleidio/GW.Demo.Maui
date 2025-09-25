namespace GW.Demo.Maui.Views;

using GW.Demo.Maui.ViewModels;
using Microsoft.Maui.Controls;

public partial class SwipeHostPage : ContentPage
{
    public SwipeHostPage(): this(ServiceHelper.GetService<SwipeHostViewModel>() ?? new SwipeHostViewModel())
    {}

    
    public SwipeHostPage(SwipeHostViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
