using Shared.Entities;
using Shared.Repositories;

namespace Shared.Services {
    public class EntityService<TEntity, TRepository> : IEntityService<TEntity>
        where TEntity : Entity
        where TRepository : IRepository<TEntity> {
        protected readonly TRepository Repository;

        protected EntityService(TRepository repository) {
            Repository = repository;
        }
    }
}