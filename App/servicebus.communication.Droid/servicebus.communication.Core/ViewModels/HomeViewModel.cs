using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace servicebus.communication.Core
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IMvxMessenger _messenger;
        private MvxSubscriptionToken _subscriptionToken;

        private int index;

        public bool IsListening { get; private set; }

        public MvxObservableCollection<string> Results { get; }

        public IMvxAsyncCommand CommandSendMessage { get; }

        public IMvxAsyncCommand CommandReset { get; }

        public HomeViewModel(IModelFacade modelFacade, IMvxMessenger messenger)
            : base(modelFacade)
        {
            _messenger = messenger;

            CommandSendMessage = new MvxAsyncCommand(SendMessage);
            CommandReset = new MvxAsyncCommand(ResetConnection);

            Results = new MvxObservableCollection<string>();
        }

        private async Task ResetConnection()
        {
            await ModelFacade.StopListening();
            await ModelFacade.StartListening();
        }

        private async Task SendMessage()
        {
            if (!IsListening)
            {
                await ModelFacade.StartListening();

                _subscriptionToken = _messenger.SubscribeOnMainThread<NotificationMessage>(OnMessageReceived);

                IsListening = true;
            }

            var result = await ModelFacade.SendCommand();

            var tmp = result ? "Success" : "Fail";
            Results.Add($"{index++}. Message sent {tmp}");
        }

        private void OnMessageReceived(NotificationMessage obj)
        {
            Results.Add($"{index++}. Notifcation Received");
        }
    }
}
