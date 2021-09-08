using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Autofac;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace PiggyBank.Lambda.Function
{
	public class Function
	{
		private readonly IContainer _container; 
		private readonly Dictionary<(HttpMethod, string), Type> _endpointTypes = new Dictionary<(HttpMethod, string), Type>();

		public Function() 
		{
			var contBuilder = new ContainerBuilder();

			var assembly = Assembly.GetExecutingAssembly();
			var endpointTypes = assembly.GetTypes()
				.Where(x => x.GetInterfaces().Contains(typeof(IEndpoint)))
				.Where(x => x.GetCustomAttributes().Any(y => y.GetType() == typeof(EndpointAttribute)));

			foreach (var endpointType in endpointTypes) 
			{
				var attr = (EndpointAttribute)endpointType.GetCustomAttributes().Where(x => x.GetType() == typeof(EndpointAttribute)).Single();
				_endpointTypes[(new HttpMethod(attr.Method), attr.Path)] = endpointType;

				contBuilder.RegisterType(endpointType);
			}

			_container = contBuilder.Build();
		}

		public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest apiGatewayRequest, ILambdaContext context)
		{
			var request = new Request(apiGatewayRequest);
			var response = FindHandler(request);

			// CORS.
			var apiGatewayResponse = response.GetResponse();
			apiGatewayResponse.Headers.Add("Access-Control-Allow-Origin", "*");
			apiGatewayResponse.Headers.Add("Access-Control-Allow-Headers", "*");
			apiGatewayResponse.Headers.Add("Access-Control-Allow-Methods", "*");

			return response.GetResponse();
		}

		public Response FindHandler(Request request) 
		{
			var lookupKey = (request.Method, request.Path);

			if (!_endpointTypes.ContainsKey(lookupKey)) 
				return new Response(HttpStatusCode.NotFound, "Sorry, don't know what you're looking for!");

			var endpointType = _endpointTypes[lookupKey];
			var endpoint = (IEndpoint)_container.Resolve(endpointType);

			return endpoint.Handle(request);
		}
	}
}
