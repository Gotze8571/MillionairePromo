namespace MillionaireWinnerPicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewStartterPromo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Export",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportName = c.String(),
                        ExportedDate = c.DateTime(nullable: false),
                        LoginUser = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Export");
        }
    }
}
