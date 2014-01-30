namespace Coinage.Data.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Basket",
                c => new
                    {
                        BasketId = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.BasketId);
            
            CreateTable(
                "dbo.BasketItem",
                c => new
                    {
                        BasketItemId = c.Int(nullable: false, identity: true),
                        BasketId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.BasketItemId)
                .ForeignKey("dbo.Basket", t => t.BasketId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.BasketId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 500),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        Description = c.String(),
                        IsFeatured = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BasketItem", "ProductId", "dbo.Product");
            DropForeignKey("dbo.BasketItem", "BasketId", "dbo.Basket");
            DropIndex("dbo.BasketItem", new[] { "ProductId" });
            DropIndex("dbo.BasketItem", new[] { "BasketId" });
            DropTable("dbo.Product");
            DropTable("dbo.BasketItem");
            DropTable("dbo.Basket");
        }
    }
}
