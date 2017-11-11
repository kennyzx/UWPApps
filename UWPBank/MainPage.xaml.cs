using Windows.UI.Xaml.Controls;

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

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ulong usageLimit = Windows.System.MemoryManager.AppMemoryUsageLimit;
            ulong currentUsage = Windows.System.MemoryManager.AppMemoryUsage;
            var report = Windows.System.MemoryManager.GetAppMemoryReport();
            strMemoryAPIs.Text += System.Environment.NewLine + 
                string.Format("UsageLimit: {0:X}/{1:X}", currentUsage, usageLimit)
                + System.Environment.NewLine + $"Report: {report.TotalCommitUsage}/{report.TotalCommitLimit}";
        }

        void BrightnessOverride()
        {
            //Windows.Graphics.Display.BrightnessOverride bo = Windows.Graphics.Display.BrightnessOverride.GetDefaultForSystem();
            //System.Diagnostics.Debug.WriteLine("BrightnessLevel:" + bo.BrightnessLevel);
            //if (bo.IsSupported)
            //{
            //    bo.SetBrightnessScenario(Windows.Graphics.Display.DisplayBrightnessScenario.IdleBrightness,
            //        Windows.Graphics.Display.DisplayBrightnessOverrideOptions.None);
            //}
        }
    }
}
