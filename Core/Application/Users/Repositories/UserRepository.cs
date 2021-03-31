using System;
using System.Linq;
using Core.Domain.Users.Models;
using Core.Storage.Context;
using Shared.Instances;
using Shared.Repositories;

namespace Core.Application.Users.Repositories {
    public class UserRepository : Repository<User, MainContext>, IScoped {
        public UserRepository(MainContext context) : base(context) { }

        public User GetIfLastModifiedIsDifferent(long id, DateTime lastModified) {
            return Table.FirstOrDefault(e => e.Id == id && e.LastModifiedDate != lastModified);
        }

        public bool Exists(long id) {
            return Table.Any(e => e.Id == id);
        }

        public User FindByEmail(string email) {
            return Table.FirstOrDefault(e => e.Email == email);
        }
    }
}