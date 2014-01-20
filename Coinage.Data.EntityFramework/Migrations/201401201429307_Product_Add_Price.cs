namespace Coinage.Data.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Product_Add_Price : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "Price", c => c.Decimal(nullable: false, storeType: "money"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "Price");
        }
    }
}
