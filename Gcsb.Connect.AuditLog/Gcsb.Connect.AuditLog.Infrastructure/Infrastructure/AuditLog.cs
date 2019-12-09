using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Gcsb.Connect.AuditLog.Infrastructure.Infrastructure
{
    public class AuditLog<THistory> where THistory : class, new()
    {
        public THistory MakeHistory(object entry, string action)
        {
            var log = new THistory();
            var propertiesHistory = log.GetType().GetProperties();

            UpdateProp(propertiesHistory.Where(w => w.Name.Contains("IdLog")).FirstOrDefault(), log, Guid.NewGuid());
            UpdateProp(propertiesHistory.Where(w => w.Name.Contains("LastUpdate")).FirstOrDefault(), log, DateTime.UtcNow);
            UpdateProp(propertiesHistory.Where(w => w.Name.Contains("Action")).FirstOrDefault(), log, action);

            var properties = entry.GetType().GetProperties().ToList();

            properties.ForEach(property =>
            {
                var propHistory = propertiesHistory.Where(w => w.Name.Equals(property.Name)).FirstOrDefault();
                propHistory.SetValue(log, property.GetValue(entry, null));
            });

            return log;
        }

        void UpdateProp(PropertyInfo property, THistory log, object value)
        {
            property.SetValue(log, value);
        }
    }
}
