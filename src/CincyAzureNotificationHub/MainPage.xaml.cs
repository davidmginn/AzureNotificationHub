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
        public enum VisualStates
        {
            Desktop_Selecting, Desktop_DataEntry, Phone_Selecting, Phone_DataEntry
        }

        private VisualStates _VisualState;
        public VisualStates VisualState
        {
            get
            {
                return _VisualState;
            }
            set
            {
                _VisualState = value;
                switch (value)
                {
                    case VisualStates.Desktop_Selecting:
                        VisualStateManager.GoToState(this, "DesktopState", false);
                        DataEntryPanel.Visibility = Visibility.Collapsed;
                        break;
                    case VisualStates.Desktop_DataEntry:
                        VisualStateManager.GoToState(this, "DesktopState", false);
                        DataEntryPanel.Visibility = Visibility.Visible;
                        break;
                    case VisualStates.Phone_Selecting:
                        DataEntryPanel.Visibility = Visibility.Collapsed;
                        VisualStateManager.GoToState(this, "PhoneState_Selecting", false);
                        break;
                    case VisualStates.Phone_DataEntry:
                        VisualStateManager.GoToState(this, "PhoneState_DataEntry", false);
                        DataEntryPanel.Visibility = Visibility.Visible;
                        break;
                    default:
                        VisualStateManager.GoToState(this, "DesktopState", false);
                        break;
                }
            }
        }

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
