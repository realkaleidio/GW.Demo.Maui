namespace GW.Demo.Maui.ViewModels;

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using GW.Demo.Maui.Posts;
using GW.Demo.Maui.Utility;



public class PostsViewModel: ObservableObject
{
    private static readonly HttpClient _HttpClient = new HttpClient();
    private readonly PostsStore _Store;
    private CancellationTokenSource? _Cts;
    private string _Query = string.Empty;
    private bool _IsBusy;
    private IAsyncRelayCommand? _SearchCommand;
    private IAsyncRelayCommand? _RefreshCommand;



    public PostsViewModel(): this(ServiceHelper.GetService<PostsStore>() ?? new PostsStore(new PostsClient(_HttpClient)))
    {}


    public PostsViewModel(PostsStore store)
    {
        _Store = store;
        _Store.Items.CollectionChanged += _OnItemsChanged;
    }


    public ObservableCollection<Post> Items
    {
        get { return _Store.Items; }
    }


    public string Query
    {
        get { return _Query; }
        set { SetProperty(ref _Query, value); }
    }

    
    public bool IsBusy
    {
        get { return _IsBusy; }
        set 
        { 
            if(SetProperty(ref _IsBusy, value)) OnPropertyChanged(nameof(IsEmpty));
        }
    }
    
    
    public bool IsEmpty
    {
        get { return !_IsBusy && Items.Count == 0; }
    }


    public IAsyncRelayCommand SearchCommand
    {
        get { return _SearchCommand ??= new AsyncRelayCommand(_LoadAsync); }
    }

        
    public IAsyncRelayCommand RefreshCommand
    {
        get { return _RefreshCommand ??= new AsyncRelayCommand(_LoadAsync); }
    }


    public async Task OnAppearingAsync()
    {
        if(Items.Count == 0) { await _LoadAsync(); }
    }


    private async Task _LoadAsync()
    {
        _CancelPending();
        _Cts = new CancellationTokenSource();

        try
        {
            IsBusy = true;
            await _Store.LoadAsync(string.IsNullOrWhiteSpace(Query) ? null : Query, _Cts.Token);
        }
        finally { IsBusy = false; }
    }


    private void _CancelPending()
    {
        try { _Cts?.Cancel(); } catch {}
        _Cts?.Dispose();
        _Cts = null;
    }


    private void _OnItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(IsEmpty));
    }
}
