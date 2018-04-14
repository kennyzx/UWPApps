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
using Windows.UI.Notifications.Management;
using Windows.UI.Notifications;
using System.Text;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPBank
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VersionAdaptiveCode : Page
    {
        public VersionAdaptiveCode()
        {
            this.InitializeComponent();

            DetectIfNotificationListenerIsSupport();
            DetectSupportedInputScope();
        }

        private void DetectIfNotificationListenerIsSupport()
        {
            if (!ApiInformation.IsTypePresent("Windows.UI.Notifications.Management.UserNotificationListener"))
            {
                btnListenUserNotification.Content = "UserNotificationListener is not supported.";
                btnListenUserNotification.IsEnabled = false;
            }
        }

#pragma warning disable IDE1006 // Naming Styles
        private async void btnListenUserNotification_Click(object sender, RoutedEventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            var userNotificationListener = UserNotificationListener.Current;
            if (await userNotificationListener.RequestAccessAsync() == UserNotificationListenerAccessStatus.Allowed)
            {
                StringBuilder sbNotifications = new StringBuilder();
                foreach (var notification in await userNotificationListener.GetNotificationsAsync(NotificationKinds.Toast))
                {
                    sbNotifications.AppendLine();
                    sbNotifications.Append(notification.AppInfo.DisplayInfo.DisplayName);
                    sbNotifications.AppendLine();
                    //TODO: display notication descriptions
                    foreach (var binding in notification.Notification.Visual.Bindings)
                    {
                        foreach (var text in binding.GetTextElements())
                        {
                            sbNotifications.AppendLine(text.Text);
                        }
                    }
                }
                tbNotifications.Text = sbNotifications.ToString();
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
