using System.Threading.Tasks;

namespace servicebus.communication.Core
{
    public interface IWebservice
    {
        Task<bool> SendCommand();
    }
}
