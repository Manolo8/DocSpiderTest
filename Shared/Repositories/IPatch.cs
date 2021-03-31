using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Shared.Repositories {
    public interface IPatch<TEntity> where TEntity : Entity {
        public void Apply(TEntity entity, Patcher patcher);
    }
}