using System;
using System.Linq.Expressions;
using Core.Domain.Users.Models;
using Shared.Repositories;

namespace Core.Application.Users.Dtos.Plain {
    public class UserRegisteredDto : IToDto<User, UserRegisteredDto> {
        public long   Id    { get; set; }
        public string Name  { get; set; }
        public string Email { get; set; }

        public Expression<Func<User, UserRegisteredDto>> ToDto() {
            return user => new UserRegisteredDto {
                Id    = user.Id,
                Email = user.Email,
                Name  = user.Name
            };
        }
    }
}