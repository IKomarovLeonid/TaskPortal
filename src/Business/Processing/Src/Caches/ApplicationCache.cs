using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Processing.Abstract;
using Quartz.Impl.Matchers;

namespace Processing.Caches
{
    public class ApplicationCache<K,T> : ICache<K,T> where T: class
    {
        private readonly ConcurrentDictionary<K, T> _data;
        public ApplicationCache()
        {
            _data = new ConcurrentDictionary<K, T>();
        }

        public void AddOrUpdate(K key, T item)
        {
            var result = _data.AddOrUpdate(key, item, (k, t) => item);

            if(result == default) throw new ArgumentException($"Unable to add or update item {item.GetType()} by specific key {key.GetType()}");
        }

        public IReadOnlyDictionary<K,T> GetAll()
        {
            return _data;
        }

        public void Remove(K key, T item)
        {
            if (_data.TryAdd(key, item))
            {
                return;
            }

            throw new ArgumentException($"Unable to remove item {item.GetType()} by specific key {key.GetType()}");
        }

        public T Get(K key)
        {
            if(_data.TryGetValue(key, out var value))
            {
                return value;
            }

            throw new ArgumentException($"Unknown key {key.GetType()}");
        }
    }
}
