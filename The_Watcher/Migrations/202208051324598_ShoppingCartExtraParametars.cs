namespace The_Watcher.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShoppingCartExtraParametars : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShoppingCarts", "Name", c => c.String());
            AddColumn("dbo.ShoppingCarts", "Surname", c => c.String());
            AddColumn("dbo.ShoppingCarts", "Phone", c => c.String());
            AddColumn("dbo.WishLists", "Name", c => c.String());
            AddColumn("dbo.WishLists", "Surname", c => c.String());
            AddColumn("dbo.WishLists", "Phone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WishLists", "Phone");
            DropColumn("dbo.WishLists", "Surname");
            DropColumn("dbo.WishLists", "Name");
            DropColumn("dbo.ShoppingCarts", "Phone");
            DropColumn("dbo.ShoppingCarts", "Surname");
            DropColumn("dbo.ShoppingCarts", "Name");
        }
    }
}
