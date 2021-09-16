using System;
using System.Collections.Generic;
using System.Text;

namespace PiggyBank.Lambda.Function.RequestModels
{
    class PaymentConfirmRequestModel
    {
        public string payment_intent_id { get; set; }
    }
}
