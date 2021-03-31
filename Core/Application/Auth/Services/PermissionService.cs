using System;
using Core.Application.Documents.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Shared.Exceptions;
using Shared.Instances;

namespace Core.Application.Auth.Services {
    public class PermissionService : IScoped {
        private readonly IServiceProvider _provider;

        public PermissionService(IServiceProvider provider) {
            _provider = provider;
        }

        private T Get<T>() {
            var value = _provider.GetService<T>();

            if (value == null)
                Throw("Error");

            return value;
        }

        public bool IsOwnerOfDocument(long userId, long documentId) {
            return Get<DocumentRepository>().Any(x => x.Id == documentId && x.UserId == userId);
        }

        private static void Throw(string message) {
            throw new NotAllowedException(message);
        }
    }
}