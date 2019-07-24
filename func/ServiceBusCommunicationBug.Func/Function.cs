using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ServiceBusCommunicationBug.Func
{
    public static class Function
    {
        [FunctionName(nameof(Function))]
        public static async Task RunAsync([EventHubTrigger("%EventHubName%", Connection = "EventHubConnectionString")]
            EventData eventData,
            ILogger log)
        {
            var connectionString = GetSetting("ServiceBusConnectionString");
            const string topicName = "test-topic";
            
            await SetupServiceBus(connectionString, topicName);
            await SendNotification(connectionString, topicName);
        }

        private static async Task SendNotification(string connectionString, string topicName)
        {
            var topicClient = new TopicClient(connectionString, topicName);

            var message = new Message
            {
                ContentType = "application/json",
                Body = Encoding.UTF8.GetBytes("")
            };
            await topicClient.SendAsync(message);
            await topicClient.CloseAsync();
        }

        private static async Task SetupServiceBus(string connectionString, string topicName)
        {
            const string subscriptionName = "test-subscription";

            var managementClient = new ManagementClient(connectionString);
            if (!await managementClient.TopicExistsAsync(topicName))
                await managementClient.CreateTopicAsync(topicName);

            if (!await managementClient.SubscriptionExistsAsync(topicName, subscriptionName))
                await managementClient.CreateSubscriptionAsync(
                    new SubscriptionDescription(topicName, subscriptionName));
        }

        private static string GetSetting(string name) => System.Environment.GetEnvironmentVariable(name);
    }
}