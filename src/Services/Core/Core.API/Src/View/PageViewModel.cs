using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Core.API.View
{
    public class PageViewModel<TModel>
    {
        public ICollection<TModel> Items { get; set; } = new Collection<TModel>();

        public static PageViewModel<TModel> Create(ICollection<TModel> models) =>
            new PageViewModel<TModel> {Items = models};
    }
}
