namespace GW.Demo.Maui.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Threading.Tasks;

using GW.Demo.Maui.Contacts;
using System.Collections.ObjectModel;



public partial class ContactsViewModel : ObservableObject
{
    private bool _IsRefreshing;
    private bool _IsSupported = true;
    private int _Calling = 0;
    private string _UnsupportedReason = string.Empty;
    private readonly SemaphoreSlim _LoadSemaphore = new(1, 1);


    public ContactsViewModel()
    {}


    public bool IsRefreshing
    {
        get { return _IsRefreshing; }
        private set { SetProperty(ref _IsRefreshing, value); }
    }


    public ObservableCollection<ContactItem> Contacts 
    { 
        get;
    } = new();


    public bool IsSupported
    {
        get { return _IsSupported; }
        set
        {
            if(SetProperty(ref _IsSupported, value)) { OnPropertyChanged(nameof(IsUnsupported)); }
        }
    }


    public bool IsUnsupported 
    { 
        get { return !IsSupported; } 
    }

    
    public string UnsupportedReason
    {
        get { return _UnsupportedReason; }
        set { SetProperty(ref _UnsupportedReason, value); }
    }


    [RelayCommand]
    private async Task RefreshAsync()
    {
        if(!await _LoadSemaphore.WaitAsync(0)) return;
        IsRefreshing = true;
        try 
        {
            await _LoadAsync(); 
        }
        finally { IsRefreshing = false; _LoadSemaphore.Release(); }
    }

    
    [RelayCommand]
    private async Task CallAsync(ContactItem? item)
    {
        if(item is null) return;
        if(Interlocked.Exchange(ref _Calling, 1) == 1) return;

        try
        {
            try { HapticFeedback.Perform(HapticFeedbackType.Click); } catch {}

            var number = (item.PhoneNumber ?? string.Empty).Trim();
            if(string.IsNullOrWhiteSpace(number))
            {
                await Shell.Current.DisplayAlert("Kein Anruf möglich", "Keine Telefonnummer hinterlegt.", "OK");
                return;
            }

            PhoneDialer.Open(number);
        }
        catch(Exception)
        {
            await Shell.Current.DisplayAlert("Fehler", "Der Anruf konnte nicht gestartet werden.", "OK");
        }
        finally
        {
            Interlocked.Exchange(ref _Calling, 0);
        }
    }


    private async Task _LoadAsync()
    {
        #if ANDROID || IOS || MACCATALYST
            IsSupported = true;
            UnsupportedReason = string.Empty;

            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();
            if(status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.ContactsRead>();
            }

            if(status != PermissionStatus.Granted)
            {
                IsSupported = false;
                UnsupportedReason = "Kein Zugriff auf Kontakte.";
                return;
            }

            try
            {
                IEnumerable<Contact> contacts = await Microsoft.Maui.ApplicationModel.Communication.Contacts.Default.GetAllAsync().ConfigureAwait(false);
                IEnumerable<ContactItem> items = await Task.Run(() =>
                {
                    return contacts.Select(c =>
                    {
                        string? name = string.IsNullOrWhiteSpace(c.DisplayName) ? $"{(c.GivenName ?? string.Empty).Trim()} {(c.FamilyName ?? string.Empty).Trim()}".Trim() : c.DisplayName.Trim();
                        if(string.IsNullOrWhiteSpace(name)) return null;
                        ;
                        return new ContactItem(name, c.Phones?.FirstOrDefault()?.PhoneNumber ?? string.Empty);
                    })
                    .Where(x => x is not null)!
                    .Cast<ContactItem>()
                    .OrderBy(x => x.DisplayName, StringComparer.OrdinalIgnoreCase);
                }).ConfigureAwait(false);

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Contacts.Clear();
                    foreach(ContactItem i in items) { Contacts.Add(i); }
                });
            }
            catch(Exception)
            {
                IsSupported = false;
                UnsupportedReason = "Nicht verfügbar.";
            }
        #else
            IsSupported = false;
            UnsupportedReason = "Nicht unterstützt.";
        #endif
    }
}
