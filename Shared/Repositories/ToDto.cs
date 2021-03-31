using System;
using System.Linq.Expressions;
using Shared.Entities;

namespace Shared.Repositories {
    public interface IToDto<TEntity, TDto> where TEntity : Entity {
        public Expression<Func<TEntity, TDto>> ToDto();
    }
}