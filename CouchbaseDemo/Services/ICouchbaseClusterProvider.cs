namespace CouchbaseDemo.Services
{
    public interface ICouchbaseClusterProvider
    {
        Task<T> GetAsync<T>(string key);

        Task SetAsync<T>(string key, T value, TimeSpan expiry);

        Task DeleteAsync(string key);

        Task UpdateAsync<T>(string key, T value, TimeSpan expiry);
    }
}
