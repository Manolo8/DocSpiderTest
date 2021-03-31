using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Shared.Security;

namespace DocSpiderTest.Base.Middlewares {
    public class AuthMiddleware {
        private const           string          AuthorizationMiddlewareInvokedWithEndpointKey   = "__AuthorizationMiddlewareWithEndpointInvoked";
        private static readonly object          AuthorizationMiddlewareWithEndpointInvokedValue = new object();
        private readonly        RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task Invoke(HttpContext context, CurrentAuth holder) {
            context.Items[AuthorizationMiddlewareInvokedWithEndpointKey] = AuthorizationMiddlewareWithEndpointInvokedValue;

            var endpoint = context.GetEndpoint();

            if (endpoint == null) {
                await _next(context);
                return;
            }

            var authorizeData = endpoint.Metadata.GetOrderedMetadata<IAuthorizeData>() ?? Array.Empty<IAuthorizeData>();

            if (endpoint.Metadata.GetMetadata<IAllowAnonymous>() != null) {
                await _next(context);
                return;
            }

            if (authorizeData.Any()) {
                if (!holder.Authenticated) {
                    context.Response.Redirect("/Auth/Login");
                    return;
                }

                var auth = holder.TryGet<IAuth>();

                if (authorizeData.Any(data => data.Roles != null && !auth.HasRole(data.Roles))) {
                    await context.Response.WriteAsync("fail");
                    return;
                }
            }

            await _next(context);
        }
    }
}