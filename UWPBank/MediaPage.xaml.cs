using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPBank
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MediaPage : Page
    {
        MediaCapture mediaCapture;
        bool isInitializedSuccessfully;
        
        public MediaPage()
        {
            this.InitializeComponent();
        }

        private async Task CreateOrReInitCamera()
        {
            if (mediaCapture != null)
                mediaCapture.Dispose();

            isInitializedSuccessfully = false;
            mediaCapture = new MediaCapture();
            try
            {
                await mediaCapture.InitializeAsync();
                previewElement.Source = mediaCapture;
                await mediaCapture.StartPreviewAsync();
                isInitializedSuccessfully = true;
            }
            catch (UnauthorizedAccessException ex)
            {
                await new MessageDialog(ex.Message, "Cannot use camera").ShowAsync();
                //TODO: Redirect user to Settings to grant permission to the app
            }
        }

        private async Task DisposeCamera()
        {
            if (isInitializedSuccessfully)
                await mediaCapture.StopPreviewAsync();

            mediaCapture.Dispose();
            mediaCapture = null;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await CreateOrReInitCamera();
            Window.Current.Activated += Current_Activated;
        }

        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            await DisposeCamera();
            Window.Current.Activated -= Current_Activated;
        }

        private async void Current_Activated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            await CreateOrReInitCamera();
        }

        private async void btnTakePicture_Click(object sender, RoutedEventArgs e)
        {
            if (!isInitializedSuccessfully)            
                return;

            var captureStream = new InMemoryRandomAccessStream();
            {
                await mediaCapture.CapturePhotoToStreamAsync(
                    Windows.Media.MediaProperties.ImageEncodingProperties.CreateJpeg(),
                     captureStream);
                
                BitmapImage bitmap = new BitmapImage();
                captureStream.Seek(0);
                bitmap.SetSource(captureStream);
                previewImage.Source = bitmap;
            }
        }
    }
}
