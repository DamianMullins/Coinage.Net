namespace Coinage.Data.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        PropertyId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 500),
                        IsFeatured = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.PropertyId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Product");
        }
    }
}
