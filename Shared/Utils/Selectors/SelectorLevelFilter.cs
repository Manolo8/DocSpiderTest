using System;
using System.Linq.Expressions;
using LinqKit;

namespace Shared.Utils.Selectors {
    public abstract class SelectorLevelFilter<TItem> : BaseSelectorFilter<TItem, SelectorLevel> {
        public long? ParentId { get; set; }

        public override Expression<Func<SelectorLevel, bool>> ToExpresion() {
            var expression = base.ToExpresion();

            if (Ids == null)
                expression = expression.And(e => e.ParentId == ParentId);

            return expression;
        }
    }
}