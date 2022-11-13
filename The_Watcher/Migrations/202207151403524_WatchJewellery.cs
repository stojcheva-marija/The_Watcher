namespace The_Watcher.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WatchJewellery : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Jewelleries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Brand = c.String(),
                        Gender = c.String(),
                        ProductCode = c.String(),
                        Price = c.Int(nullable: false),
                        Discount = c.Int(nullable: false),
                        ImageURL = c.String(),
                        Category = c.String(),
                        Color = c.String(),
                        Length = c.Int(nullable: false),
                        Material = c.String(),
                        Avaliability = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Watches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Gender = c.String(),
                        Brand = c.String(),
                        ProductCode = c.String(),
                        Price = c.Int(nullable: false),
                        Discount = c.Int(nullable: false),
                        ImageURL = c.String(),
                        Category = c.String(),
                        Color = c.String(),
                        WatchWidth = c.Int(nullable: false),
                        WatchMaterial = c.String(),
                        StrapMaterial = c.String(),
                        WatchShape = c.String(),
                        Avaliability = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Watches");
            DropTable("dbo.Jewelleries");
        }
    }
}
