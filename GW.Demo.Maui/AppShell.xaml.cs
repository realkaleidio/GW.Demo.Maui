namespace GW.Demo.Maui;

using System.Windows.Input;



public partial class AppShell: Shell
{
    public ICommand SelectCommand { get; private init; }


    public AppShell()
    {
        SelectCommand = new Command<string>(async s =>
        {
            int idx = -1;
            if(!int.TryParse(s, out idx)) { idx = -1; }

            if(Shell.Current.CurrentPage is Views.SwipeHostPage p)
            {
                p.NavigateTo(idx);
            }
            else { await Shell.Current.GoToAsync($"//swipe?index={idx}"); }

            Shell.Current.FlyoutIsPresented = false;
        });

        InitializeComponent();
        BindingContext = this;
    }
}
