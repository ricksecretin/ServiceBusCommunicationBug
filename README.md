# ServiceBusCommunicationBug

To run this sample:

### Azure:

Create the following resources:
- service bus namespace
- event hub namespace
- function app

### Func:

Replace in local.settings.json the following with values from azure:
- EventHubConnectionString
- EventHubName
- ServiceBusConnectionString

Upload the function app to azure.

### Xamarin

Replace in config.cs in servicebus.communication.core with values from azure:
- Keyname
- SharedAccessKey
- Url

### How to reproduce the issue

1. Use mobile device as hotspot
2. Connect other device to that wifi
3. Force mobile device to switch between network modes (4g -> 3g, 3g -> 2g, 2g -> 4g,...)
4 The other device will no longer receive messages

In the application itself there are 2 buttons. The bottom button sends a message to the Eventhub which responds with a notification through the service bus.
In a normal situation you will see "Message sent" followed by "Notification Received".

Once perform the steps mentioned above no more notifications are received, while the message is successfully sent.

The top button can be used to reset the connection (the subscription client is closed and a new one is generated) At this point all notifications that were previously sent to the device are received.
Thus resetting the connection returns the application to a normal state.

https://github.com/Azure/azure-sdk-for-net/issues/6252