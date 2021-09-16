using System.Threading.Tasks;
using PiggyBank.Lambda.Function.RequestModels;
using PiggyBank.Lambda.Function.ResponseModels;
using PiggyBank.Stripe;

namespace PiggyBank.Lambda.Function
{
    [Endpoint("POST", "/payment/setup")]
    public class PaymentSetupEndpoint : IEndpoint
    {
        private readonly IStripeService _stripeService;

        public PaymentSetupEndpoint(IStripeService stripeService)
        {
            _stripeService = stripeService;
        }
        public async Task<Response> Handle(Request request)
        {
            var body = request.DeserializeBody<PaymentSetupRequestModel>();
            var amount = body.Amount;

            if (amount <= 0) return new Response(System.Net.HttpStatusCode.UnprocessableEntity, "Sorry, invalid amount entered");
            
            var paymentIntentObj = await _stripeService.SetupPaymentIntent(amount);

            return new Response(System.Net.HttpStatusCode.OK, new PaymentSetupResponseModel 
            { 
                payment_intent_id = paymentIntentObj.paymentIntentId,
                client_secret = paymentIntentObj.clientSecret
            });
        }
    }
}