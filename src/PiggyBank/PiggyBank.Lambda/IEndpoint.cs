using System.Threading.Tasks;

namespace PiggyBank.Lambda
{
    public interface IEndpoint
    {
        Task<Response> Handle(Request request);
    }
}
