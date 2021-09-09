using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Moq;
using NUnit.Framework;
using PiggyBank.Lambda.Function;

namespace PiggyBank.Test.Unit
{
    public class FunctionFixture
    {
        [Test]
        public void should_hit_demo_endpoint()
        {
            // Arrange.
            var sut = new Function();

            var apiGatewayRequest = new APIGatewayProxyRequest
            {
                HttpMethod = "GET",
                Path = "/demo"
            };

            // Act.
            var apiGatewayResponse = sut.FunctionHandler(apiGatewayRequest, Mock.Of<ILambdaContext>());

            // Assert.
            Assert.That(apiGatewayResponse.StatusCode, Is.EqualTo(200));
        }
    }
}