namespace GW.Demo.Maui.ViewModels;

using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Devices.Sensors;



public class PositionViewModel: INotifyPropertyChanged
{
    private readonly IGeolocation _GeoLocation;
    private string _CoordinatesText = "–";


    public event PropertyChangedEventHandler? PropertyChanged;


    public PositionViewModel()
    {
        _GeoLocation = Geolocation.Default;
    }


    public string CoordinateText
    {
        get { return _CoordinatesText; }
        set
        {
            if(_CoordinatesText != value)
            {
                _CoordinatesText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CoordinateText)));
            }
        }
    }


    public async Task RefreshAsync()
    {
        try
        {
            GeolocationRequest rq = new GeolocationRequest(GeolocationAccuracy.Default, TimeSpan.FromSeconds(10));
            using CancellationTokenSource cs = new CancellationTokenSource(TimeSpan.FromSeconds(15));            
            Location? loc = await _GeoLocation.GetLocationAsync(rq, cs.Token);

            if(loc != null)
            {
                CoordinateText = _FormatLocation(loc.Latitude, loc.Longitude);
            }
            else { CoordinateText = "Unbekannt."; }
        }
        catch(Exception)
        {
            CoordinateText = "Service nicht verfügbar.";
        }
    }


    private static string _FormatLocation(double lat, double lon)
    {
        string ns = lat >= 0 ? "N" : "S";
        string ow = lon >= 0 ? "O" : "W";
        
        return $"{Math.Abs(lat):0.00000}° {ns}\n{Math.Abs(lon):0.00000}° {ow}";
    }
}
