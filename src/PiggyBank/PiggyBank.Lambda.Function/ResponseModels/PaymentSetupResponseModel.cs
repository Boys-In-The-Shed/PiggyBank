using System;
using System.Collections.Generic;
using System.Text;

namespace PiggyBank.Lambda.Function.ResponseModels
{
    class PaymentSetupResponseModel
    {
        public string payment_intent_id { get; set; }
        public string client_secret { get; set; }
    }
}
