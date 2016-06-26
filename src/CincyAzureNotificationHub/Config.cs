using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CincyAzureNotificationHub
{
    public class Config
    {
        //public const string API_BASE_URI = "http://localhost:61931/api";
        public const string API_BASE_URI = "http://cincyazurenotificationhub.azurewebsites.net/api";
        public const string NOTIFICATION_HUB_PATH = "CincyAzure";
        public const string NOTIFICATION_HUB_CONNECTION_STRING = "Endpoint=sb://cincyazure.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=w8KPh8QPYeCsteaHiUw85M9FZhRcpEgbq5DELUJeAQw=";
    }
}
