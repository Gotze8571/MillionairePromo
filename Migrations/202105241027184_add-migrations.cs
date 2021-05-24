namespace MillionaireWinnerPicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addmigrations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserIdentity", "TimeStamp", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserIdentity", "TimeStamp");
        }
    }
}
