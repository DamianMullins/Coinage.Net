namespace Coinage.Data.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Customer_Add_Attributes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "FirstName", c => c.String());
            AddColumn("dbo.Customer", "LastName", c => c.String());
            AddColumn("dbo.Customer", "Phone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customer", "Phone");
            DropColumn("dbo.Customer", "LastName");
            DropColumn("dbo.Customer", "FirstName");
        }
    }
}
