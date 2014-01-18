namespace Coinage.Data.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Product_Add_IsFeatured : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "IsFeatured", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "IsFeatured");
        }
    }
}
