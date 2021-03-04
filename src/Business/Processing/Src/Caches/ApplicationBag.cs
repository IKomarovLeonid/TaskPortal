using System.Collections.Concurrent;
using System.Collections.Generic;
using Processing.Abstract;

namespace Processing.Caches
{
    public class ApplicationBag<T> : IBag<T>
    {
        // data
        private ConcurrentBag<T> _data;

        public ApplicationBag()
        {
            _data = new ConcurrentBag<T>();
        }

        public void Push(T item)
        {
            _data.Add(item);
        }

        public ICollection<T> Take()
        {
            var collection = new List<T>();

            while (_data.Count != 0)
            {
                if (_data.TryTake(out var result))
                {
                    collection.Add(result);
                }
                else break;
            }
            
            return collection;
        }
    }
}
