using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Shared.Entities;

namespace Core.Domain.Archives.Models {
    public class Archive : Entity {
        public string Name { get; set; }
        public Guid   Guid { get; set; }
        
        [NotMapped]
        public IFormFile File { get; set; }
        [NotMapped]
        public string OldName { get; set; }
    }
}