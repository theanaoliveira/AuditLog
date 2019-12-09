using FluentAssertions;
using Gcsb.Connect.AuditLog.Infrastructure.Domain;
using Gcsb.Connect.AuditLog.Infrastructure.Infrastructure;
using Gcsb.Connect.AuditLog.Test.Builders.Domain;
using System.Linq;
using Xunit;

namespace Gcsb.Connect.AuditLog.Test.Cases.Repositories
{
    public class IncludeHistoryTest
    {
        [Fact]
        public void ShouldIncludeNewObject()
        {
            var person = PersonBuilder.New();
            var properties = new DbProperties("", "Person", "PersonHistory", "test");
            var context = new Context<PersonBuilder, PersonHistoryBuilder>(properties);

            context.GenericTable.Add(person);
            context.SaveChangesAsync();

            person = person.WithAge(25);

            context.GenericTable.Update(person);
            context.SaveChangesAsync();

            var history = context.GenericHistoryTable.ToList();

            history.Should().HaveCountGreaterThan(0);
        }        
    }
}
