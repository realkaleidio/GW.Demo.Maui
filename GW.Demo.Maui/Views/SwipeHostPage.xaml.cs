namespace GW.Demo.Maui.Views;

using System.Collections;
using System.Collections.Specialized;

using GW.Demo.Maui.Utility;
using GW.Demo.Maui.ViewModels;

using Microsoft.Maui.Controls;



[QueryProperty(nameof(Index), "index")]
public partial class SwipeHostPage: ContentPage
{
    private int _Index = -1;
    private bool _IsInit = false;


    public SwipeHostPage(): this(ServiceHelper.GetService<SwipeHostViewModel>() ?? new SwipeHostViewModel())
    {}
    
        
    public SwipeHostPage(SwipeHostViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        Carousel.Loaded += (_, _) =>
        {
            _Navigate();
            if(!_IsInit)
            {
                if(_Index >= 0) return;
                if(Carousel?.ItemsSource == null) return;

                NavigateTo(0);
                _IsInit = true;
            }
        };
    }


    public int Index
    {
        get { return _Index; }
        set { _Index = value; _Navigate(); }
    }


    public void NavigateTo(int index)
    {
        _Index = index;
        _Navigate();
    }


    private static int _Count(object itemsSource)
    {
        if(itemsSource is ICollection c) { return c.Count; }
        if(itemsSource is IEnumerable e) { return e.Cast<object>().Count(); }

        return 0;
    }


    private void _OnItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        _Navigate();
    }


    private void _Navigate()
    {
        if(_Index < 0 || Carousel is null || Carousel.ItemsSource is null) { return; }

        int count = _Count(Carousel.ItemsSource);
        if(count <= 0) { return; }

        Carousel.ScrollTo(Math.Clamp(_Index, 0, count - 1), position: ScrollToPosition.Center, animate: false);

        _Index = -1;
    }
      

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        _Navigate();
        
        if(Carousel?.ItemsSource is INotifyCollectionChanged incc) { incc.CollectionChanged += _OnItemsChanged; }
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();
        _Navigate();
    }


    protected override void OnDisappearing()
    {
        if(Carousel?.ItemsSource is INotifyCollectionChanged incc) { incc.CollectionChanged -= _OnItemsChanged; }
        base.OnDisappearing();
    }
}
