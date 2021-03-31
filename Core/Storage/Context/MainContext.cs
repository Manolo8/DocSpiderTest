using System;
using System.Linq;
using Core.Application.Archives.Services;
using Core.Domain.Archives.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Storage.Context {
    public class MainContext : DbContext {
        private readonly IServiceProvider _provider;

        public MainContext(DbContextOptions<MainContext> options, IServiceProvider provider) : base(options) {
            _provider = provider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfigurationsFromAssembly(
                GetType().Assembly,
                p => p.GetInterfaces().Contains(typeof(IMainContextConfiguration)));

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges() {
            var entries = ChangeTracker
                          .Entries()
                          .ToList();

            entries.ForEach(x => {
                if (x.Entity is Archive archive)
                    _provider.GetService<ArchiveService>()?.OnUpdate(archive);
            });


            return base.SaveChanges();
        }
    }
}