using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.ApplicationModel.ExtendedExecution;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
    public sealed partial class ExtendedExecutionPage : Page
    {
        private ExtendedExecutionSession session;
        private Timer periodicTimer = null;

        public ExtendedExecutionPage()
        {
            this.InitializeComponent();
        }

        private async void doWorkButton_Click(object sender, RoutedEventArgs e)
        {
            // The previous Extended Execution must be closed before a new one can be requested.       
            //ClearSession();

            session = new ExtendedExecutionSession();
            session.Reason = ExtendedExecutionReason.Unspecified;
            session.Revoked += Session_Revoked;
            ExtendedExecutionResult result = await session.RequestExtensionAsync();
            switch (result)
            {
                case ExtendedExecutionResult.Allowed:
                    strExtendedExecutionResult.Text = "ExtendedExecutionResult.Allowed";                    
                    break;
                case ExtendedExecutionResult.Denied:
                    strExtendedExecutionResult.Text = "ExtendedExecutionResult.Denied";
                    break;
            }
            periodicTimer = new Timer(OnTimer, DateTime.Now, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(20));
        }

        private async void OnTimer(object state)
        {
            var startTime = (DateTime)state;
            var runningTime = Math.Round((DateTime.Now - startTime).TotalSeconds, 0);
            using (Windows.Media.SpeechRecognition.SpeechRecognizer recognizer =
                        new Windows.Media.SpeechRecognition.SpeechRecognizer())
            {
                //recognizer.Constraints.Add(new Windows.Media.SpeechRecognition.SpeechRecognitionTopicConstraint
                //    (Windows.Media.SpeechRecognition.SpeechRecognitionScenario.FormFilling, "Phone"));
                await recognizer.CompileConstraintsAsync();
                recognizer.Timeouts.InitialSilenceTimeout = TimeSpan.FromSeconds(5);
                recognizer.Timeouts.EndSilenceTimeout = TimeSpan.FromSeconds(20);

                Windows.Media.SpeechRecognition.SpeechRecognitionResult aresult = await recognizer.RecognizeAsync();
                if (aresult.Status == Windows.Media.SpeechRecognition.SpeechRecognitionResultStatus.Success)
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        ExtendedExecutionSessionStatus.Text += aresult.Text + Environment.NewLine;
                    });
                }
            }
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ExtendedExecutionSessionStatus.Text += $"Extended execution has been active for {runningTime} seconds" + Environment.NewLine;
            });
        }

        private void ClearSession()
        {
            if (session != null)
            {
                session.Revoked -= Session_Revoked;
                session.Dispose();
                session = null;
            }
            if (periodicTimer != null)
            {
                periodicTimer.Dispose();
                periodicTimer = null;
            }
        }

        private void Session_Revoked(object sender, ExtendedExecutionRevokedEventArgs args)
        {
            ClearSession();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ClearSession();
        }
    }
}
