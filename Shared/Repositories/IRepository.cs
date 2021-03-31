using System;
using System.Linq.Expressions;
using Shared.Entities;
using Shared.Utils.Filters;

namespace Shared.Repositories {
    public interface IRepository<TEntity>
        where TEntity : Entity {
        public T Find<T>(long id, Expression<Func<TEntity, T>> parser);

        FilterResult<T> Search<T>(PagedFilter<TEntity> filter, Expression<Func<TEntity, T>> parser);

        #region Find

        T Find<T>(Expression<Func<TEntity, bool>> predicate) where T : IToDto<TEntity, T>;

        T Find<T>(long id) where T : IToDto<TEntity, T>;

        #endregion

        #region Crud

        TEntity Create(Action<TEntity> action, bool save = true);

        TEntity Create<TPatch>(TPatch patch, bool save = true)
            where TPatch : IPatch<TEntity>, ICreate;

        TEntity Create(TEntity entity, bool save = true);

        TEntity Update<TPatch>(long id, TPatch patch, bool save = true)
            where TPatch : IPatch<TEntity>, IUpdate;

        TEntity Update(TEntity entity, bool save = true);

        TEntity Update(long id, Action<TEntity> action, bool save = true);

        void Update<TPatchList>(TPatchList patch, bool save = true)
            where TPatchList : IPatchList<TEntity>, IUpdate;

        bool Delete(long id, bool save = true);

        #endregion

        void Save();
    }
}