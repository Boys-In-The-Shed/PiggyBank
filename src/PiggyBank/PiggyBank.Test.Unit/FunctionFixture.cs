using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Moq;
using NUnit.Framework;
using PiggyBank.Lambda.Function;
using System.Threading.Tasks;

namespace PiggyBank.Test.Unit
{
    public class FunctionFixture
    {
        [Test]
        public async Task should_hit_demo_balance_endpoint()
        {
            // Arrange.
            var sut = new Function();

            var apiGatewayRequest = new APIGatewayProxyRequest
            {
                HttpMethod = "GET",
                Path = "/balance"
            };

            // Act.
            var apiGatewayResponse = await sut.FunctionHandler(apiGatewayRequest, Mock.Of<ILambdaContext>());

            // Assert.
            Assert.That(apiGatewayResponse.StatusCode, Is.EqualTo(200));
        }
    }
}