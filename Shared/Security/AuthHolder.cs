using System;

namespace Shared.Security {
    public class CurrentAuth {
        private IAuth         _current;
        private IAuthProvider _provider;
        public  Guid          SessionKey    { get; set; }
        public  long          LastAction    { get; set; }
        public  bool          Authenticated => _current != null;

        public CurrentAuth(Guid sessionKey) {
            SessionKey = sessionKey;
        }

        public void Set(IAuth auth, IAuthProvider provider) {
            _current  = auth;
            _provider = provider;
        }

        public void Logout() {
            _current  = null;
            _provider = null;
        }

        public TAuth TryGet<TAuth>()
            where TAuth : IAuth {
            if (_current is TAuth v)
                return v;

            return default;
        }

        public void Refresh(IServiceProvider provider) {
            if (_current != null && _provider != null)
                _current = _provider.Refresh(_current, provider);
        }
    }
}