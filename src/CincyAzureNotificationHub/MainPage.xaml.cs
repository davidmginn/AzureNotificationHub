using CincyAzureNotificationHub.Model;
using CincyAzureNotificationHub.Services;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CincyAzureNotificationHub
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    { 
        NotificationService service;

        public MainPage()
        {
            this.InitializeComponent();
            this.service = new NotificationService();


        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            await service.InitNotificationsAsync();

            using (var client = new HttpClient())
            {
                var postData = JsonConvert.SerializeObject(new SimpleModel()
                {
                    Title = "Hello World"
                });

                await client.PostAsync("http://localhost:61931/api/values/", new StringContent(postData, Encoding.UTF8, "application/json"));
            }
        }
    }
}
