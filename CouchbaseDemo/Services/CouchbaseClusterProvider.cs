using Couchbase;
using Couchbase.KeyValue;
using System.Text.Json;

namespace CouchbaseDemo.Services
{
    public class CouchbaseClusterProvider : ICouchbaseClusterProvider
    {
        private readonly ICluster _cluster;
        private readonly IBucket _bucket;
        private readonly ICouchbaseCollection _collection;

        public CouchbaseClusterProvider(ICluster cluster, IBucket bucket)
        {
            _cluster = cluster;
            _bucket = bucket;
        }

        public CouchbaseClusterProvider(string connectionString, string username, string password, string bucketName)
        {
            _cluster = Cluster.ConnectAsync(connectionString, username, password).Result;
            _bucket = _cluster.BucketAsync(bucketName).Result;
            _collection = _bucket.DefaultCollection();
        }

        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                var result = await _collection.GetAsync(key);
                return JsonSerializer.Deserialize<T>(result.ContentAs<string>());
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiry)
        {
            try
            {
                await _collection.UpsertAsync(key, JsonSerializer.Serialize(value), options =>
                {
                    options.Expiry(expiry);
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while setting the key '{key}': {ex.Message}", ex);
            }
        }

        public async Task DeleteAsync(string key)
        {
            try
            {
                await _collection.RemoveAsync(key);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the key '{key}': {ex.Message}", ex);
            }
        }

        public async Task UpdateAsync<T>(string key, T value, TimeSpan expiry)
        {
            try
            {
                var getResult = await _collection.GetAsync(key);
                if (getResult != null)
                {
                    await _collection.UpsertAsync(key, JsonSerializer.Serialize(value), options =>
                    {
                        options.Expiry(expiry);
                    });
                }
                else
                {
                    throw new Exception($"Key '{key}' not found for update.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the key '{key}': {ex.Message}", ex);
            }
        }
    }
}
