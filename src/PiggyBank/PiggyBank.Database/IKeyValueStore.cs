using System.Threading.Tasks;

namespace PiggyBank.Database
{
    public interface IKeyValueStore
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value);
    }
}
