using System.Net.Http;
using Amazon.Lambda.APIGatewayEvents;

namespace PiggyBank.Lambda
{
    public class Request
    {
        public Request(APIGatewayProxyRequest request)
        {
            Method = new HttpMethod(request.HttpMethod);
            Path = request.Path;
        }

        public HttpMethod Method { get; }
        public string Path { get; }

        public string GetEndpoint() 
        {
            return Method + Path;
        }
    }
}
