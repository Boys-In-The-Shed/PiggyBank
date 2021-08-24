using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using System;
using System.Net;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace PiggyBank.Lambda.Function
{
	public class Function
	{
		public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest apiGatewayRequest, ILambdaContext context)
		{
			var request = new Request(apiGatewayRequest);

			return new Response<string>(HttpStatusCode.OK, "Thanks for requesting!").GetResponse();
		}
	}
}
