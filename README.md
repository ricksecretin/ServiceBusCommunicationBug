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