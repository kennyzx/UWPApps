using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Storage.Streams;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPBank
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
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

                var mapIcon = new MapIcon();
                mapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/mapIcon.png"));
                mapIcon.Location = location.Coordinate.Point;
                mapIcon.Title = "You are here";
                branchesMap.MapElements.Add(mapIcon);
            }            
        }

        private void GoHomeAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }
    }
}
