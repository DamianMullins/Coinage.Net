namespace Coinage.Data.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Customer_Update_CustomerRoleRelationship : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Customer_CustomerRole", name: "Customer_Id", newName: "CustomerId");
            RenameColumn(table: "dbo.Customer_CustomerRole", name: "CustomerRole_Id", newName: "CustomerRoleId");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Customer_CustomerRole", name: "CustomerRoleId", newName: "CustomerRole_Id");
            RenameColumn(table: "dbo.Customer_CustomerRole", name: "CustomerId", newName: "Customer_Id");
        }
    }
}
