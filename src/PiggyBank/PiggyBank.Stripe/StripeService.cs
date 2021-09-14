using Stripe;
using System;
using System.Threading.Tasks;

namespace PiggyBank.Stripe
{
    public class StripeService : IStripeService
    {
        private readonly ICreatable<PaymentIntent, PaymentIntentCreateOptions> _paymentIntentService;

        public StripeService(ICreatable<PaymentIntent, PaymentIntentCreateOptions> paymentIntentService)
        {
            _paymentIntentService = paymentIntentService;
        }

        public async Task<(string paymentIntentId, string clientSecret)> SetupPaymentIntent(decimal amount) 
        {
            var options = new RequestOptions
            {
                ApiKey = "sk_test_51JWWyOLVagDHTLlfKUoFEKhvMzos9Nmj0WQkDwEhjzWr0SXQKiHBn4zSxVUCeO9ITywdL8bqqcO1nN4aUgaBmFNl00fbRVbAHk"
            };

            var paymentIntent = await _paymentIntentService.CreateAsync(new PaymentIntentCreateOptions
            {
                Amount = Convert.ToInt64(amount*100),
                Currency = "nzd",
            }, options);

            return (paymentIntent.Id, paymentIntent.ClientSecret);
        }
    }
}