namespace Coinage.Data.EntityFramework.Migrations
{
    using Coinage.Domain.Concrete.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Coinage.Data.EntityFramework.Context.CoinageDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Coinage.Data.EntityFramework.Context.CoinageDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            // TODO: figure out how to run this only when databse is created?
            //context.Set<CustomerRole>().Add(new CustomerRole { Name = "Administrator", Active = true });
            //context.Set<CustomerRole>().Add(new CustomerRole { Name = "Registered", Active = true });
            //context.Set<CustomerRole>().Add(new CustomerRole { Name = "Guest", Active = true });
            //context.SaveChanges();
        }
    }
}
