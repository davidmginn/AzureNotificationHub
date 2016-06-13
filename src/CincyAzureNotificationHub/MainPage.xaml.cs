using CincyAzureNotificationHub.Model;
using CincyAzureNotificationHub.Services;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Windows.UI.Core;
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

            // Set up visual states to handle window resizing for different devices
            SetUpVisualStates();

            // Register back button input
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
        }

        /// <summary>
        /// Handles back button VisualState navigation
        /// </summary>
        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            // Navigate "back" if you are a phone user
            if (VisualState == VisualStates.Phone_DataEntry)
            {
                VisualState = VisualStates.Phone_Selecting;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Sets inital visualState and handles window size change events
        /// </summary>
        private void SetUpVisualStates()
        {
            // Set inital visual state
            if (Window.Current.Bounds.Width > 720)
            {
                VisualState = VisualStates.Desktop_Selecting;
            }
            else
            {
                VisualState = VisualStates.Phone_Selecting;
            }

            // Handle window size changes
            Window.Current.SizeChanged += (s1, e1) =>
            {
                if (Window.Current.Bounds.Width >= 720)
                {
                    // Window should be in desktop visual state

                    // Convert from phone state if needed 
                    if (VisualState == VisualStates.Phone_DataEntry)
                    {
                        VisualState = VisualStates.Desktop_DataEntry;
                    }
                    else if (VisualState == VisualStates.Phone_Selecting)
                    {
                        VisualState = VisualStates.Desktop_Selecting;
                    }
                }
                else
                {
                    // Window should be in a "phone" visual state

                    // Convert from desktop state to correct view if needed 
                    if (VisualState == VisualStates.Desktop_DataEntry)
                    {
                        VisualState = VisualStates.Phone_DataEntry;
                    }
                    else if (VisualState == VisualStates.Desktop_Selecting)
                    {
                        VisualState = VisualStates.Phone_Selecting;
                    }
                }
            };
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
