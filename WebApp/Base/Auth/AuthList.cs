using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Security;

namespace DocSpiderTest.Base.Auth {
    public class AuthList {
        private readonly Dictionary<Guid, CurrentAuth> _auths;

        public AuthList() {
            _auths = new Dictionary<Guid, CurrentAuth>();
        }

        public CurrentAuth Anonymous() {
            return new CurrentAuth(Guid.Empty);
        }

        public CurrentAuth Get(Guid guid) {
            CurrentAuth auth;

            lock (_auths) {
                _auths.TryGetValue(guid, out auth);

                if (auth == null)
                    _auths[guid] = auth = new CurrentAuth(guid);
            }

            auth.LastAction = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            return auth;
        }

        public void Refresh(IServiceProvider provider) {
            lock (_auths) {
                #region Remove

                var removeDate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - (1000 * 60 * 60); // 1 hour

                var remove =
                    _auths.Where(e => e.Value.LastAction < removeDate)
                          .Select(e => e.Key)
                          .ToList();

                remove.ForEach(key => _auths.Remove(key));

                #endregion

                #region Refresh

                var refresh =
                    _auths.Select(e => e.Value)
                          .ToList();

                //todo criar um sistema para atualizar todos usando apenas 1 query
                refresh.ForEach(e => e.Refresh(provider));

                #endregion
            }
        }
    }
}