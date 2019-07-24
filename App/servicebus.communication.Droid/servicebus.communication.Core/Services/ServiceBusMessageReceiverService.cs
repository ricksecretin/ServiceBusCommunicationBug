using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Primitives;
using MvvmCross.Plugin.Messenger;

namespace servicebus.communication.Core
{
    [LazySingletonService]
    public class ServiceBusMessageReceiverService : IServiceBusMessageReceiverService
    {
        private const int MAX_CONCURRENT_CALLS = 10;

        private const string TopicPath = "test-topic";
        private const string SubscriptionName = "test-subscription";

        private readonly IMvxMessenger _messenger;
        private readonly RetryExponential _retryPolicy;

        private readonly SemaphoreSlim _semaphoreStartStop = new SemaphoreSlim(1, 1);

        private SubscriptionClient _subscriptionClient;
        private string _sasToken;

        public ServiceBusMessageReceiverService(IMvxMessenger messenger)
        {
            _messenger = messenger;

            var minimumBackoff = TimeSpan.FromSeconds(0);
            var maximumBackoff = TimeSpan.FromSeconds(5);
            var maxRetryCount = 3;

            _retryPolicy = new RetryExponential(minimumBackoff, maximumBackoff, maxRetryCount);
        }
        
        public async Task StartListening()
        {
            try
            {
                await _semaphoreStartStop.WaitAsync();
                await StartListeningImpl();
            }
            finally
            {
                _semaphoreStartStop.Release();
            }
        }

        public async Task StopListening()
        {
            try
            {
                await _semaphoreStartStop.WaitAsync();
                await StopListeningImpl();
            }
            finally
            {
                _semaphoreStartStop.Release();
            }
        }

        private async Task StartListeningImpl()
        {
            //Always use with _semaphoreStartStop
            if (_subscriptionClient != null)
            {
                return;
            }

            try
            {
                if (string.IsNullOrEmpty(_sasToken))
                {
                    var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(Config.ServiceBus.KeyName, Config.ServiceBus.SharedAccessKey, TimeSpan.FromMinutes(30));
                    _sasToken = (await tokenProvider.GetTokenAsync(Config.ServiceBus.Url, TimeSpan.FromMinutes(30))).TokenValue;
                }


                //TODO correct endpoints
                _subscriptionClient = new SubscriptionClient(Config.ServiceBus.Url,
                                                             TopicPath,
                                                             SubscriptionName,
                                                             TokenProvider.CreateSharedAccessSignatureTokenProvider(_sasToken),
                                                             receiveMode: ReceiveMode.PeekLock,
                                                             retryPolicy: _retryPolicy);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return;
            }

            // register the RegisterMessageHandler callback
            _subscriptionClient.RegisterMessageHandler(
                async (message, cancellationToken1) =>
                {
                    if (_subscriptionClient != null && !_subscriptionClient.IsClosedOrClosing)
                    {
                        await MessageHandler(message, cancellationToken1, _subscriptionClient);
                    }
                },
                new MessageHandlerOptions(LogMessageHandlerException) { AutoComplete = false, MaxConcurrentCalls = MAX_CONCURRENT_CALLS });
        }

        private async Task StopListeningImpl()
        {
            //Always use with _semaphoreStartStop
            if (_subscriptionClient != null)
            {
                await _subscriptionClient.CloseAsync();
                _subscriptionClient = null;
            }
        }

        private Task LogMessageHandlerException(ExceptionReceivedEventArgs e)
        {
            Debug.WriteLine(e.Exception.Message);

            return Task.CompletedTask;
        }

        private Task MessageHandler(Message message, CancellationToken cancellationToken1, SubscriptionClient receiver)
        {
            _messenger.Publish(new NotificationMessage(this, message));

            CompleteMessage();

            return Task.CompletedTask;

            void CompleteMessage()
            {
                try
                {
                    receiver.CompleteAsync(message.SystemProperties.LockToken);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }
    }
}
