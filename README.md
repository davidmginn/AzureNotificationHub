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
is a great platform for building RESTful web services that can easily be consumed by mobile applications.  In our mobile app, we will have a POST method defined that take the nessary parameters to process
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

### Types of Notifications

#### Broadcast

Sends a push notification to all subscribed devices

#### Unicast/Multicast

Sends to a specific user/group of users.  This is done by using a sigle tag within the expressing

```
user_davidmginn
```

#### Segmentation

Sends a push notification to a complex group using a tag expression

```
(location_cincinnati || location_northern_kentucky) && azure
```



