using Shared.Dtos;
using Shared.Entities;
using Shared.Repositories;

namespace Shared.Services {
    public abstract class CruService<TModel, TEntity, TRepository>
        : EntityService<TEntity, TRepository>
        where TModel : DtoWithId
        where TEntity : Entity
        where TRepository : IRepository<TEntity> {
        protected CruService(TRepository repository) : base(repository) { }

        protected abstract TEntity CreateReturnEntity(TModel model);
        protected abstract TEntity UpdateReturnEntity(TModel model);

        public abstract TModel Model(long id);
    }
}