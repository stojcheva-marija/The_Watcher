namespace The_Watcher.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShoppingCartWishList : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShoppingCarts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WishLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ShoppingCartJewelleries",
                c => new
                    {
                        ShoppingCart_Id = c.Int(nullable: false),
                        Jewellery_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ShoppingCart_Id, t.Jewellery_Id })
                .ForeignKey("dbo.ShoppingCarts", t => t.ShoppingCart_Id, cascadeDelete: true)
                .ForeignKey("dbo.Jewelleries", t => t.Jewellery_Id, cascadeDelete: true)
                .Index(t => t.ShoppingCart_Id)
                .Index(t => t.Jewellery_Id);
            
            CreateTable(
                "dbo.WatchShoppingCarts",
                c => new
                    {
                        Watch_Id = c.Int(nullable: false),
                        ShoppingCart_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Watch_Id, t.ShoppingCart_Id })
                .ForeignKey("dbo.Watches", t => t.Watch_Id, cascadeDelete: true)
                .ForeignKey("dbo.ShoppingCarts", t => t.ShoppingCart_Id, cascadeDelete: true)
                .Index(t => t.Watch_Id)
                .Index(t => t.ShoppingCart_Id);
            
            CreateTable(
                "dbo.WishListJewelleries",
                c => new
                    {
                        WishList_Id = c.Int(nullable: false),
                        Jewellery_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.WishList_Id, t.Jewellery_Id })
                .ForeignKey("dbo.WishLists", t => t.WishList_Id, cascadeDelete: true)
                .ForeignKey("dbo.Jewelleries", t => t.Jewellery_Id, cascadeDelete: true)
                .Index(t => t.WishList_Id)
                .Index(t => t.Jewellery_Id);
            
            CreateTable(
                "dbo.WishListWatches",
                c => new
                    {
                        WishList_Id = c.Int(nullable: false),
                        Watch_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.WishList_Id, t.Watch_Id })
                .ForeignKey("dbo.WishLists", t => t.WishList_Id, cascadeDelete: true)
                .ForeignKey("dbo.Watches", t => t.Watch_Id, cascadeDelete: true)
                .Index(t => t.WishList_Id)
                .Index(t => t.Watch_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WishListWatches", "Watch_Id", "dbo.Watches");
            DropForeignKey("dbo.WishListWatches", "WishList_Id", "dbo.WishLists");
            DropForeignKey("dbo.WishListJewelleries", "Jewellery_Id", "dbo.Jewelleries");
            DropForeignKey("dbo.WishListJewelleries", "WishList_Id", "dbo.WishLists");
            DropForeignKey("dbo.WatchShoppingCarts", "ShoppingCart_Id", "dbo.ShoppingCarts");
            DropForeignKey("dbo.WatchShoppingCarts", "Watch_Id", "dbo.Watches");
            DropForeignKey("dbo.ShoppingCartJewelleries", "Jewellery_Id", "dbo.Jewelleries");
            DropForeignKey("dbo.ShoppingCartJewelleries", "ShoppingCart_Id", "dbo.ShoppingCarts");
            DropIndex("dbo.WishListWatches", new[] { "Watch_Id" });
            DropIndex("dbo.WishListWatches", new[] { "WishList_Id" });
            DropIndex("dbo.WishListJewelleries", new[] { "Jewellery_Id" });
            DropIndex("dbo.WishListJewelleries", new[] { "WishList_Id" });
            DropIndex("dbo.WatchShoppingCarts", new[] { "ShoppingCart_Id" });
            DropIndex("dbo.WatchShoppingCarts", new[] { "Watch_Id" });
            DropIndex("dbo.ShoppingCartJewelleries", new[] { "Jewellery_Id" });
            DropIndex("dbo.ShoppingCartJewelleries", new[] { "ShoppingCart_Id" });
            DropTable("dbo.WishListWatches");
            DropTable("dbo.WishListJewelleries");
            DropTable("dbo.WatchShoppingCarts");
            DropTable("dbo.ShoppingCartJewelleries");
            DropTable("dbo.WishLists");
            DropTable("dbo.ShoppingCarts");
        }
    }
}
