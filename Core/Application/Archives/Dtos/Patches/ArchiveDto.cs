using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Core.Domain.Archives.Models;
using Microsoft.AspNetCore.Http;
using Shared.Dtos;
using Shared.Repositories;
using Shared.Utils.Other;
using ValidationMessages = Shared.ValidationMessages;

namespace Core.Application.Archives.Dtos.Patches {
    public class ArchiveDto : DtoWithId, IPatch<Archive>, IValidatableObject {
        [MaxLength(128)]
        public string Name { get; set; }

        public IFormFile File { get; set; }
        public string    Url  { get; set; }

        public void Apply(Archive entity, Patcher patcher) {
            entity.Id = Id;

            if (!entity.IsNew()) {
                entity.OldName = patcher.Value(entity, x => x.Name);
            }

            entity.Name = Name;
            entity.File = File;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            var validations = new List<ValidationResult>();

            if (Id == 0 && File == null)
                validations.Error(ValidationMessages.Get<RequiredAttribute>(), nameof(File));

            if (File != null) {
                var extension = Path.GetExtension(File.FileName).ToLower();

                if (extension == ".exe" || extension == ".zip" || extension == ".bat")
                    validations.Error("Formato de arquivo não permitido!", nameof(File));
            }


            return validations;
        }
    }
}