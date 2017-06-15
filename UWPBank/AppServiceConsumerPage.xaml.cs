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
using Windows.ApplicationModel.AppService;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPBank
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppServiceConsumerPage : Page
    {
        public AppServiceConsumerPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (inventoryService == null)
            {
                inventoryService = new AppServiceConnection();
                inventoryService.AppServiceName = "InProcessAppService";
                inventoryService.PackageFamilyName = "UWPBank.CommonAppService_m40mq4mvr89fy";

                var status = await inventoryService.OpenAsync();
                if (status != AppServiceConnectionStatus.Success)
                {
                    tb.Text = "Failed to connect.";
                    return;
                }
            }

            //call the service
            var message = new ValueSet();
            message.Add("Request", "CAPITALIZE");
            message.Add("Value", tb.Text);
            AppServiceResponse response = await inventoryService.SendMessageAsync(message);
            string result = "";
            if (response.Status == AppServiceResponseStatus.Success)
            {
                result += response.Message["Response"] as string;               
            }

            tb.Text = result;
        }

        private AppServiceConnection inventoryService;

        private void GoHomeAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
                this.Frame.GoBack();
        }
    }
}
