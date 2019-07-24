using Microsoft.Azure.ServiceBus;
using MvvmCross.Plugin.Messenger;

namespace servicebus.communication.Core
{
    public class NotificationMessage : MvxMessage
    {
        public Message Message { get; }

        public NotificationMessage(object sender, Message message)
            : base(sender)
        {
            Message = message;
        }
    }
}
