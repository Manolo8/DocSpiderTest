using Core.Application.Documents.Dtos.Filters;
using Core.Application.Documents.Dtos.Patches;
using Core.Application.Documents.Dtos.Plain;
using Core.Application.Documents.Repositories;
using Core.Domain.Documents.Models;
using Shared.Exceptions;
using Shared.Instances;
using Shared.Services;
using Shared.Utils.Filters;

namespace Core.Application.Documents.Services {
    public class DocumentService : CrudService<DocumentDto, Document, DocumentRepository>, IScoped {
        public DocumentService(DocumentRepository repository) : base(repository) { }

        public FilterResult<DocumentToViewListDto> SearchToViewList(DocumentFilterDto filter) {
            return Repository.Search<DocumentToViewListDto>(filter);
        }

        public DocumentToViewListDto FindToViewInfo(long id) {
            return Repository.Find<DocumentToViewListDto>(id);
        }

        protected override Document CreateReturnEntity(DocumentDto model) {
            if (Repository.Any(x => x.Title == model.Title))
                throw new ValidationFieldException(nameof(model.Title), "Título em uso!");

            return base.CreateReturnEntity(model);
        }

        protected override Document UpdateReturnEntity(DocumentDto model) {
            if (Repository.Any(x => x.Title == model.Title && x.Id != model.Id))
                throw new ValidationFieldException(nameof(model.Title), "Título em uso!");

            return base.UpdateReturnEntity(model);
        }
    }
}