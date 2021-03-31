using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DocSpiderTest.Base.Auth {
    public class ConnectedUsersRefresh : IHostedService, IDisposable {
        private readonly AuthList             _authList;
        private          Timer                _timer;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ConnectedUsersRefresh(AuthList             authList,
                                     IServiceScopeFactory serviceScopeFactory) {
            _authList            = authList;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken) {
            _timer = new Timer(Tick, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
            return Task.CompletedTask;
        }

        private void Tick(object state) {
            using var scope = _serviceScopeFactory.CreateScope();

            _authList.Refresh(scope.ServiceProvider);
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose() {
            _timer?.Dispose();
        }
    }
}