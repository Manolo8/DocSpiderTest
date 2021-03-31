using System;
using Core.Application.Users.Repositories;
using Core.Domain.Users.Models;
using Microsoft.Extensions.DependencyInjection;
using Shared.Instances;
using Shared.Security;

namespace Core.Application.Auth.Impl {
    public class UserAuthProvider : IAuthProvider, ISingleton {
        public IAuth Refresh(IAuth old, IServiceProvider provider) {
            var repository = provider.GetService<UserRepository>();

            if (!repository?.Exists(old.Id) ?? true)
                return null;

            var debtor = repository?.GetIfLastModifiedIsDifferent(old.Id, old.LastModified);

            return debtor != null ? ToAuth(debtor) : old;
        }

        public void Load(User user, CurrentAuth auth) {
            auth.Set(ToAuth(user), this);
        }

        private static UserAuth ToAuth(User user) {
            return new UserAuth(user.Id, user.Name, user.LastModifiedDate);
        }
    }
}