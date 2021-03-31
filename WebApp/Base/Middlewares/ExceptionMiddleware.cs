using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shared.Dtos;
using Shared.Exceptions;

namespace DocSpiderTest.Base.Middlewares {
    public class ExceptionMiddleware {
        private static   JsonSerializerSettings _settings = new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};
        private readonly RequestDelegate        _next;

        public ExceptionMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task Invoke(HttpContext context) {
            try {
                await _next(context);
            }
            catch (ValidationException e) {
                await Write(context, null, e.Errors);
            }
            catch (ValidationFieldException e) {
                await Write(context, null, FieldError.Of(e.Field, e.Error));
            }
            catch (NotAllowedException e) {
                await Write(context, e.Message);
            }
        }

        private static async Task Write(HttpContext context, string message, IEnumerable<FieldError> errors = null) {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode  = StatusCodes.Status400BadRequest;

            var error = ResultBuilder.Error<object>(message, errors);

            await context.Response.WriteAsync(JsonConvert.SerializeObject(error, _settings));
        }
    }
}