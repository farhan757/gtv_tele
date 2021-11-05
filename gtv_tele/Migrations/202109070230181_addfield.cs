namespace gtv_tele.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Master_Upload_Detail", "Short_Link", c => c.String());
            AddColumn("dbo.Master_Upload_Detail", "Path_PDF", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Master_Upload_Detail", "Path_PDF");
            DropColumn("dbo.Master_Upload_Detail", "Short_Link");
        }
    }
}
