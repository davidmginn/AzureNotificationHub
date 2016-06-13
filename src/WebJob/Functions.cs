using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.NotificationHubs;
using WebAPI.Models;
using Newtonsoft.Json;

namespace WebJob
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([ServiceBusTrigger("TestQueue")] SimpleModel model, TextWriter log)
        {
            log.WriteLine(model.Title);

            SendNotificationAsync(model).Wait();
        }

        static async Task SendNotificationAsync(SimpleModel model)
        {
            NotificationHubClient hub = NotificationHubClient
                .CreateClientFromConnectionString("Endpoint=sb://cincyazure.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=HDCgcWXf9CLsdmgvnNzfSqy66ZRLC++E07yzG1BRxkg=", "CincyAzure");

            var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">The report you requested has been processed and is now available for viewing!</text></binding></visual></toast>";
            await hub.SendWindowsNativeNotificationAsync(toast, "Azure");
        }
    }
}
