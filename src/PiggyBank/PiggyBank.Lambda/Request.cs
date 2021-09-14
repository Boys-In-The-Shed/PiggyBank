using System;
using System.Net.Http;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;

namespace PiggyBank.Lambda
{
    public class Request
    {
        public Request(APIGatewayProxyRequest request)
        {
            Method = new HttpMethod(request.HttpMethod);
            Path = request.Path;
            Body = request.Body;
        }

        public HttpMethod Method { get; }
        public string Path { get; }
        public string Body { get; }

        public string GetEndpoint() 
        {
            return Method + Path;
        }

        public T DeserializeBody<T>()
        {
            return JsonConvert.DeserializeObject<T>(Body);
        }
    }
}
