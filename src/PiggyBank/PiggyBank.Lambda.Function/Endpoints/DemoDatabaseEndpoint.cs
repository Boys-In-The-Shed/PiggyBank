using PiggyBank.Database;
using System;
using System.Threading.Tasks;

namespace PiggyBank.Lambda.Function.Endpoints
{
    [Endpoint("GET", "/database")]
    public class DemoDatabaseEndpoint : IEndpoint
    {
        private readonly IKeyValueStore _keyVaultStore;

        public DemoDatabaseEndpoint(IKeyValueStore keyValueStore)
        {
            _keyVaultStore = keyValueStore ?? throw new ArgumentNullException(nameof(keyValueStore));
        }

        public async Task<Response> Handle(Request request)
        {
            var currentValue = await _keyVaultStore.GetAsync<int>("test_value");
            currentValue++;

            await _keyVaultStore.SetAsync<int>("test_value", currentValue);

            return new Response(System.Net.HttpStatusCode.OK, new
            {
                CurrentValue = currentValue
            });
        }
    }
}
