namespace PiggyBank.Lambda.Function
{
    [Endpoint("GET", "/demo")]
	public class DemoEndpoint : IEndpoint
	{
        public Response Handle(Request request) 
        {
            return new Response(System.Net.HttpStatusCode.OK, "Test");
        }
	}
}