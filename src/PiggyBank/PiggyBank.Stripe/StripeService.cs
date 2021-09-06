namespace PiggyBank.Stripe
{
    public class StripeService : IStripeService
    {
        public Task<string> SetupPaymentIntent(decimal amount) 
        {
            var paymentIntents = new PaymentIntentService();
            var paymentIntent = paymentIntents.Create(new PaymentIntentCreateOptions
            {
                Amount = CalculateOrderAmount(amount),
                Currency = "usd",
            });
            return Json(new { clientSecret = paymentIntent.ClientSecret });
        }
    }
}