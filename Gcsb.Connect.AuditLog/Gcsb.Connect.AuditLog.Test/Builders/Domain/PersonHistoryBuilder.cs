using Gcsb.Connect.AuditLog.Infrastructure.Domain;
using System;

namespace Gcsb.Connect.AuditLog.Test.Builders.Domain
{
    public class PersonHistoryBuilder: HistoryBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Cpf { get; set; }
    }
}
