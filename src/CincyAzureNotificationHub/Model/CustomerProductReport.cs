using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CincyAzureNotificationHub.Model
{
    public class CustomerProductReport
    { 
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public string TimePeriod { get; set; }
        public string RequestedBy { get; set; }
    }
}
