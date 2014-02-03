namespace Coinage.Data.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Customer_Add_CustomerGuid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "CustomerGuid", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customer", "CustomerGuid");
        }
    }
}
