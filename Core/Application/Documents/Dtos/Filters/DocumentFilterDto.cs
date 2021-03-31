using System;
using System.Linq.Expressions;
using Core.Domain.Documents.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Shared.Utils.Filters;

namespace Core.Application.Documents.Dtos.Filters {
    public class DocumentFilterDto : PagedFilter<Document> {
        public string Title  { get; set; }
        public long   UserId { get; set; }

        public override Expression<Func<Document, bool>> ToExpresion() {
            var expression = base.ToExpresion();

            if (UserId != 0)
                expression = expression.And(x => x.UserId == UserId);

            if (Title != null)
                expression = expression.And(x => EF.Functions.ILike(x.Title, $"%{Title}%"));

            return expression;
        }
    }
}