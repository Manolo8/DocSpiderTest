using System;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Shared.Utils.Filters;

namespace Shared.Utils.Selectors {
    public abstract class BaseSelectorFilter<TItem, TSelector> : PagedFilter<TSelector>
        where TSelector : Selector {
        public long[] Ids  { get; set; }
        public string Text { get; set; }

        public abstract Expression<Func<TItem, TSelector>> ToSelector();

        public virtual Expression<Func<TItem, bool>> ToExpressionBefore() {
            return Predicate<TItem>();
        }

        public override Expression<Func<TSelector, bool>> ToExpresion() {
            var expresion = base.ToExpresion();

            if (Ids != null)
                expresion = expresion.And(e => Ids.Contains(e.Id));

            if (Text != null)
                expresion = expresion.And(x => EF.Functions.Like(x.Text, $"%{Text}%"));

            return expresion;
        }
    }
}