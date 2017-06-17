using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPBank.Desktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LaunchDesktopProcessPage : Page
    {
        public LaunchDesktopProcessPage()
        {
            this.InitializeComponent();
        }
        
        private async void BtnLaunchDesktopProcess_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Full​Trust​Process​Launcher.LaunchFullTrustProcessForAppAsync("App");
            }
            catch (System.UnauthorizedAccessException)
            {
                //if the runFullTrust capability is not declared in manifest
                await new MessageDialog("Acess Denied: runFullTrust capability is not declared in manifest").ShowAsync();
            }
        }        

        private void GoHomeAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
                this.Frame.GoBack();
        }
    }
}
