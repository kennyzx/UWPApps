using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Windows.UI.Core.CoreWindow.GetForCurrentThread().KeyDown += AppServiceConsumerPage_KeyDown;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            Windows.UI.Core.CoreWindow.GetForCurrentThread().KeyDown -= AppServiceConsumerPage_KeyDown;
        }

        private void AppServiceConsumerPage_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (args.VirtualKey == Windows.System.VirtualKey.Enter)
            {
                //Q: How to call an async RelayCommand?
                (this.DataContext as ViewModel.UWPBankViewModel)?.ConsumeAppServiceCommand.Execute(null);
            }
        }
    }
}
