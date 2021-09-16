using Stripe;
using System;
using System.Threading.Tasks;

namespace PiggyBank.Stripe
{
    public class StripeService : IStripeService
    {
        private readonly ICreatable<PaymentIntent, PaymentIntentCreateOptions> _paymentIntentCreator;
        private readonly IRetrievable<PaymentIntent, PaymentIntentGetOptions> _paymentIntentRetriever;

        public StripeService(
            ICreatable<PaymentIntent, PaymentIntentCreateOptions> paymentIntentCreator,
            IRetrievable<PaymentIntent, PaymentIntentGetOptions> paymentIntentRetriever)
        {
            _paymentIntentCreator = paymentIntentCreator;
            _paymentIntentRetriever = paymentIntentRetriever;
        }

        public async Task<(string paymentIntentId, string clientSecret)> SetupPaymentIntent(decimal amount) 
        {
            var paymentIntent = await _paymentIntentCreator.CreateAsync(new PaymentIntentCreateOptions
            {
                Amount = Convert.ToInt64(amount*100),
                Currency = "nzd",
            }, GetOptions());

            return (paymentIntent.Id, paymentIntent.ClientSecret);
        }

        public async Task<(string status, decimal amount)> CheckPaymentIntentStatus(string paymentIntent)
        {
            var result = await _paymentIntentRetriever.GetAsync(paymentIntent, requestOptions: GetOptions());

            var amount = ((decimal)result.Amount) / 100;

            return (result.Status, amount);
        }

        private RequestOptions GetOptions() => new RequestOptions
        {
            ApiKey = "sk_test_51JWWyOLVagDHTLlfKUoFEKhvMzos9Nmj0WQkDwEhjzWr0SXQKiHBn4zSxVUCeO9ITywdL8bqqcO1nN4aUgaBmFNl00fbRVbAHk"
        };
    }
}