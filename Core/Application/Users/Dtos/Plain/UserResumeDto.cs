using System;
using System.Linq.Expressions;
using Core.Domain.Users.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;

namespace Core.Application.Users.Dtos.Plain {
    public class UserResumeDto : IToDto<User, UserResumeDto> {
        public long   Id       { get; set; }
        public string Name     { get; set; }
        public bool   Verified { get; set; }
        public bool   Active   { get; set; }

        public Expression<Func<User, UserResumeDto>> ToDto() {
            return user => new UserResumeDto {
                Id       = user.Id,
                Name     = user.Name,
                Active   = user.Active,
                Verified = user.Verified
            };
        }
    }
}