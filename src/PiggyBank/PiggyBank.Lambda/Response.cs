using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;

namespace PiggyBank.Lambda
{
    public class Response<T>
    {
        public Response(HttpStatusCode statusCode, T responseModel)
        {
            StatusCode = statusCode;
            ResponseModel = responseModel;
        }

        public HttpStatusCode StatusCode { get; }
        public T ResponseModel { get; }

        public APIGatewayProxyResponse GetResponse() =>
            new APIGatewayProxyResponse{
                StatusCode = (int)StatusCode,
                Body = JsonConvert.SerializeObject(ResponseModel),
            };
        
    }
}
