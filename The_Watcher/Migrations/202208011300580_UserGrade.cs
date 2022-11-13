namespace The_Watcher.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserGrade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jewelleries", "UserGrade", c => c.Double(nullable: false));
            AddColumn("dbo.Watches", "UserGrade", c => c.Double(nullable: false));
            DropColumn("dbo.Jewelleries", "Grade");
            DropColumn("dbo.Watches", "Grade");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Watches", "Grade", c => c.Int(nullable: false));
            AddColumn("dbo.Jewelleries", "Grade", c => c.Int(nullable: false));
            DropColumn("dbo.Watches", "UserGrade");
            DropColumn("dbo.Jewelleries", "UserGrade");
        }
    }
}
