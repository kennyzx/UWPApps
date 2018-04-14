using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechRecognition;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPBank
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SpeechSynthesisRecognitionPage : Page
    {
        public SpeechSynthesisRecognitionPage()
        {
            this.InitializeComponent();
            
            this.Loaded += SpeechSynthesisRecognitionPage_Loaded;
        }

        private void SpeechSynthesisRecognitionPage_Loaded(object sender, RoutedEventArgs e)
        {
            //populate combobox with available voices
            cbAvailableVoices.DisplayMemberPath = "DisplayName";
            cbAvailableVoices.ItemsSource = SpeechSynthesizer.AllVoices;
            cbAvailableVoices.SelectedIndex = SpeechSynthesizer.AllVoices.Count > 0 ? 0 : -1;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            StopReading();
        }

        private async void btnSelectTextToRead_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker filePicker = new FileOpenPicker()
            {
                CommitButtonText = "Select File",
                SuggestedStartLocation = PickerLocationId.Desktop,
                ViewMode = PickerViewMode.Thumbnail
            };
            filePicker.FileTypeFilter.Add(".txt");
            var file = await filePicker.PickSingleFileAsync();
            if (file != null)
            {
                string fileContent = await FileIO.ReadTextAsync(file);
                if (fileContent.Length > 10 * 1024)
                    fileContent = fileContent.Substring(10 * 1024); //set upper limit of text length

                tbTextFileContent.Text = fileContent;
                
                await ConvertTextToSpeechAndPlay(fileContent);
            }
        }

        private async Task ConvertTextToSpeechAndPlay(string text)
        {
            StopReading();            

            using (var synth = new SpeechSynthesizer())
            {
                synth.Voice = cbAvailableVoices.SelectedItem as VoiceInformation;
                SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(text);
                ttsMediaElement = new MediaElement();
                ttsMediaElement.MediaEnded += (s, args) =>
                {
                    stream.Dispose(); //TODO: This is not actually hit, potential memory leak?
                };
                ttsMediaElement.SetSource(stream, stream.ContentType);
                ttsMediaElement.Play();
            }
        }

        private void StopReading()
        {
            if (ttsMediaElement != null)
                ttsMediaElement.Stop(); //stop the reading that has not finished, if any
        }

        private async void btnSpeechRecognize_Click(object sender, RoutedEventArgs e)
        {
            string phoneNumber = string.Empty;
            using (SpeechRecognizer recognizer = new SpeechRecognizer())
            {
                recognizer.Constraints.Add(new SpeechRecognitionTopicConstraint
                    (SpeechRecognitionScenario.FormFilling, "Phone"));
                await recognizer.CompileConstraintsAsync();
                recognizer.Timeouts.InitialSilenceTimeout = TimeSpan.FromSeconds(5);
                recognizer.Timeouts.EndSilenceTimeout = TimeSpan.FromSeconds(20);
                try
                {
                    SpeechRecognitionResult result = await recognizer.RecognizeAsync();
                    if (result.Status == SpeechRecognitionResultStatus.Success)
                    {
                        tbPhoneNumber.Text = result.Text;
                    }
                    else
                    {
                        tbPhoneNumber.Text = result.Status.ToString();
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    await new MessageDialog(ex.Message).ShowAsync();
                    //TODO: Redirect user to Settings to grant permission to the app
                }
            }
        }

        MediaElement ttsMediaElement;
    }
}
