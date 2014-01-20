namespace Coinage.Data.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Product_Add_Timestamps : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Product", "ModifiedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "ModifiedOn");
            DropColumn("dbo.Product", "CreatedOn");
        }
    }
}
