namespace gtv_tele.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Master_Upload_Detail1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Master_Upload_Detail", "NoHP", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Master_Upload_Detail", "NoHP");
        }
    }
}
