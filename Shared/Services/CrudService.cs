using Shared.Dtos;
using Shared.Entities;
using Shared.Repositories;

namespace Shared.Services {
    public abstract class CrudService<TDto, TEntity, TRepository>
        : CruService<TDto, TEntity, TRepository>, ICrudService<TDto, TEntity>
        where TDto : DtoWithId, IPatch<TEntity>, ICreate, IUpdate, IToDto<TEntity, TDto>
        where TEntity : Entity
        where TRepository : IRepository<TEntity> {
        protected CrudService(TRepository repository) : base(repository) { }

        public TDto Create(TDto dto) {
            var entity = CreateReturnEntity(dto);

            return Model(entity.Id);
        }

        public TDto Update(TDto model) {
            var entity = UpdateReturnEntity(model);

            return Model(entity.Id);
        }

        protected override TEntity CreateReturnEntity(TDto model) => Repository.Create(model);

        protected override TEntity UpdateReturnEntity(TDto model) => Repository.Update(model.Id, model);

        public override TDto Model(long  id) => Repository.Find<TDto>(id);
        public virtual  bool Delete(long id) => Repository.Delete(id);
    }
}