namespace PiggyBank.Lambda.Function.Endpoints
{
    [Endpoint("POST", "/payment/setup")]
    public class TestStripeEndpoint : IEndpoint
    {
        public Response Handle(Request request)
        {
            return new Response(System.Net.HttpStatusCode.OK, new
            {
                payment_intent_id = "lkjasdfkljsadf",
                client_secret = "sajkhdfkjsdf"
            });
        }
    }
}
