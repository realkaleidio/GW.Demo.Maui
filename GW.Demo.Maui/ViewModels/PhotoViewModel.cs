namespace GW.Demo.Maui.ViewModels;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Media;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;



public partial class PhotoViewModel: ObservableObject
{
    public const string PhotosFolderName = "photos";

    private ImageSource? _PhotoSource;
    private string? _PhotoPath;
    private bool _IsBusy;


    public PhotoViewModel()
    {
        TakePhotoCommand = new Command(async () => await _TakePhotoAsync(), () => !IsBusy);
    }


    public Command TakePhotoCommand { get; }


    public ImageSource? PhotoSource
    {
        get { return _PhotoSource; }
        private set { SetProperty(ref _PhotoSource, value); }
    }

    
    public string? PhotoPath
    {
        get { return _PhotoPath; }
        private set { SetProperty(ref _PhotoPath, value); }
    }

    
    public bool IsBusy
    {
        get { return _IsBusy; }
        set
        {
            if(SetProperty(ref _IsBusy, value)) { TakePhotoCommand.ChangeCanExecute(); }
        }
    }


    public bool HasPhoto
    {
        get { return !string.IsNullOrWhiteSpace(_PhotoPath) && File.Exists(_PhotoPath); }
    }
    

    public string GetPhotosFolderPath()
    {
        string baseDir = FileSystem.AppDataDirectory;
        string photosDir = Path.Combine(baseDir, PhotosFolderName);
        return photosDir;
    }


    public void Load()
    {
        if (IsBusy) { return; }
        IsBusy = true;

        try
        {
            string dir = GetPhotosFolderPath();
            if(!Directory.Exists(dir))
            {
                PhotoPath = null;
                PhotoSource = null;
                OnPropertyChanged(nameof(HasPhoto));
                return;
            }

            IEnumerable<string> files = Directory.EnumerateFiles(dir).Where(p => 
            { 
                switch(Path.GetExtension(p).ToLowerInvariant())
                {
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                    case ".heic":
                    case ".bmp":
                    case ".gif": return true;
                }

                return false;
            });

            string? last = files.Select(p => new FileInfo(p)).OrderByDescending(fi => fi.LastWriteTimeUtc).Select(fi => fi.FullName).FirstOrDefault();
            if(string.IsNullOrWhiteSpace(last))
            {
                PhotoPath = null;
                PhotoSource = null;
                OnPropertyChanged(nameof(HasPhoto));
                return;
            }

            PhotoPath = last;
            PhotoSource = ImageSource.FromFile(last);
            OnPropertyChanged(nameof(HasPhoto));
        }
        finally
        {
            IsBusy = false;
        }
    }


    private async Task _TakePhotoAsync()
{
    if (IsBusy) return;
    IsBusy = true;

    try
    {
        if(!MediaPicker.Default.IsCaptureSupported) return;

        var result = await MediaPicker.Default.CapturePhotoAsync();
        if (result is null) return;

        _EnsurePhotoDir();

        var ext = Path.GetExtension(result.FileName);
        if (string.IsNullOrWhiteSpace(ext)) ext = ".jpg";
        var destPath = Path.Combine(GetPhotosFolderPath(), $"photo_{DateTime.UtcNow:yyyyMMdd_HHmmssfff}{ext}");

        using (var src = await result.OpenReadAsync())
        using (var dst = File.Open(destPath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            await src.CopyToAsync(dst);
            await dst.FlushAsync();
        }

        PhotoPath = destPath;
        PhotoSource = ImageSource.FromFile(destPath);
        OnPropertyChanged(nameof(HasPhoto));
    }
    finally
    {
        IsBusy = false;
    }
}

    private void _EnsurePhotoDir()
    {
        string dir = GetPhotosFolderPath();
        if(!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
    }
}
