using System;
using System.Data;
using System.IO;
using System.Linq;
using Core.Application.Archives.Repositories;
using Core.Domain.Archives.Models;
using Shared;
using Shared.Instances;
using Shared.Services;

namespace Core.Application.Archives.Services {
    public class ArchiveService : EntityService<Archive, ArchiveRepository>, IScoped {
        private readonly IConfiguration _configuration;

        public ArchiveService(ArchiveRepository repository, IConfiguration configuration) : base(repository) {
            _configuration = configuration;
        }

        public void OnUpdate(Archive archive) {
            Guid guid;

            if (archive.IsNew())
                guid = archive.Guid = Guid.NewGuid();
            else
                guid = Repository.Find(archive.Id, x => x.Guid);

            var name      = GenerateName(archive);
            var directory = Path.Combine(_configuration.WebRootPath, GenerateDirectory(guid));

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            archive.Name = name;
            var newFile = Path.Combine(directory, name);

            if (archive.OldName != null) {
                var oldFile = Path.Combine(directory, archive.OldName);

                if (archive.File == null) {
                    if (oldFile != newFile)
                        File.Move(oldFile, newFile);
                    return;
                }

                if (File.Exists(oldFile))
                    File.Delete(oldFile);
            }

            if (archive.File == null)
                return;

            using var stream = File.Create(newFile);

            archive.File.CopyTo(stream);
        }

        private static string GenerateName(Archive archive) {
            var extension = Path.GetExtension(archive.File?.FileName ?? archive.OldName);

            //remover / e extensão
            var name = Path.GetFileName(
                Path.GetFileNameWithoutExtension(archive.Name ?? archive.File?.FileName ?? archive.OldName)
            );

            return name + extension;
        }

        public static string DownloadPath(Guid guid, string name) {
            var directory = GenerateDirectory(guid);

            return '/' + Path.Combine(
                       directory,
                       name
                   );
        }

        private static string GenerateDirectory(Guid guid) {
            var str = guid.ToString("N");

            return Path.Combine(
                "files",
                Path.Combine(
                    str.Substring(0, 10).Select(x => x.ToString()).ToArray()
                )
            );
        }
    }
}