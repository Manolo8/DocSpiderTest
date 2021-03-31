using Core.Application.Auth.Impl;
using Core.Application.Users.Dtos.Patches;
using Core.Application.Users.Dtos.Plain;
using Core.Application.Users.Repositories;
using Core.Domain.Users.Models;
using Shared.Exceptions;
using Shared.Instances;
using Shared.Security;
using Shared.Services;

namespace Core.Application.Users.Services {
    public class UserService : EntityService<User, UserRepository>, IScoped {
        private readonly UserAuthProvider _userAuthProvider;


        public UserService(UserRepository   repository,
                           UserAuthProvider userAuthProvider
        ) : base(repository) {
            _userAuthProvider = userAuthProvider;
        }

        #region Auth

        public void Register(UserDto dto) {
            dto.Verified = true;
            dto.Active   = true;

            if (Repository.Any(x => x.Email == dto.Email.ToLower()))
                throw new ValidationFieldException(nameof(dto.Email), "Email em uso!");

            Repository.Create(dto);
        }

        public void Authenticate(CurrentAuth auth, UserCredentialsDto credentials) {
            var user = Repository.FindByEmail(credentials.Email.ToLower());

            if (user == null || user.Password != credentials.Password)
                throw new ValidationFieldException(nameof(credentials.Password), "Nome ou senha incorretos!");

            if (!user.Active)
                throw new ValidationFieldException(nameof(credentials.Email), "Sua conta está desativada!");

            _userAuthProvider.Load(user, auth);
        }

        #endregion
    }
}