using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DocSpiderTest.Base.Auth {
    public static class ConnectedExtension {
        public static void AddConnectedUsers(this IServiceCollection services) {
            services.AddSingleton<AuthList>();
            services.AddScoped(provider => {
                var context   = provider.GetService<IHttpContextAccessor>()?.HttpContext;
                var connected = provider.GetService<AuthList>();

                var endpoint = context?.GetEndpoint();

                //Se não tiver endpoint, deixa passar
                if (endpoint == null)
                    return connected?.Anonymous();

                context.Request.Cookies.TryGetValue("sid", out var sidStr);

                if (sidStr != null && Guid.TryParse(sidStr, out var sid))
                    return connected?.Get(sid);

                sid = Guid.NewGuid();

                context.Response.Cookies.Append("sid", sid.ToString());

                return connected?.Get(sid);
            });
            services.AddHostedService<ConnectedUsersRefresh>();
        }
    }
}