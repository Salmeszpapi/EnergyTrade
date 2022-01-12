namespace EnergyTrade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePost : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Brand",
                c => new
                    {
                        BrandId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.BrandId);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Size = c.Int(nullable: false),
                        Coffein = c.Int(nullable: false),
                        Brand_Id = c.Int(),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.Brand", t => t.Brand_Id);
            
            CreateTable(
                "dbo.StockItem",
                c => new
                    {
                        StockItemId = c.Int(nullable: false, identity: true),
                        Count = c.Int(nullable: false),
                        Product_Id = c.Int(),
                        Stock_Id = c.Int(),
                    })
                .PrimaryKey(t => t.StockItemId)
                .ForeignKey("dbo.Product", t => t.Product_Id)
                .ForeignKey("dbo.Stock", t => t.Stock_Id);
            
            CreateTable(
                "dbo.Stock",
                c => new
                    {
                        StockId = c.Int(nullable: false, identity: true),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.StockId)
                .ForeignKey("dbo.User", t => t.User_Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stock", "User_Id", "dbo.User");
            DropForeignKey("dbo.StockItem", "Stock_Id", "dbo.Stock");
            DropForeignKey("dbo.StockItem", "Product_Id", "dbo.Product");
            DropForeignKey("dbo.Product", "Brand_Id", "dbo.Brand");
            DropTable("dbo.User");
            DropTable("dbo.Stock");
            DropTable("dbo.StockItem");
            DropTable("dbo.Product");
            DropTable("dbo.Brand");
        }
    }
}
