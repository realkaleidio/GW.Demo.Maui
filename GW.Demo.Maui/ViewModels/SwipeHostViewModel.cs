namespace GW.Demo.Maui.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;



public partial class SwipeHostViewModel: ObservableObject
{
    private readonly ObservableCollection<int> _Screens = new() { 0, 1, 2, 3, 4 };
    private readonly string[] _Titles = new[] { "Position", "Posts", "Video", "Foto", "Kontakte" };
    private int _CurrentIndex;


    public ObservableCollection<int> Screens
    {
        get { return _Screens; }
    }
    

    public int CurrentIndex
    {
        get { return _CurrentIndex; }
        set 
        {
            if(SetProperty(ref _CurrentIndex, value)) { OnPropertyChanged(nameof(CurrentTitle)); }
        }
    }


    public string CurrentTitle
    {
        get { return _Titles[_CurrentIndex]; }
    }
}
