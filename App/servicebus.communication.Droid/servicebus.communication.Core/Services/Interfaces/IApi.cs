using System.Threading.Tasks;
using Refit;

namespace servicebus.communication.Core
{
    public interface IApi
    {
        [Post("")]
        Task SendCommand([Body] string body, [Header("Authorization")] string authorization);
    }
}
