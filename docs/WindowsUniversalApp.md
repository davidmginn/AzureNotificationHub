# Building a basic notification based app

* Access the [Windows Developer Portal](https://developer.microsoft.com/en-us/windows/windows-apps) **Note there is membership fee to develop windows apps*
* Sign in and then click "dashboard" to see your apps or create a new app

![Alt text](img/dashboard.PNG)

* Click "Create new app"

![Alt text](img/newapp.PNG)

* Give your app a unique name

![Alt text](img/reserveName.PNG)

* Click "Services", followed by "Push Notification"

![Alt text](img/services.PNG)

* Click "Live Services site"

![Alt text](img/liveServices.PNG)

* Make note of the App Secret and Package SID

![Alt text](img/appSecret.PNG)

![Alt text](img/packageSID.PNG)

* Log into the [Azure Portal](https://portal.azure.com)

* Click "New" and search for "Notifcation Hub"

![Alt text](img/newNotificationHub.PNG)

![Alt text](img/newNotificationHubProperties.PNG)

* Once the notification hub has been provisioned, select it from your Azure Portal start page.  Click "Notification Services" -> "Windows (WNS)" and set the Package SID and Security Key (App Secret) that were noted above

![Alt text](img/newNotificationHubSettings.PNG)

* Open Visual Studio 2015, and create a new Windows Universal App

![Alt text](img/newProject.PNG)

* Right click on the project, select "Store" and then "Associate App with the Store".  Proceed through the dialogs to associate the project with the Windows Store App we created earlier

![Alt text](img/associateAppWithStore.PNG)

![Alt text](img/associateAppWithStore2.PNG)

* Add the "WindowsAzure.Messsaging.Managed" nuget package

![Alt text](img/nuget.PNG)

* Open "App.xaml.cs" and add the below using statements

```
using Windows.Networking.PushNotifications;
using Microsoft.WindowsAzure.Messaging;
using Windows.UI.Popups;
```

* Add the below method to "App.xaml.cs"

```
private async void InitNotificationsAsync()
{
    var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

    var hub = new NotificationHub("<hub name>", "<connection string with listen access>");
    var result = await hub.RegisterNativeAsync(channel.Uri);

    // Displays the registration ID so you know it was successful
    if (result.RegistrationId != null)
    {
        var dialog = new MessageDialog("Registration successful: " + result.RegistrationId);
        dialog.Commands.Add(new UICommand("OK"));
        await dialog.ShowAsync();
    }

}
```

* Add the below line to the beginning of the "OnLaunched" method in "App.xaml.cs"

```
InitNotificationsAsync();
```

* Hit "F5" to install and debug your application

* Browse to the [Azure Portal](https://portal.azure.com) and access your notification hub

* Click "Test Send", Select "Windows" as your platform, and "Toast" as your notification type, then click send

![Alt text](img/TestNotification.PNG)

* If your app is installed and running, you should receive a new Push Notification!