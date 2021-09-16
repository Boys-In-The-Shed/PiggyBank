using System.Threading.Tasks;

namespace PiggyBank.Stripe 
{
    public interface IStripeService
    {
        Task<(string paymentIntentId, string clientSecret)> SetupPaymentIntent(decimal amount);
        Task<(string status, decimal amount)> CheckPaymentIntentStatus(string paymentIntent);
    }
}