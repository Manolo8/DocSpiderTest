using Core.Domain.Archives.Models;
using Core.Storage.Context;
using Shared.Instances;
using Shared.Repositories;

namespace Core.Application.Archives.Repositories {
    public class ArchiveRepository : Repository<Archive, MainContext>, IScoped {
        public ArchiveRepository(MainContext context) : base(context) { }
    }
}