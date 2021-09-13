using Newtonsoft.Json;

namespace PiggyBank.Lambda.Function.RequestModels
{
    public class PaymentSetupRequestModel
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }
}