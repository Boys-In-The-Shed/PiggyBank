using Stripe;

namespace PiggyBank.Stripe
{
    public interface IPaymentIntentService :
        ICreatable<PaymentIntent, PaymentIntentCreateOptions>,
        IRetrievable<PaymentIntent, PaymentIntentGetOptions>
    {
    }
}
