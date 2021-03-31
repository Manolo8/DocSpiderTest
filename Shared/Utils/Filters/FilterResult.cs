using System.Collections.Generic;

namespace Shared.Utils.Filters {
    public class FilterResult<T> {
        public int            Total { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}