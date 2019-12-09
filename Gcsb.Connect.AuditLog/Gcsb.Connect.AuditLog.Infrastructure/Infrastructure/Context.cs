using Gcsb.Connect.AuditLog.Infrastructure.Domain;
using Gcsb.Connect.AuditLog.Infrastructure.Infrastructure.Map;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Gcsb.Connect.AuditLog.Infrastructure.Infrastructure
{
    public class Context<T, THistory> : DbContext where T : class where THistory : class, new()
    {
        public DbSet<T> GenericTable { get; set; }

        public DbSet<THistory> GenericHistoryTable { get; set; }

        public DbProperties Properties { get; set; }

        public Context(DbProperties properties)
        {
            Properties = properties;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(Properties.ConnectionString))
            {
                optionsBuilder.UseNpgsql(Properties.ConnectionString, npgsqlOptionsAction: options =>
                {
                    options.EnableRetryOnFailure(2, TimeSpan.FromSeconds(5), new List<string>());
                    options.MigrationsHistoryTable("_MigrationHistory", Properties.Schema);
                });
            }
            else
            {
                optionsBuilder.UseInMemoryDatabase("Test");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GenericMap<T>(Properties.TableName, Properties.Schema));
            modelBuilder.ApplyConfiguration(new GenericMap<THistory>(Properties.TableHistory, Properties.Schema));

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var auditEntries = OnBeforeSaveChanges();

            this.GenericHistoryTable.AddRange(auditEntries);
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            return result;
        }

        private List<THistory> OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();

            var auditEntries = new List<THistory>();
            var auditLog = new AuditLog<THistory>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntries.Add(auditLog.MakeHistory(entry.CurrentValues.ToObject(), EntityState.Added, "Insert"));
                        break;
                    case EntityState.Deleted:
                        entry.OriginalValues.SetValues(entry.GetDatabaseValuesAsync().Result);
                        auditEntries.Add(auditLog.MakeHistory(entry.OriginalValues.ToObject(), EntityState.Deleted, "Delete"));
                        break;
                    case EntityState.Modified:
                        entry.OriginalValues.SetValues(entry.GetDatabaseValuesAsync().Result);
                        auditEntries.Add(auditLog.MakeHistory(entry.OriginalValues.ToObject(), EntityState.Deleted, "Update - Old Value"));
                        auditEntries.Add(auditLog.MakeHistory(entry.CurrentValues.ToObject(), EntityState.Added, "Update - New value"));
                        break;
                }
            }

            return auditEntries;
        }
    }
}
