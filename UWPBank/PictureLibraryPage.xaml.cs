using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPBank.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPBank
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PictureLibraryPage : Page
    {
        public PictureLibraryPage()
        {
            this.InitializeComponent();
            this.Loaded += PictureLibraryPage_Loaded;
        }

        private void PictureLibraryPage_Loaded(object sender, RoutedEventArgs e)
        {
            (this.DataContext as UWPBankViewModel)?.LibraryViewModelInstance.UpdateImages();
        }
    }
}
