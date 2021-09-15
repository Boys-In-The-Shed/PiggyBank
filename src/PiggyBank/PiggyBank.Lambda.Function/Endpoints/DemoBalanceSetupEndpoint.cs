using System.Threading.Tasks;

namespace PiggyBank.Lambda.Function.Endpoints
{
    [Endpoint("GET", "/balance")]
    public class DemoBalanceSetupEndpoint : IEndpoint
    {
        public Task<Response> Handle(Request request)
            => Task.FromResult(new Response(System.Net.HttpStatusCode.OK, new
            {
                current_balance = 1238.42
            }));
    }
}
