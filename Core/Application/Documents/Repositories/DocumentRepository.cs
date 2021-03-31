using Core.Domain.Documents.Models;
using Core.Storage.Context;
using Shared.Instances;
using Shared.Repositories;

namespace Core.Application.Documents.Repositories {
    public class DocumentRepository : Repository<Document, MainContext>, IScoped {
        public DocumentRepository(MainContext context) : base(context) { }
    }
}