using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Storage.Streams;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPBank
{
    /// <summary>
    /// MapControl: Request a maps authentication key
    /// https://docs.microsoft.com/en-us/windows/uwp/maps-and-location/authentication-key
    /// </summary>
    public sealed partial class MapControlPage : Page
    {
        public MapControlPage()
        {
            this.InitializeComponent();
            this.Loaded += MapControlPage_Loaded;
            this.branchesMap.ZoomLevelChanged += BranchesMap_ZoomLevelChanged;
        }

        private void BranchesMap_ZoomLevelChanged(Windows.UI.Xaml.Controls.Maps.MapControl sender, object args)
        {
            System.Diagnostics.Debug.WriteLine($"ZoomLevel: {branchesMap.ZoomLevel}");
        }

        private async void MapControlPage_Loaded(object sender, RoutedEventArgs e)
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            if (accessStatus == GeolocationAccessStatus.Allowed)
            {
                Geolocator locator = new Geolocator()
                {
                    DesiredAccuracyInMeters = 20
                };
                var location = await locator.GetGeopositionAsync();
                branchesMap.Center = location.Coordinate.Point;

                var mapIcon = new MapIcon()
                {
                    Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/mapIcon.png")),
                    Location = location.Coordinate.Point,
                    Title = "You are here"
                };
                branchesMap.MapElements.Add(mapIcon);
            }            
        }
    }
}
