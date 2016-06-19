# Leveraging Azure to build notification based apps

## Windows Universal App

Our application for this scenario

## Web API

Mobile apps need to consome data.  

## Service Bus

Back-end piece of a distributed architecture. 

## Web Job

Scheduled or on-demand task hosted in Azure.  Web Jobs SDK makes it easy to process
messages from the Service Bus.

## Azure Notification Hub

Abstraction over top of individual platform push notification services.  Allows you to 
easily send push notifications to devices on most major platforms without writing platform specific
code.

* iOS
* Android
* Windows
* Kindle
* Google Chrome

### Types of Notifications

#### Broadcast

Sends a push notification to all subscribed devices

#### Unicast/Multicast

Sends to a specific user/group of users

#### Segmentation

Sends a push notification to a complex group using a tag expression

```
(location_cincinnati || location_northern_kentucky) && azure
```

## Simple Notification App

* Register a new Windows Universal App
* Enable Notifications
* Provision an azure notification hub
* Write the codez
* Debug App
* Send push notification from notification hub



