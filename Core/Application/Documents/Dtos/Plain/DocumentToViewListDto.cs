using System;
using System.Linq.Expressions;
using Core.Application.Archives.Services;
using Core.Domain.Documents.Models;
using Shared.Repositories;

namespace Core.Application.Documents.Dtos.Plain {
    public class DocumentToViewListDto : IToDto<Document, DocumentToViewListDto> {
        public long     Id          { get; set; }
        public string   Title       { get; set; }
        public string   Description { get; set; }
        public string   ArchiveName { get; set; }
        public string   ArchiveUrl  { get; set; }
        public DateTime CreateDate  { get; set; }

        public Expression<Func<Document, DocumentToViewListDto>> ToDto() {
            return document => new DocumentToViewListDto {
                Id          = document.Id,
                Title       = document.Title,
                Description = document.Description,
                CreateDate  = document.CreateDate,
                ArchiveUrl  = ArchiveService.DownloadPath(document.Archive.Guid, document.Archive.Name),
                ArchiveName = document.Archive.Name,
            };
        }
    }
}