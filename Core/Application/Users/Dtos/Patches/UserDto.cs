using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Core.Domain.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared.Dtos;
using Shared.Repositories;
using Shared.Utils.Other;

namespace Core.Application.Users.Dtos.Patches {
    public class UserDto : DtoWithId, IValidatableObject, IPatch<User>, ICreate, IUpdate,
                           IToDto<User, UserDto> {
        [Required, StringLength(128, MinimumLength = 8)]
        public string Name { get; set; }

        [Required, MaxLength(128), MinLength(8), EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(48), MinLength(8)]
        [Compare(nameof(ConfirmPassword), ErrorMessage = "As senhas são diferentes!")]
        public string Password { get; set; }

        [Required, MaxLength(48), MinLength(8)]
        [Compare(nameof(Password), ErrorMessage = "As senhas são diferentes!")]
        public string ConfirmPassword { get; set; }

        public bool Active   { get; set; } = true;
        public bool Verified { get; set; } = false;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            var errors = new List<ValidationResult>();

            if (Id != 0)
                return errors;

            if (Password == null)
                errors.Error(ValidationMessages.Required, nameof(Password));

            return errors;
        }

        public void Apply(User entity, Patcher patcher) {
            entity.Name   = Name;
            entity.Active = Active;

            if (entity.IsNew()) {
                entity.Password = Password;
                entity.Verified = Verified;
                entity.Email    = Email?.ToLower();
            }
            else {
                if (Password != null)
                    entity.Password = Password;

                if (entity.Email == Email)
                    return;

                entity.Email    = Email;
                entity.Verified = false;
            }
        }

        public Expression<Func<User, UserDto>> ToDto() {
            return user => new UserDto {
                Id       = user.Id,
                Active   = user.Active,
                Email    = user.Email,
                Name     = user.Name,
                Verified = user.Verified
            };
        }
    }
}