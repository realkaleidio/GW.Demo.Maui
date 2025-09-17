namespace GW.Demo.Maui.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;



public partial class SwipeHostViewModel: ObservableObject
{
    public ObservableCollection<int> Screens
    {
        get { return new() { 0, 1, 2, 3, 4 }; }
    }
        

    private int _CurrentIndex;


    public int CurrentIndex
    {
        get { return _CurrentIndex; }
        set { SetProperty(ref _CurrentIndex, value); }
    }
}
