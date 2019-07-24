using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Refit;

namespace servicebus.communication.Core
{
    [LazySingletonService]
    public class Webservice : IWebservice
    {
        private HttpClient _commandClient;

        private string _sasToken;

        public async Task<bool> SendCommand()
        {
            if (string.IsNullOrEmpty(_sasToken))
            {
                var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(Config.EventHub.KeyName, Config.EventHub.SharedAccessKey, TimeSpan.FromMinutes(30));
                _sasToken = (await tokenProvider.GetTokenAsync(Config.EventHub.Url, TimeSpan.FromMinutes(30))).TokenValue;
            }

            try
            {
                await CommandApi.SendCommand("body", _sasToken);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }

            return true;
        }

        private IApi _commandApi;
        private IApi CommandApi
        {
            get
            {
                _commandClient = _commandClient ?? ClientFor(Config.EventHub.Url);

                _commandApi = _commandApi ?? RestService.For<IApi>(_commandClient, Settings);

                return _commandApi;
            }
        }

        private static RefitSettings Settings
        {
            get
            {
                return new RefitSettings
                {
                    JsonSerializerSettings = new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Converters = new List<JsonConverter>
                        {
                            new StringEnumConverter()
                        }
                    }
                };
            }
        }

        private HttpClient ClientFor(string url)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(url),
                Timeout = TimeSpan.FromSeconds(10)
            };

            return client;
        }

        private string SerializeCommand(object obj)
        {
            return JsonConvert.SerializeObject(obj, Settings.JsonSerializerSettings);
        }
    }
}
