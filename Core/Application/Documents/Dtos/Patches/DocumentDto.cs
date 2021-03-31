using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Core.Application.Archives.Dtos.Patches;
using Core.Application.Archives.Services;
using Core.Domain.Archives.Models;
using Core.Domain.Documents.Models;
using Shared.Dtos;
using Shared.Repositories;

namespace Core.Application.Documents.Dtos.Patches {
    public class DocumentDto : DtoWithId, IPatch<Document>, ICreate, IUpdate, IToDto<Document, DocumentDto> {
        [Required, StringLength(100, MinimumLength = 8)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public ArchiveDto Archive { get; set; }
        public long       UserId  { get; set; }

        public void Apply(Document entity, Patcher patcher) {
            entity.Id          = Id;
            entity.Title       = Title;
            entity.Description = Description;
            entity.UserId      = UserId;

            if (entity.IsNew()) {
                entity.Archive    = patcher.NewInstance(Archive);
                entity.CreateDate = DateTime.Now;
            }
            else {
                patcher.AttachAndApply(Archive, entity.ArchiveId);
            }
        }

        public Expression<Func<Document, DocumentDto>> ToDto() {
            return document => new DocumentDto {
                Id    = document.Id,
                Title = document.Title,
                Archive = new ArchiveDto {
                    Id   = document.ArchiveId,
                    Name = document.Archive.Name,
                    Url  = ArchiveService.DownloadPath(document.Archive.Guid, document.Archive.Name) 
                },
                Description = document.Description,
                UserId      = document.UserId
            };
        }
    }
}