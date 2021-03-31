using Shared.Dtos;
using Shared.Entities;

namespace Shared.Services {
    public interface ICrudService<TModel, TEntity>
        : IEntityService<TEntity>
        where TModel : DtoWithId
        where TEntity : Entity {
        public TModel Create(TModel dto);

        public TModel Update(TModel dto);

        public TModel Model(long id);

        public bool Delete(long id);
    }
}