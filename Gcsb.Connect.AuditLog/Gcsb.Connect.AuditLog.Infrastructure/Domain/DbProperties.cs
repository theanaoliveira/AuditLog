using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcsb.Connect.AuditLog.Infrastructure.Domain
{
    public class DbProperties
    {
        public string ConnectionString { get; set; }
        public string TableName { get; private set; }
        public string TableHistory { get; private set; }
        public string Schema { get; set; }
        
        public DbProperties(string connectionString, string tableName, string tableHistory, string schema)
        {
            ConnectionString = connectionString;
            TableName = tableName;
            TableHistory = tableHistory;
            Schema = schema;
        }
    }
}
