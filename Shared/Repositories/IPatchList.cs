using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Shared.Repositories {
    public interface IPatchList<TEntity> where TEntity : Entity {
        public void Apply(Patcher patcher);
    }
}