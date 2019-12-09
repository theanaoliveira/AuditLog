using System;
using System.ComponentModel.DataAnnotations;

namespace Gcsb.Connect.AuditLog.Test.Builders.Domain
{
    public class PersonBuilder
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Cpf { get; set; }

        public static PersonBuilder New()
        {
            return new PersonBuilder()
            {
                Id = Guid.NewGuid(),
                Name = "Cauã Rodrigues",
                Age = 20,
                Cpf = "654.667.510-22"
            };
        }

        public PersonBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public PersonBuilder WithAge(int age)
        {
            Age = age;
            return this;
        }

        public PersonBuilder WithCpf(string cpf)
        {
            Cpf = cpf;
            return this;
        }
    }
}
