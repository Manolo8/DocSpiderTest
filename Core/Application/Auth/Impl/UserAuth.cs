using System;
using System.Collections.Generic;
using System.Linq;
using Core.Application.Users.Dtos.Plain;
using Shared;
using Shared.Security;

namespace Core.Application.Auth.Impl {
    public class UserAuth : IAuth {
        public long     Id           { get; }
        public string   Name         { get; }
        public DateTime LastModified { get; }

        public HashSet<string> CurrentCompanyRoles { get; set; }

        public UserAuth(long id, string name, DateTime lastModified) {
            Id           = id;
            Name         = name;
            LastModified = lastModified;
        }

        public bool HasRole(string name) {
            return name == RegisteredRoles.User || (CurrentCompanyRoles?.Contains(name) ?? false);
        }

        public UserAuthDto ToDto() {
            return new UserAuthDto {
                Id                  = Id,
                Name                = Name,
                LastModified        = LastModified,
                Roles = CurrentCompanyRoles?.ToArray()
            };
        }
    }
}