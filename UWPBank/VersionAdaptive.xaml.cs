using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
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
    public sealed partial class VersionAdaptive : Page
    {
        public VersionAdaptive()
        {
            this.InitializeComponent();
            DetectSupportedInputScope();
            DetectNotificationListenerIsSupport();
        }

        private void DetectNotificationListenerIsSupport()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.Notifications.Management.UserNotificationListener"))
            {
                // Listener supported!
            }
        }

        private void DetectSupportedInputScope()
        {
            InputScope scope = new InputScope();
            InputScopeName scopeName = new InputScopeName();

            // Check that the ChatWithEmoji value is present.
            // (It's present starting with Windows 10, version 1607,
            //  the Target version for the app. This check returns false on earlier versions.)         
            if (ApiInformation.IsEnumNamedValuePresent("Windows.UI.Xaml.Input.InputScopeNameValue", "ChatWithoutEmoji"))
            {
                // Set new ChatWithoutEmoji InputScope if present.
                scopeName.NameValue = InputScopeNameValue.ChatWithoutEmoji;
            }
            else
            {
                // Fall back to Chat InputScope.
                scopeName.NameValue = InputScopeNameValue.Chat;
            }

            // Set InputScope on messaging TextBox.
            scope.Names.Add(scopeName);
            chatBox.InputScope = scope;

            // For this example, set the TextBox text to show the selected InputScope.
            chatBox.Text = chatBox.InputScope.Names[0].NameValue.ToString();
        }
    }
}
