# Introduction 
Create a logs register on your database with a model and registers of any type

# Getting Started
Sample:

Create a domain of type what you need, and create a mirror of that, to save a history on your database.

Example:

I have a Person domain, and a PersonHistory:

```
using System;

namespace Domain.Person
{
    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public int Age { get; set; }

        public Person(Guid id, string name, string cpf, int age)
        {
            Id = id;
            Name = name;
            Cpf = cpf;
            Age = age;
        }
    }
}
```

In my history class, I'll inherit from "HistoryBase"

```
using Gcsb.Connect.AuditLog.Infrastructure.Domain;

namespace Infrastructure.PostgresDataAccess.Entities.Person
{
    public class PersonHistory : HistoryBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public int Age { get; set; }
    }
}

```

After create the domains, I'll create a new instance for "Context" class, like that:

```
using var context = new Context<Person, PersonHistory>(dbProperties);
```

The "dbProperties" are a configuration of the connection, tables and schema, like that:

```
namespace Gcsb.Connect.AuditLog.Infrastructure.Domain
{
    public class DbProperties
    {
        public DbProperties(string connectionString, string tableName, string tableHistory, string schema);

        public string ConnectionString { get; set; }
        public string TableName { get; }
        public string TableHistory { get; }
        public string Schema { get; set; }
    }
}

private readonly DbProperties dbProperties = new DbProperties(Environment.GetEnvironmentVariable("CONN"), "Person", "PersonHistory", "GenericDB");
```

And for the save the data, just call "SaveChangesAsync" method on context above

Example:

```
var dbProperties = new DbProperties(Environment.GetEnvironmentVariable("CONN"), "Person", "PersonHistory", "GenericDB");

using var context = new Context<Person, PersonHistory>(dbProperties);
context.GenericTable.Add(personObject);
await context.SaveChangesAsync();
```

Enjoy!


# Contribute
TODO: Explain how other users and developers can contribute to make your code better. 

If you want to learn more about creating good readme files then refer the following [guidelines](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-a-readme?view=azure-devops). You can also seek inspiration from the below readme files:
- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)
