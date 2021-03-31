using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Shared.Repositories {
    public class Patcher {
        private readonly DbContext _context;

        public Patcher(DbContext context) {
            _context = context;
        }

        public T NewInstance<T>(IPatch<T> patch)
            where T : Entity {
            var instance = Activator.CreateInstance<T>();

            patch.Apply(instance, this);

            if (!instance.IsNew())
                //don't allow that
                instance.Id = 0;

            return instance;
        }

        public void Load<TEntity, TProperty>(
            TEntity                                           entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> expression
        ) where TEntity : Entity where TProperty : class {
            var entry = _context.Entry(entity);

            entry.Collection(expression).Load();
        }

        public void AttachAndApply<TEntity>(
            IPatch<TEntity> patch, long id
        ) where TEntity : Entity {
            var entity = Activator.CreateInstance<TEntity>();

            entity.Id = id;

            patch.Apply(_context.Attach(entity).Entity, this);
        }

        public void Reload(Entity entity) {
            _context.Entry(entity).Reload();
        }

        public TProperty Value<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> expression)
            where TEntity : Entity {
            return _context.Set<TEntity>().AsNoTracking().Where(x => x.Id == entity.Id).Select(expression).FirstOrDefault();
        }
    }
}