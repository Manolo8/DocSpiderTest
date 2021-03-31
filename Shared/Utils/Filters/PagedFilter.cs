using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using LinqKit;

namespace Shared.Utils.Filters {
    public class PagedFilter<TItem> : IPaged {
        [Range(1, 50)]
        public int Size { get; set; } = 10;

        [Range(0, int.MaxValue)]
        public int Page { get; set; } = 1;

        public string SortColumn { get; set; }
        public bool   SortAsc    { get; set; }

        public virtual Expression<Func<TItem, bool>> ToExpresion() {
            return Predicate<TItem>();
        }

        protected Expression<Func<T, bool>> Predicate<T>() {
            return PredicateBuilder.New<T>(true);
        }
    }
}