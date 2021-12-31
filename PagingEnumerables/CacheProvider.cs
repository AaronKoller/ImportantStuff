using System.Runtime.Caching;
namespace PagingEnumerables
{
    public interface ICacheProvider<TValue>
    {
        void Save(string key, TValue value);
    }

    public class CacheProvider<TValue> : ICacheProvider<TValue>
    {
        private readonly MemoryCache _memoryCache = new MemoryCache("TotalWorks");
        public void Save(string key, TValue value)
        {
            var cacheItem = new CacheItem(key, value);
            _memoryCache.Set(cacheItem, new CacheItemPolicy());
        }
    }
}