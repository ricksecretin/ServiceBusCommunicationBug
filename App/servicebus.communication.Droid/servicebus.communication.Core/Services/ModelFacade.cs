using System.Threading.Tasks;

namespace servicebus.communication.Core
{
    [NonLazySingletonService]
    public class ModelFacade : IModelFacade
    {
        private readonly IServiceBusMessageReceiverService _serviceBusMessageReceiverService;
        private readonly IWebservice _webservice;

        public ModelFacade(IServiceBusMessageReceiverService serviceBusMessageReceiverService, IWebservice webservice)
        {
            _serviceBusMessageReceiverService = serviceBusMessageReceiverService;
            _webservice = webservice;
        }

        public Task StartListening()
        {
            return _serviceBusMessageReceiverService.StartListening();
        }

        public Task StopListening()
        {
            return _serviceBusMessageReceiverService.StopListening();
        }

        public Task<bool> SendCommand()
        {
            return _webservice.SendCommand();
        }
    }
}
