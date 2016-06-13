using System.Collections.Generic;
using System.Web.Http;
using Microsoft.ServiceBus;
using Microsoft.Azure;
using Microsoft.ServiceBus.Messaging;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post(SimpleModel model)
        {
            // Create the queue if it does not exist already.
            string connectionString =
                CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");

            var namespaceManager =
                NamespaceManager.CreateFromConnectionString(connectionString);

            if (!namespaceManager.QueueExists("TestQueue"))
            {
                namespaceManager.CreateQueue("TestQueue");
            }

            QueueClient Client =
                QueueClient.CreateFromConnectionString(connectionString, "TestQueue");

            Client.Send(new BrokeredMessage(model));
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
