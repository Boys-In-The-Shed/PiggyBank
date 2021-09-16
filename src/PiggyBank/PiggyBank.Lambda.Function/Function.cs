using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Autofac;
using PiggyBank.Database;
using PiggyBank.Stripe;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace PiggyBank.Lambda.Function
{
	public class Function
	{
		private readonly IContainer _container; 
		private readonly Dictionary<(HttpMethod, string), Type> _endpointTypes = new Dictionary<(HttpMethod, string), Type>();
		private readonly DateTimeOffset _startupTime;

		public Function() 
		{
			_startupTime = DateTimeOffset.Now;
			Console.WriteLine("Started setting up!");

			var contBuilder = new ContainerBuilder();
			Registrations(contBuilder);
			Console.WriteLine($"Finished registrations ({(DateTimeOffset.Now-_startupTime).TotalMilliseconds}ms)");

			var assembly = Assembly.GetExecutingAssembly();
			var endpointTypes = assembly.GetTypes()
				.Where(x => x.GetInterfaces().Contains(typeof(IEndpoint)))
				.Where(x => x.GetCustomAttributes().Any(y => y.GetType() == typeof(EndpointAttribute)));

			Console.WriteLine($"Got all endpoint types from assembly ({(DateTimeOffset.Now - _startupTime).TotalMilliseconds}ms)");

			foreach (var endpointType in endpointTypes) 
			{
				var attr = (EndpointAttribute)endpointType.GetCustomAttributes().Where(x => x.GetType() == typeof(EndpointAttribute)).Single();
				_endpointTypes[(new HttpMethod(attr.Method), attr.Path)] = endpointType;

				contBuilder.RegisterType(endpointType);
			}

			Console.WriteLine($"Registered all the endpoints ({(DateTimeOffset.Now - _startupTime).TotalMilliseconds}ms)");

			_container = contBuilder.Build();

			Console.WriteLine($"Built the container ({(DateTimeOffset.Now - _startupTime).TotalMilliseconds}ms)");
		}

		public static void Registrations(ContainerBuilder cb) 
		{
			cb.RegisterType<PaymentIntentService>()
				.As<ICreatable<PaymentIntent, PaymentIntentCreateOptions>>()
				.As<IRetrievable<PaymentIntent, PaymentIntentGetOptions>>();

			cb.RegisterType<StripeService>().As<IStripeService>();
			cb.RegisterType<DynamoKeyValueStore>().As<IKeyValueStore>().SingleInstance();
		}

		public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apiGatewayRequest, ILambdaContext context)
		{
			Console.WriteLine($"Handling incoming request... ({(DateTimeOffset.Now - _startupTime).TotalMilliseconds}ms)");
			try
			{
				var request = new Request(apiGatewayRequest);
				var response = await FindHandler(request);
				Console.WriteLine($"Found handler for request ({(DateTimeOffset.Now - _startupTime).TotalMilliseconds}ms)");

				// CORS.
				var apiGatewayResponse = response.GetResponse();
				Console.WriteLine($"Finished handling ({(DateTimeOffset.Now - _startupTime).TotalMilliseconds}ms)");
				apiGatewayResponse.Headers.Add("Access-Control-Allow-Origin", "*");
				apiGatewayResponse.Headers.Add("Access-Control-Allow-Headers", "*");
				apiGatewayResponse.Headers.Add("Access-Control-Allow-Methods", "*");

				return apiGatewayResponse;
			}
			catch (Exception e)
			{
				return new Response(HttpStatusCode.InternalServerError, new { message = e.Message }).GetResponse();
			}
		}

		public async Task<Response> FindHandler(Request request) 
		{
			var lookupKey = (request.Method, request.Path);

			if (!_endpointTypes.ContainsKey(lookupKey)) 
				return new Response(HttpStatusCode.NotFound, "Sorry, don't know what you're looking for!");

			var endpointType = _endpointTypes[lookupKey];
			var endpoint = (IEndpoint)_container.Resolve(endpointType);

			return await endpoint.Handle(request);
		}
	}
}
