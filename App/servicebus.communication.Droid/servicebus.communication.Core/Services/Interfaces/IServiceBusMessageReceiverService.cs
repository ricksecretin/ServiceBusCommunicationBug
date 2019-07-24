using System.Threading.Tasks;

namespace servicebus.communication.Core
{
    public interface IServiceBusMessageReceiverService
    {
        Task StartListening();

        Task StopListening();
    }
}
