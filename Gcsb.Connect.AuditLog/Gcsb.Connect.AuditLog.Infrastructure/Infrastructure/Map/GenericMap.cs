using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gcsb.Connect.AuditLog.Infrastructure.Infrastructure.Map
{
    public class GenericMap<T> : IEntityTypeConfiguration<T> where T : class
    {
        public string Table { get; private set; }
        public string Schema { get; private set; }

        public GenericMap(string table, string schema)
        {
            Table = table;
            Schema = schema;
        }

        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(Table, Schema);
        }
    }
}
