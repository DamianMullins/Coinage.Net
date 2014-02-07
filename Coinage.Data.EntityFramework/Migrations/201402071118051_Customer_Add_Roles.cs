namespace Coinage.Data.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Customer_Add_Roles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerRole",
                c => new
                    {
                        CustomerRoleId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerRoleId);
            
            CreateTable(
                "dbo.Customer_CustomerRole",
                c => new
                    {
                        Customer_Id = c.Int(nullable: false),
                        CustomerRole_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Customer_Id, t.CustomerRole_Id })
                .ForeignKey("dbo.Customer", t => t.Customer_Id, cascadeDelete: true)
                .ForeignKey("dbo.CustomerRole", t => t.CustomerRole_Id, cascadeDelete: true)
                .Index(t => t.Customer_Id)
                .Index(t => t.CustomerRole_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customer_CustomerRole", "CustomerRole_Id", "dbo.CustomerRole");
            DropForeignKey("dbo.Customer_CustomerRole", "Customer_Id", "dbo.Customer");
            DropIndex("dbo.Customer_CustomerRole", new[] { "CustomerRole_Id" });
            DropIndex("dbo.Customer_CustomerRole", new[] { "Customer_Id" });
            DropTable("dbo.Customer_CustomerRole");
            DropTable("dbo.CustomerRole");
        }
    }
}
