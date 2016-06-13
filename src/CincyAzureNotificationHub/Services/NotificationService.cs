using Microsoft.WindowsAzure.Messaging;
using NotificationsExtensions.Toasts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Networking.PushNotifications;
using Windows.UI.Notifications;

namespace CincyAzureNotificationHub.Services
{
    public class NotificationService
    {
        PushNotificationChannel channel;

        public async Task<string> InitNotificationsAsync()
        {
            this.channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            channel.PushNotificationReceived += this.OnPushNotification;

            var hub = new NotificationHub("CincyAzure", "Endpoint=sb://cincyazure.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=w8KPh8QPYeCsteaHiUw85M9FZhRcpEgbq5DELUJeAQw=");
            var result = await hub.RegisterNativeAsync(channel.Uri, new List<string>()
            {
                "Kentucky",
                "Azure"
            });

            return result.RegistrationId;
        }

        private void OnPushNotification(PushNotificationChannel sender, PushNotificationReceivedEventArgs e)
        {
            var text = e.ToastNotification.Content;

            ToastContent content = new ToastContent()
            {
                Launch = "Azure Notification Hub",

                Visual = new ToastVisual()
                {
                    TitleText = new ToastText()
                    {
                        Text = text.InnerText
                    },

                    BodyTextLine1 = new ToastText()
                    {
                        Text = "NotificationsExtensions is great!"  //Lets get some better body text here based on what we decide to do
                    },

                    AppLogoOverride = new ToastAppLogo()
                    {
                        Crop = ToastImageCrop.Circle,
                        Source = new ToastImageSource("http://messageme.com/lei/profile.jpg")
                    }
                },
                Audio = new ToastAudio()
                {
                    Src = new Uri("ms-winsoundevent:Notification.IM")
                }
            };

            XmlDocument doc = content.GetXml();

            // Generate WinRT notification
            var toast = new ToastNotification(doc);

            // Display toast
            ToastNotificationManager.CreateToastNotifier().Show(toast);

            e.Cancel = true;
        }
    }
}
