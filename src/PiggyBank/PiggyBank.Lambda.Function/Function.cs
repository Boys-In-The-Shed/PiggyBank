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
		public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
			=> new APIGatewayProxyResponse
			{
				StatusCode = (int)HttpStatusCode.OK,
				Body = JsonConvert.SerializeObject(new { Now = DateTimeOffset.Now })
			};
	}
}
