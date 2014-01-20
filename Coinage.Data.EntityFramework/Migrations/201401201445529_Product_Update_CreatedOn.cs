namespace Coinage.Data.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Product_Update_CreatedOn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Product", "CreatedOn", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Product", "CreatedOn", c => c.DateTime(nullable: false));
        }
    }
}
