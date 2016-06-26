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
using Models;
using System.Threading;
using System.Configuration;

namespace WebJob
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([ServiceBusTrigger("CustomerProductReport")] CustomerProductReport model, TextWriter log)
        {
            Console.WriteLine(model.CustomerName);
            Console.WriteLine(model.ProductName);
            Console.WriteLine(model.TimePeriod);
            Console.WriteLine(model.RequestedBy);


            Thread.Sleep(10000);

            Console.WriteLine("Sending Notification...");

            SendNotificationAsync(model).Wait();

            Console.WriteLine("Notification Sent!");
        }

        static async Task SendNotificationAsync(CustomerProductReport model)
        {
            NotificationHubClient hub = NotificationHubClient
                .CreateClientFromConnectionString(ConfigurationManager.AppSettings["NotificationHubConnectionString"], ConfigurationManager.AppSettings["NotificationHubPath"]);

            var outcome = await hub.SendTemplateNotificationAsync(new Dictionary<string, string>()
            {
                {
                    "message", "The report you requested has been processed and is now available for viewing!"
                }
            }, 
            model.RequestedBy);
        }
    }
}
