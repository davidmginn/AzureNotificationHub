# Leveraging Azure to build notification based apps

## Building a simple notification app in 15 minutes!

[Simple Notification App](docs/WindowsUniversalApp.md)

## Windows Universal App

In order to demonstrate push notifications, we need an application capable of subscribing to and receiving push notifications.  Windows Universal Applications are designed to be run on any Windows 10 device and 
are perfect for just this.  Our Windows Universal Application will register itself to receive push notifications on startup with the below tags.  

* user_davidmginn
* location_cincinnati
* location_northern_kentucky
* subject_azure

The sample application is a reporting app that allows the user to select from a list of reports, input parameters and submit the report for processing.  The parameters will be passed to the Web API Component.  

## Web API

Mobile appliations need a way to consume data.  Typically, this data needs to come from a web service, as only having locally available data isn't going to make an application very valuable.  ASP.Net Web API 
is a great platform for building RESTful web services that can easily be consumed by mobile applications.  In our mobile app, we will have a POST method defined that take the necessary parameters to process
a report and send a message containing the parameters to an Azure Service Bus Queue.

## Service Bus

Azure Service Bus is the back-end component of our sample architecture.  It allows for serialzied object to be placed in a queue, and then picked up from the queue and processed by another component.

## Web Job

Web Jobs are always running, scheduled or on-demand task hosted in Azure.  Web Jobs SDK makes it easy to process messages from the Service Bus.  The web job will be in always running mode, and 
call a function when a new message appears on the Service Bus Queue.  The message is then processed and our report is aggregated.  Upon successful completion of the report, a push notification will be sent
to the user that requested the report.  

## Azure Notification Hub

Abstraction over top of individual platform push notification services.  Allows you to easily send push notifications to devices on most major platforms without writing platform specific code.

* iOS
* Android
* Windows
* Kindle
* Google Chrome

### X-Plat Notifications using templates

Each Platform Notification Service has its own specific format that messages are expected in. 

```
Apple Push Notificaiton Format

{"aps": {"alert" : "Hello!" }}
```

```
Windows Push Notification Formation

<toast>
  <visual>
    <binding template=\"ToastText01\">
      <text id=\"1\">Hello!</text>
    </binding>
  </visual>
</toast>
```

Typically, we'd need to send the push notificaition in the format expected by the specific device.  You can use a template registration however in order for the device to register an expected format with the notification
hub, which then allows us to send a platform independent notification.

```
iOS Template

{"aps": {"alert": "$(message)"}}
```

```
Windows Template

<toast>
    <visual>
        <binding template=\"ToastText01\">
            <text id=\"1\">$(message)</text>
        </binding>
    </visual>
</toast>
```

From here, sending a cross platform notification from .NET is as easy as sending a list of key/value pairs containing the variables that satisfy the registered templates

```
await hub.SendTemplateNotificationAsync(new Dictionary<string, string>()
    {
        {
            "message", "The report you requested has been processed and is now available for viewing!"
        }
    });
```

### Types of Notifications

#### Broadcast

Sends a push notification to all subscribed devices

#### Unicast/Multicast

Sends to a specific user/group of users.  This is done by using a single tag within the expression

```
user_davidmginn
```

#### Segmentation

Sends a push notification to a complex group using a tag expression

```
(location_cincinnati || location_northern_kentucky) && azure
```



