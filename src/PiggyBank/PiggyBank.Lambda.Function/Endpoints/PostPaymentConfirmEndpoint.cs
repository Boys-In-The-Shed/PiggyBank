using PiggyBank.Database;
using PiggyBank.Lambda.Function.RequestModels;
using PiggyBank.Lambda.Function.ResponseModels;
using PiggyBank.Stripe;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PiggyBank.Lambda.Function.Endpoints
{
    [Endpoint("POST", "payment/confirm")]
    class PostPaymentConfirmEndpoint : IEndpoint
    {
        private readonly IKeyValueStore _database;
        private readonly IStripeService _stripe;

        public PostPaymentConfirmEndpoint(IKeyValueStore database, IStripeService stripe)
        {
            _database = database;
            _stripe = stripe;
        }

        public async Task<Response> Handle(Request request)
        {
            var requestBody = request.DeserializeBody<PaymentConfirmRequestModel>();

            var paymentIntentId = await _database.GetAsync<string>(requestBody.payment_intent_id);

            // Check if payment was already made
            if (paymentIntentId != null) 
            {
                return new Response(System.Net.HttpStatusCode.BadRequest, "Payment already confirmed");
            }
            
            // Get result of payment intent id
            var (status, amount) = await _stripe.CheckPaymentIntentStatus(requestBody.payment_intent_id);

            // Check if result was successful
            if (status != "succeeded")
            {
                return new Response(System.Net.HttpStatusCode.BadRequest, "Payment was not successful");
            }

            // Retreive current balance
            var balance = await _database.GetAsync<decimal>("balance");

            // Add payment to balance
            balance += amount;

            // Writeback balance
            await _database.SetAsync("balance", balance);

            // Add payment intent id to database
            await _database.SetAsync("payment_intent_id", requestBody.payment_intent_id);

            // return balance
            return new Response(System.Net.HttpStatusCode.OK, new PaymentConfirmResponseModel
            {
                current_balance = balance
            });
        }
    }
}
