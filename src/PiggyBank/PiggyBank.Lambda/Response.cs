using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;

namespace PiggyBank.Lambda
{
    public class Response
    {
        public Response(HttpStatusCode statusCode, object responseModel)
        {
            StatusCode = statusCode;
            ResponseModel = responseModel;
        }

        public HttpStatusCode StatusCode { get; }
        public object ResponseModel { get; }

        public APIGatewayProxyResponse GetResponse() =>
            new APIGatewayProxyResponse{
                StatusCode = (int)StatusCode,
                Body = JsonConvert.SerializeObject(ResponseModel),
            };
        
    }
}
