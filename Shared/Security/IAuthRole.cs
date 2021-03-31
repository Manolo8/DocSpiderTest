using System.Collections.Generic;

namespace Shared.Security {
    public interface IAuthRole<TRole> : IAuth
        where TRole : IRole {
        List<TRole> Roles { get; set; }
    }
}