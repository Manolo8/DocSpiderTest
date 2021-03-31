using System;

namespace Shared.Security {
    public interface IAuthProvider {
        public IAuth Refresh(IAuth old, IServiceProvider provider);
    }
}