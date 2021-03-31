using System;
using System.Linq;
using Shared.Utils.Filters;
using Shared.Utils.Selectors;

namespace Shared.Utils.Other {
    public class EnumUtils {
        public static FilterResult<Selector> EnumToResult<TType>(SelectorFilter<Selector> filter) {
            var values =
                (from object value in Enum.GetValues(typeof(TType))
                 select new Selector {
                     Id   = (int) value,
                     Text = Enum.GetName(typeof(TType), value)
                 })
                .Where(filter.ToExpresion().Compile())
                .OrderBy(e => e.Text)
                .ToList();

            var total =
                values.Count;
            
            var result =
                values.Skip(filter.Size * (filter.Page - 1))
                      .Take(filter.Size)
                      .ToList();

            return new FilterResult<Selector> {
                Items = result,
                Total = total
            };
        }
    }
}