using System.Collections.Generic;

namespace Processing.Abstract
{
    public interface IBag<T>
    {
        void Push(T item);

        ICollection<T> Take();
    }
}
