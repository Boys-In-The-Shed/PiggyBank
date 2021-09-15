using PiggyBank.Database;
using PiggyBank.Lambda.Function.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PiggyBank.Lambda.Function.Endpoints
{
    [Endpoint("GET", "/balance")]
    class GetBalanceEndpoint : IEndpoint
    {
        private readonly IKeyValueStore _database; 

        public GetBalanceEndpoint(IKeyValueStore database)
        {
            _database = database;
        }

        public async Task<Response> Handle(Request request)
        {
            var balance = await _database.GetAsync<decimal>("balance");

            return new Response(System.Net.HttpStatusCode.OK, new GetBalanceResponseModel 
            { 
                current_balance = balance
            });
        }
    }
}
