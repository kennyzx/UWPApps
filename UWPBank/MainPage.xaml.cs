using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPBank
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Desktop.LaunchDesktopProcessPage));
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            throw new Exception("This is an unhandled exception and will be reported to HockeyApp");
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AdvertisementPage));
        }

        private void FullScreenModeToggleButton_Click(object sender, RoutedEventArgs e)
        {
            var view = ApplicationView.GetForCurrentView();
            if (view.IsFullScreenMode)
            {
                view.ExitFullScreenMode();
            }
            else
            {
                if (view.TryEnterFullScreenMode())
                {
                    
                }
                else
                {
                    
                }
            }
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AppServiceConsumerPage));
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MapControlPage));
        }
    }
}
