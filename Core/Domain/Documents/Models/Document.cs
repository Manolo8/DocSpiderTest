using System;
using Core.Domain.Archives.Models;
using Core.Domain.Users.Models;
using Shared.Entities;

namespace Core.Domain.Documents.Models {
    public class Document : Entity {
        public string   Title       { get; set; }
        public string   Description { get; set; }
        public DateTime CreateDate  { get; set; }
        public long     ArchiveId   { get; set; }
        public long     UserId      { get; set; }

        public Archive Archive { get; set; }
        public User    User    { get; set; }
    }
}