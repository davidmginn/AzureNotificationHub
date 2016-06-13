using Microsoft.WindowsAzure.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.PushNotifications;

namespace CincyAzureNotificationHub.Services
{
    public class NotificationService
    {
        public static async Task<string> InitNotificationsAsync()
        {
            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            var hub = new NotificationHub("CincyAzure", "Endpoint=sb://cincyazure.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=w8KPh8QPYeCsteaHiUw85M9FZhRcpEgbq5DELUJeAQw=");
            var result = await hub.RegisterNativeAsync(channel.Uri);

            var registration = new Registration(channel.Uri);

            return result.RegistrationId;
        }
    }
}
