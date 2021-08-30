namespace PiggyBank.Lambda
{
    public interface IEndpoint
    {
        Response Handle(Request request);
    }
}
