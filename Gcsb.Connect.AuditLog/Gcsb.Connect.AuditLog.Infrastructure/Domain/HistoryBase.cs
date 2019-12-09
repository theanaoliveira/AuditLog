using System;
using System.ComponentModel.DataAnnotations;

namespace Gcsb.Connect.AuditLog.Infrastructure.Domain
{
    public abstract class HistoryBase
    {
        [Key]
        public Guid IdLog { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string Action { get; set; }
    }
}
