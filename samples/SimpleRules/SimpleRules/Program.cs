﻿using NRules.Fluent;

namespace NRules.Samples.SimpleRules
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var dwelling = new Dwelling {Address = "1 Main Street, New York, NY", Type = DwellingTypes.SingleHouse};
            var dwelling2 = new Dwelling {Address = "2 Main Street, New York, NY", Type = DwellingTypes.SingleHouse};
            var policy1 = new Policy {Name = "Silver", PolicyType = PolicyTypes.Home, Price = 1200, Dwelling = dwelling};
            var policy2 = new Policy {Name = "Gold", PolicyType = PolicyTypes.Home, Price = 2300, Dwelling = dwelling2};
            var customer1 = new Customer {Name = "John Do", Age = 22, Sex = SexTypes.Male, Policy = policy1};
            var customer2 = new Customer {Name = "Emily Brown", Age = 32, Sex = SexTypes.Female, Policy = policy2};

            var repository = new RuleRepository();
            repository.Load(x => x.From(typeof (Program).Assembly));
            var ruleSets = repository.GetRuleSets();

            var compiler = new RuleCompiler();
            ISessionFactory factory = compiler.Compile(ruleSets);
            ISession session = factory.CreateSession();

            session.Insert(policy1);
            session.Insert(policy2);
            session.Insert(customer1);
            session.Insert(customer2);
            session.Insert(dwelling);
            session.Insert(dwelling2);

            customer1.Age = 10;
            session.Update(customer1);

            session.Retract(customer2);

            session.Fire();

            session.Insert(customer2);

            session.Fire();

            customer1.Age = 30;
            session.Update(customer1);

            session.Fire();
        }
    }
}