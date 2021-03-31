using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Shared.Exceptions;
using Shared.Utils.Filters;
using Shared.Utils.Selectors;

namespace Shared.Repositories {
    public class Repository<TEntity, TContext> : IRepository<TEntity>
        where TContext : DbContext
        where TEntity : Entity {
        protected readonly TContext Context;

        protected Repository(TContext context) {
            Context = context;
        }

        protected DbSet<TEntity> Table => Context.Set<TEntity>();

        #region Find

        public T Find<T>(long id, Expression<Func<TEntity, T>> parser) {
            return Find(e => e.Id == id, parser);
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate) {
            var query = Table.AsNoTracking().Where(predicate);
            return query.FirstOrDefault();
        }

        public TEntity Find(long id) {
            return Find(e => e.Id == id);
        }

        public T Find<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> parser) {
            var query = Table.AsNoTracking().Where(predicate).Select(parser);
            return query.FirstOrDefault();
        }

        public T Find<T>(Expression<Func<TEntity, bool>> predicate) where T : IToDto<TEntity, T> {
            var instance = Activator.CreateInstance<T>();

            return Find(predicate, instance.ToDto());
        }

        public T Find<T>(long id) where T : IToDto<TEntity, T> {
            var instance = Activator.CreateInstance<T>();

            return id == 0 ? instance : Find(id, instance.ToDto());
        }

        #endregion

        #region Search

        public FilterResult<Selector> Selector(SelectorFilter<TEntity> filter) {
            var query =
                Table.AsNoTracking()
                     .Where(filter.ToExpressionBefore())
                     .Select(filter.ToSelector());

            return Search(filter, query);
        }

        public FilterResult<SelectorLevel> SelectorLevel(SelectorLevelFilter<TEntity> filter) {
            var query =
                Table.AsNoTracking()
                     .Where(filter.ToExpressionBefore())
                     .Select(filter.ToSelector());

            var result = Search(filter, query);

            return result;
        }

        public FilterResult<T> Search<T>(PagedFilter<TEntity> filter) where T : IToDto<TEntity, T> {
            var instance = Activator.CreateInstance<T>();

            return Search(filter, instance.ToDto());
        }

        public FilterResult<T> Search<T>(PagedFilter<TEntity> filter, Expression<Func<TEntity, T>> parser) {
            var query =
                Table
                    .AsNoTracking()
                    .Where(filter.ToExpresion())
                    .Select(parser);

            return Paginate(filter, query);
        }

        protected FilterResult<T> Search<T>(PagedFilter<T> filter, IQueryable<T> query) {
            query = query.Where(filter.ToExpresion());

            return Paginate(filter, query);
        }

        protected static FilterResult<T> Paginate<T>(IPaged paged, IQueryable<T> query) {
            ValidatePaged<T>(paged);

            if (paged.SortColumn != null)
                query = paged.SortAsc ?
                            query.OrderBy(e => EF.Property<T>(e, paged.SortColumn)) :
                            query.OrderByDescending(e => EF.Property<T>(e, paged.SortColumn));

            var total  = query.Count();
            var result = query.Skip(paged.Size * (paged.Page - 1)).Take(paged.Size).ToList();

            return new FilterResult<T> {Total = total, Items = result};
        }

        private static void ValidatePaged<T>(IPaged paged) {
            var type = typeof(T);

            if (paged.SortColumn == null) {
                if (type.GetProperty("Id") != null)
                    paged.SortColumn = "Id";
            }
            else if (type.GetProperty(paged.SortColumn) == null)
                throw new ValidationGenericException(paged.SortColumn + " não existe em " + type.Name);
        }

        #endregion

        #region Utility

        public bool Any(Expression<Func<TEntity, bool>> predicate) {
            return Table.Any(predicate);
        }

        #endregion

        #region Crud

        public void Create(IEnumerable<TEntity> entities, bool save = true) {
            foreach (var entity in entities)
                Create(entity, false);

            if (save)
                Save();
        }

        public TEntity Create<TPatch>(TPatch patch, bool save = true)
            where TPatch : IPatch<TEntity>, ICreate {
            return Create(entity => patch.Apply(entity, new Patcher(Context)), save);
        }

        public TEntity Create(TEntity entity, bool save = true) {
            var result = Context.Add(entity).Entity;

            if (save)
                Context.SaveChanges();

            return result;
        }

        public TEntity Create(Action<TEntity> action = null, bool save = true) {
            var entity = Activator.CreateInstance<TEntity>();

            action?.Invoke(entity);

            return Create(entity, save);
        }

        public TEntity Update(long id, Action<TEntity> action, bool save = true) {
            var entry = Table.Find(id);

            action(entry);

            if (save)
                Context.SaveChanges();

            return entry;
        }

        public TEntity Update<TPatch>(long id, TPatch patch, bool save = true)
            where TPatch : IPatch<TEntity>, IUpdate {
            return Update(id, entity => patch.Apply(entity, new Patcher(Context)), save);
        }

        public TEntity Update(TEntity entity, bool save = true) {
            var result = Context.Update(entity);

            if (save)
                Context.SaveChanges();

            return result.Entity;
        }

        public void Update<TPatchList>(TPatchList patch, bool save = true)
            where TPatchList : IPatchList<TEntity>, IUpdate {
            patch.Apply(new Patcher(Context));

            if (save)
                Context.SaveChanges();
        }

        public bool Delete(long id, bool save = true) {
            var entry = Table.Find(id);

            if (entry == null)
                return false;

            Context.Remove(entry);

            if (save)
                Context.SaveChanges();

            return true;
        }

        public void Save() {
            Context.SaveChanges();
        }

        #endregion
    }
}