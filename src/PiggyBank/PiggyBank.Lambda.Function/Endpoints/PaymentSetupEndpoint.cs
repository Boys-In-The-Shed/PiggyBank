namespace PiggyBank.Lambda.Function
{
    [Endpoint("POST", "/payment/setup")]
    public class PaymentSetupEndpoint : IEndpoint
    {
        public Response Handle(Request request)
        {
            var body = request.DeserializeBody<PaymentSetupRequestModel>();

            return new Response(System.Net.HttpStatusCode.OK, null);
        }
    }
}