namespace PiggyBank.Stripe 
{
    interface IStripeService
    {
        Task<string> SetupPaymentIntent(decimal amount); 
        
    }
}