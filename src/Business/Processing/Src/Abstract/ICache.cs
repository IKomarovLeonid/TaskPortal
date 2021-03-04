using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Processing.Abstract
{
    public interface ICache<K,T> where T: class
    {
        void AddOrUpdate(K key, T item);

        IReadOnlyDictionary<K,T> GetAll();

        void Remove(K key, T item);

        T Get(K key);
    }
}
