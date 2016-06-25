using Microsoft.Azure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class CustomerProductReportController : ApiController
    {
        // GET: api/CustomerProductReport/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CustomerProductReport
        public void Post(CustomerProductReport report)
        {
            // Create the queue if it does not exist already.
            string connectionString =
                CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");

            var namespaceManager =
                NamespaceManager.CreateFromConnectionString(connectionString);

            if (!namespaceManager.QueueExists("CustomerProductReport"))
            {
                namespaceManager.CreateQueue("CustomerProductReport");
            }

            QueueClient Client =
                QueueClient.CreateFromConnectionString(connectionString, "CustomerProductReport");

            Client.Send(new BrokeredMessage(report));
        }

        // PUT: api/CustomerProductReport/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CustomerProductReport/5
        public void Delete(int id)
        {
        }
    }
}
