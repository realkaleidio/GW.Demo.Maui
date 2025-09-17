namespace GW.Demo.Maui.ViewModels;

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using GW.Demo.Maui.Posts;



public class PostsViewModel: ObservableObject
{
    private readonly PostsStore _Store;
    private CancellationTokenSource? _Cts;
    private string _Query = string.Empty;
    private bool _IsBusy;
    private IAsyncRelayCommand? _SearchCommand;
    private IAsyncRelayCommand? _RefreshCommand;
    private IAsyncRelayCommand? _SaveLocalCommand;
    private IAsyncRelayCommand? _LoadLocalCommand;


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
        get { return _SearchCommand ??= new AsyncRelayCommand(_LoadFromApiAsync); }
    }

        
    public IAsyncRelayCommand RefreshCommand
    {
        get { return _RefreshCommand ??= new AsyncRelayCommand(_LoadFromApiAsync); }
    }

    
    public IAsyncRelayCommand SaveLocalCommand
    {
        get { return _SaveLocalCommand ??= new AsyncRelayCommand(_SaveLocalAsync); }
    }

    
    public IAsyncRelayCommand LoadLocalCommand
    {
        get { return _LoadLocalCommand ??= new AsyncRelayCommand(_LoadLocalAsync); }
    }


    public async Task OnAppearingAsync()
    {
        bool hadCache = await _LoadLocalAsync();
        if(Items.Count == 0) { await _LoadFromApiAsync(); }
    }


    private async Task _LoadFromApiAsync()
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


    private async Task _SaveLocalAsync()
    {
        _CancelPending();
        _Cts = new CancellationTokenSource();

        await _Store.SaveAsJsonAsync(_Cts.Token);
    }


    private async Task<bool> _LoadLocalAsync()
    {
        _CancelPending();
        _Cts = new CancellationTokenSource();

        return await _Store.LoadFromJsonAsync(_Cts.Token);
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
