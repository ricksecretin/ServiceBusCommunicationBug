using System.Threading.Tasks;

namespace servicebus.communication.Core
{
    public interface IModelFacade
    {
        Task StartListening();

        Task StopListening();

        Task<bool> SendCommand();

    }
}
