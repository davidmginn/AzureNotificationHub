using CincyAzureNotificationHub.Model;
using CincyAzureNotificationHub.Services;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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

        public MainPage()
        {
            this.InitializeComponent();

            // Set up visual states to handle window resizing for different devices
            SetUpVisualStates();

            // Register back button input
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;

            // Hide the status bar so that the black bar at the top does not appear
            HideStatusBar();
        }

        /// <summary>
        /// Hides the status bar at the top of the page to add more room
        /// </summary>
        private async void HideStatusBar()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                await statusbar.HideAsync();
            }

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

        /// <summary>
        /// Handles when a report button is pressed
        /// </summary>
        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Set visual state
            if (VisualState == VisualStates.Phone_Selecting)
            {
                VisualState = VisualStates.Phone_DataEntry;
            }
            else if (VisualState == VisualStates.Desktop_Selecting)
            {
                VisualState = VisualStates.Desktop_DataEntry;
            }

            // Assign document title
            string name = ((Grid)sender).Name;
            name = name.Remove(0, 6);
            DocumentTitle.Text = "Test Report " + name;

            // Clear old entries
            CustomerName.Text = "";
            ProductName.Text = "";
            MonthToDate.IsChecked = false;
            QuarterToDate.IsChecked = false;
            YearToDate.IsChecked = false;
        }

        /// <summary>
        /// Handles when the submit button is pressed.
        /// </summary>
        private async void SubmitButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SendAzureRequest();

            // Show notification 
            MessageDialog msg = new MessageDialog("Your report has been sent to Azure and is running. You will recieve a notification when it is complete.", "Report Sent");
            await msg.ShowAsync();

            // Reset state
            if (VisualState == VisualStates.Phone_DataEntry)
            {
                VisualState = VisualStates.Phone_Selecting;
            }
        }

        private async void SendAzureRequest()
        {
            var report = new CustomerProductReport()
            {
                CustomerName = CustomerName.Text,
                ProductName = ProductName.Text,
                RequestedBy = "user_davidmginn"
            };

            if (YearToDate.IsChecked.Value)
            {
                report.TimePeriod = "YearToDate";
            }
            else if (QuarterToDate.IsChecked.Value)
            {
                report.TimePeriod = "QuarterToDate";
            }
            else if (MonthToDate.IsChecked.Value)
            {
                report.TimePeriod = "MonthToDate";
            }

            using (var client = new HttpClient())
            {
                var postData = JsonConvert.SerializeObject(report);

                await client.PostAsync($"{Config.API_BASE_URI}/CustomerProductReport/", new StringContent(postData, Encoding.UTF8, "application/json"));
            }
        }
    }
}
